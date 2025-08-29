using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Data;
using System.Windows.Threading;
using System.Xml;
using Microsoft.Win32.SafeHandles;
using WPFShared;

namespace GimaSoft.Business.GINA
{
	// Token: 0x02000022 RID: 34
	public class GINACharacter : BindableObject
	{
		// Token: 0x06000165 RID: 357
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern int DeviceIoControl(SafeFileHandle hDevice, int dwIoControlCode, ref short lpInBuffer, int nInBufferSize, IntPtr lpOutBuffer, int nOutBufferSize, ref int lpBytesReturned, IntPtr lpOverlapped);

		// Token: 0x06000166 RID: 358 RVA: 0x00006F20 File Offset: 0x00005120
		static GINACharacter()
		{
			BindableObject.SetDependentProperties(typeof(GINACharacter));
			GINACharacter.ScheduledArchiveTimer = new DispatcherTimer();
			GINACharacter.ScheduledArchiveTimer.Tick += delegate(object o, EventArgs e)
			{
				GINACharacter.ScheduledArchiveTimer.IsEnabled = false;
				foreach (GINACharacter ginacharacter in GINACharacter.All)
				{
					if (ginacharacter.ArchiveNeeded)
					{
						ginacharacter.ArchiveLog();
					}
					ginacharacter.PurgeLogs();
				}
				GINACharacter.ScheduledArchiveTimer.Interval = DateTime.Today.AddDays(1.0).AddMinutes(1.0) - DateTime.Now;
				GINACharacter.ScheduledArchiveTimer.IsEnabled = true;
			};
			GINACharacter.ScheduledArchiveTimer.Interval = DateTime.Today.AddDays(1.0).AddMinutes(1.0) - DateTime.Now;
			GINACharacter.ScheduledArchiveTimer.IsEnabled = true;
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00006FB3 File Offset: 0x000051B3
		public static void RegisterDependentProperties(Type type)
		{
			BindableObject.RegisterDependentProperty(type, "DisplayName", "Name", null);
			BindableObject.RegisterDependentProperty(type, "DisplayName", "Server", null);
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000168 RID: 360 RVA: 0x00006FD7 File Offset: 0x000051D7
		public static ObservableCollection<GINACharacter> All
		{
			get
			{
				if (GINACharacter._All == null)
				{
					GINACharacter._All = new ObservableCollection<GINACharacter>();
					CollectionViewSource.GetDefaultView(GINACharacter._All).SortDescriptions.Add(new SortDescription("DisplayName", ListSortDirection.Ascending));
				}
				return GINACharacter._All;
			}
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00007010 File Offset: 0x00005210
		public static void Clear()
		{
			List<GINACharacter> list = GINACharacter.All.ToList<GINACharacter>();
			GINACharacter.All.Clear();
			foreach (GINACharacter ginacharacter in list)
			{
				ginacharacter.Dispose();
			}
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600016A RID: 362 RVA: 0x00007074 File Offset: 0x00005274
		// (remove) Token: 0x0600016B RID: 363 RVA: 0x000070AC File Offset: 0x000052AC
		public event TriggerMatchedHandler TriggerMatched;

		// Token: 0x0600016C RID: 364 RVA: 0x00007120 File Offset: 0x00005320
		protected virtual void OnTriggerMatched(TriggerMatchedEventArgs e)
		{
			if (this.TriggerMatched != null)
			{
				this.TriggerMatched(this, e);
			}
			if (e.TriggerFilter.Trigger != null && e.TriggerFilter.Trigger.Category != null)
			{
				e.TriggerFilter.TextOverlay.AddMatch(e, false);
			}
			if (e.MatchType == MatchTypes.Standard && e.TriggerFilter.Trigger != null && e.TriggerFilter.Trigger.TimerType != TimerTypes.NoTimer && e.TriggerFilter.Trigger.Category != null)
			{
				BehaviorGroup timerOverlay = e.TriggerFilter.TimerOverlay;
				switch (e.TriggerFilter.Trigger.TimerStartBehavior)
				{
				case TimerStartBehaviors.StartNewTimer:
					timerOverlay.AddMatch(e.Clone(MatchTypes.Timer), false);
					break;
				case TimerStartBehaviors.RestartTimer:
					timerOverlay.RestartTimer(e.Clone(MatchTypes.Timer));
					break;
				case TimerStartBehaviors.IgnoreIfRunning:
					timerOverlay.AddMatch(e.Clone(MatchTypes.Timer), true);
					break;
				}
			}
			if (e.TriggerFilter.Trigger.CopyToClipboard)
			{
				base.Dispatcher.Invoke(DispatcherPriority.Normal, new ThreadStart(delegate
				{
					try
					{
						Clipboard.SetText(e.ClipboardText);
					}
					catch
					{
					}
				}));
			}
		}

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x0600016D RID: 365 RVA: 0x00007298 File Offset: 0x00005498
		// (remove) Token: 0x0600016E RID: 366 RVA: 0x000072D0 File Offset: 0x000054D0
		public event ShareDetectedHandler ShareDetected;

		// Token: 0x0600016F RID: 367 RVA: 0x00007305 File Offset: 0x00005505
		protected virtual void OnShareDetected(ShareDetectedEventArgs e)
		{
			if (this.ShareDetected != null)
			{
				this.ShareDetected(this, e);
			}
		}

		// Token: 0x06000170 RID: 368 RVA: 0x0000731C File Offset: 0x0000551C
		public GINACharacter()
		{
			this.TextStyle = new BehaviorStyle();
			this.TimerStyle = new BehaviorStyle();
			this.TriggerGroups = new ObservableCollection<TriggerGroup>();
			this.Categories = new ObservableCollection<CharacterCategory>();
			this.VoiceName = Configuration.DefaultVoiceName;
			GINACharacter.All.Add(this);
		}

		// Token: 0x06000171 RID: 369 RVA: 0x000073F4 File Offset: 0x000055F4
		public GINACharacter(string displayName, string characterName, string logFile, string voiceName)
		{
			this.TextStyle = new BehaviorStyle();
			this.TimerStyle = new BehaviorStyle();
			this.DisplayName = displayName;
			this.Name = characterName;
			this.LogFilePath = logFile;
			this.VoiceName = (string.IsNullOrWhiteSpace(voiceName) ? Configuration.DefaultVoiceName : voiceName);
			this.TriggerGroups = new ObservableCollection<TriggerGroup>();
			this.Categories = new ObservableCollection<CharacterCategory>();
			GINACharacter.All.Add(this);
		}

		// Token: 0x06000172 RID: 370 RVA: 0x000074EC File Offset: 0x000056EC
		public void AddTriggerGroup(TriggerGroup group)
		{
			lock (this.TriggerGroups)
			{
				if (!this.TriggerGroups.Contains(group))
				{
					this.TriggerGroups.Add(group);
				}
			}
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00007540 File Offset: 0x00005740
		public void RemoveTriggerGroup(TriggerGroup group)
		{
			lock (this.TriggerGroups)
			{
				if (this.TriggerGroups.Contains(group))
				{
					this.TriggerGroups.Remove(group);
				}
			}
		}

		// Token: 0x06000174 RID: 372 RVA: 0x000075A0 File Offset: 0x000057A0
		private void MonitorTriggerGroupChanges(IEnumerable<TriggerGroup> groups)
		{
			foreach (TriggerGroup triggerGroup in groups)
			{
				triggerGroup.Triggers.CollectionChanged += delegate(object o, NotifyCollectionChangedEventArgs e)
				{
					this.QueueRegenerate();
				};
			}
			this.QueueRegenerate();
		}

		// Token: 0x06000175 RID: 373 RVA: 0x0000766C File Offset: 0x0000586C
		private void RegenerateFilters()
		{
			List<Trigger> triggers = null;
			lock (this.TriggerGroups)
			{
				triggers = this.TriggerGroups.SelectMany((TriggerGroup o) => o.Triggers).ToList<Trigger>();
			}
			List<TriggerFilter> list = this.Filters.Where((TriggerFilter o) => !triggers.Contains(o.Trigger)).ToList<TriggerFilter>();
			List<Trigger> list2 = triggers.Where((Trigger o) => !this.Filters.Select((TriggerFilter n) => n.Trigger).Contains(o)).ToList<Trigger>();
			foreach (TriggerFilter triggerFilter in list)
			{
				this.Filters.Remove(triggerFilter);
			}
			foreach (Trigger trigger in list2)
			{
				this.Filters.Add(new TriggerFilter(this, trigger));
			}
		}

		// Token: 0x06000176 RID: 374 RVA: 0x000077C0 File Offset: 0x000059C0
		public void RethrowMatch(TriggerMatchedEventArgs args)
		{
			this.OnTriggerMatched(args);
		}

		// Token: 0x06000177 RID: 375 RVA: 0x000077CC File Offset: 0x000059CC
		private void OpenFile(string filename, bool moveToEnd)
		{
			if (this.LogFileStream != null)
			{
				this.CloseFile();
			}
			if (File.Exists(filename))
			{
				this.LogFileStream = new StreamReader(File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
				if (moveToEnd)
				{
					this.LogFileStream.BaseStream.Seek(0L, SeekOrigin.End);
				}
			}
			base.RaisePropertyChanged("HasInvalidFile");
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00007828 File Offset: 0x00005A28
		private void CloseFile()
		{
			if (this.LogFileStream != null)
			{
				try
				{
					this.LogFileStream.Close();
					this.LogFileStream.Dispose();
				}
				catch
				{
				}
				finally
				{
					this.LogFileStream = null;
				}
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000179 RID: 377 RVA: 0x00007880 File Offset: 0x00005A80
		private bool ArchiveNeeded
		{
			get
			{
				if (!Configuration.Current.ArchiveLogs)
				{
					return false;
				}
				if (string.IsNullOrWhiteSpace(this.LogFilePath) || !File.Exists(this.LogFilePath))
				{
					return false;
				}
				FileInfo fileInfo = null;
				try
				{
					fileInfo = new FileInfo(this.LogFilePath);
				}
				catch
				{
				}
				return fileInfo != null && ((Configuration.Current.LogArchiveMethod == ArchiveMethods.BySize && fileInfo.Length > Configuration.Current.LogArchiveThresholdSize) || (Configuration.Current.LogArchiveMethod == ArchiveMethods.ByDate && Configuration.Current.LogArchiveSchedule.GetNextDate(this.LastArchiveDate) <= DateTime.Today));
			}
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00007930 File Offset: 0x00005B30
		public void QueueRegenerate()
		{
			Interlocked.Increment(ref this._RegenerateNeededCount);
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x0600017B RID: 379 RVA: 0x00007940 File Offset: 0x00005B40
		internal List<string> ArchivedLogFiles
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(Configuration.Current.LogArchiveFolder) && Directory.Exists(Configuration.Current.LogArchiveFolder))
				{
					return Directory.GetFiles(Configuration.Current.LogArchiveFolder, string.Format("{0}_*{1}", Path.GetFileNameWithoutExtension(this.LogFilePath), Path.GetExtension(this.LogFilePath))).ToList<string>();
				}
				return new List<string>();
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x0600017C RID: 380 RVA: 0x000079A9 File Offset: 0x00005BA9
		public bool IsEnabled
		{
			get
			{
				return this._IsEnabled;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x0600017D RID: 381 RVA: 0x000079B1 File Offset: 0x00005BB1
		public bool HasInvalidFile
		{
			get
			{
				return this.IsEnabled && (string.IsNullOrWhiteSpace(this.LogFilePath) || !File.Exists(this.LogFilePath));
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x0600017E RID: 382 RVA: 0x000079DA File Offset: 0x00005BDA
		// (set) Token: 0x0600017F RID: 383 RVA: 0x000079E7 File Offset: 0x00005BE7
		public string Name
		{
			get
			{
				return base.Get<string>("Name");
			}
			set
			{
				base.Set("Name", value);
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000180 RID: 384 RVA: 0x000079F5 File Offset: 0x00005BF5
		// (set) Token: 0x06000181 RID: 385 RVA: 0x00007A02 File Offset: 0x00005C02
		public string DisplayName
		{
			get
			{
				return base.Get<string>("DisplayName");
			}
			set
			{
				base.Set("DisplayName", value);
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000182 RID: 386 RVA: 0x00007A10 File Offset: 0x00005C10
		// (set) Token: 0x06000183 RID: 387 RVA: 0x00007A1D File Offset: 0x00005C1D
		public bool AutoMonitor
		{
			get
			{
				return base.Get<bool>("AutoMonitor");
			}
			set
			{
				base.Set("AutoMonitor", value);
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000184 RID: 388 RVA: 0x00007A30 File Offset: 0x00005C30
		// (set) Token: 0x06000185 RID: 389 RVA: 0x00007A3D File Offset: 0x00005C3D
		public string VoiceName
		{
			get
			{
				return base.Get<string>("VoiceName");
			}
			set
			{
				base.Set("VoiceName", value);
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000186 RID: 390 RVA: 0x00007A4B File Offset: 0x00005C4B
		// (set) Token: 0x06000187 RID: 391 RVA: 0x00007A58 File Offset: 0x00005C58
		public int VoiceSpeed
		{
			get
			{
				return base.Get<int>("VoiceSpeed");
			}
			set
			{
				base.Set("VoiceSpeed", value);
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000188 RID: 392 RVA: 0x00007A6B File Offset: 0x00005C6B
		// (set) Token: 0x06000189 RID: 393 RVA: 0x00007A78 File Offset: 0x00005C78
		public int Volume
		{
			get
			{
				return base.Get<int>("Volume");
			}
			set
			{
				base.Set("Volume", value);
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x0600018A RID: 394 RVA: 0x00007A8B File Offset: 0x00005C8B
		// (set) Token: 0x0600018B RID: 395 RVA: 0x00007A98 File Offset: 0x00005C98
		public DateTime LastArchiveDate
		{
			get
			{
				return base.Get<DateTime>("LastArchiveDate");
			}
			set
			{
				base.Set("LastArchiveDate", value);
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x0600018C RID: 396 RVA: 0x00007AAB File Offset: 0x00005CAB
		// (set) Token: 0x0600018D RID: 397 RVA: 0x00007AB8 File Offset: 0x00005CB8
		public string LogFilePath
		{
			get
			{
				return base.Get<string>("LogFilePath");
			}
			set
			{
				bool isEnabled = this.IsEnabled;
				if (isEnabled)
				{
					this.SetMonitoringStatus(false, true);
				}
				base.Set("LogFilePath", value);
				if (isEnabled)
				{
					this.SetMonitoringStatus(true, true);
				}
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x0600018E RID: 398 RVA: 0x00007AEE File Offset: 0x00005CEE
		// (set) Token: 0x0600018F RID: 399 RVA: 0x00007B94 File Offset: 0x00005D94
		public ObservableCollection<TriggerGroup> TriggerGroups
		{
			get
			{
				return base.Get<ObservableCollection<TriggerGroup>>("TriggerGroups");
			}
			set
			{
				base.Set("TriggerGroups", value);
				this.MonitorTriggerGroupChanges(this.TriggerGroups);
				this.TriggerGroups.CollectionChanged += delegate(object o, NotifyCollectionChangedEventArgs e)
				{
					if (e.NewItems != null && e.NewItems.Count > 0)
					{
						this.MonitorTriggerGroupChanges(e.NewItems.Cast<TriggerGroup>());
						return;
					}
					if (e.OldItems != null && e.OldItems.Count > 0)
					{
						this.QueueRegenerate();
						foreach (TriggerGroup triggerGroup in e.OldItems.Cast<TriggerGroup>())
						{
							triggerGroup.Dispose();
						}
					}
				};
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000190 RID: 400 RVA: 0x00007BE4 File Offset: 0x00005DE4
		public string PhoneticName
		{
			get
			{
				PhoneticTransform phoneticTransform = Configuration.Current.PhoneticDictionary.FirstOrDefault((PhoneticTransform o) => o.ActualWord.ToUpper() == this.Name.ToUpper());
				if (phoneticTransform == null)
				{
					return this.Name;
				}
				return phoneticTransform.PhoneticWord;
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000191 RID: 401 RVA: 0x00007C1D File Offset: 0x00005E1D
		// (set) Token: 0x06000192 RID: 402 RVA: 0x00007C2A File Offset: 0x00005E2A
		public BehaviorStyle TextStyle
		{
			get
			{
				return base.Get<BehaviorStyle>("TextStyle");
			}
			set
			{
				base.Set("TextStyle", value);
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000193 RID: 403 RVA: 0x00007C38 File Offset: 0x00005E38
		// (set) Token: 0x06000194 RID: 404 RVA: 0x00007C45 File Offset: 0x00005E45
		public BehaviorStyle TimerStyle
		{
			get
			{
				return base.Get<BehaviorStyle>("TimerStyle");
			}
			set
			{
				base.Set("TimerStyle", value);
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000195 RID: 405 RVA: 0x00007C53 File Offset: 0x00005E53
		// (set) Token: 0x06000196 RID: 406 RVA: 0x00007C60 File Offset: 0x00005E60
		public ObservableCollection<CharacterCategory> Categories
		{
			get
			{
				return base.Get<ObservableCollection<CharacterCategory>>("Categories");
			}
			set
			{
				base.Set("Categories", value);
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000197 RID: 407 RVA: 0x00007C6E File Offset: 0x00005E6E
		public int CharacterIndex
		{
			get
			{
				return GINACharacter.All.IndexOf(this);
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000198 RID: 408 RVA: 0x00007C7B File Offset: 0x00005E7B
		// (set) Token: 0x06000199 RID: 409 RVA: 0x00007C83 File Offset: 0x00005E83
		public long ProfiledLines
		{
			get
			{
				return this._ProfiledLines;
			}
			set
			{
				Interlocked.Exchange(ref this._ProfiledLines, value);
			}
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00007C94 File Offset: 0x00005E94
		public void AddTimerEarlyEndFilter(TimerEarlyEndFilter filter)
		{
			lock (this.TimerEarlyEndFilters)
			{
				this.TimerEarlyEndFilters.Add(filter);
			}
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00007CF4 File Offset: 0x00005EF4
		public void RemoveTimerEarlyEndFilter(TriggerMatchedEventArgs args)
		{
			lock (this.TimerEarlyEndFilters)
			{
				this.TimerEarlyEndFilters.RemoveAll((TimerEarlyEndFilter o) => o.TriggerMatchedEventArgs == args);
			}
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00007D5C File Offset: 0x00005F5C
		public bool GetIsEverquestCharacterFromLog(string logString)
		{
			if (this.IsEverquestCharacter == null)
			{
				this.IsEverquestCharacter = new bool?(Regex.Match(logString, Configuration.EverquestLogLineFormat).Success);
			}
			return this.IsEverquestCharacter.Value;
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00007D94 File Offset: 0x00005F94
		private void CompressFile(object filename)
		{
			int num = 0;
			short num2 = 1;
			FileStream fileStream = null;
			try
			{
				fileStream = File.Open(filename as string, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
			}
			catch
			{
				return;
			}
			try
			{
				int num3 = GINACharacter.DeviceIoControl(fileStream.SafeFileHandle, 639040, ref num2, 2, IntPtr.Zero, 0, ref num, IntPtr.Zero);
				if (num3 != 0)
				{
					Exception exceptionForHR = Marshal.GetExceptionForHR(Marshal.GetLastWin32Error());
					if (exceptionForHR != null && !string.IsNullOrWhiteSpace(exceptionForHR.Message))
					{
						Configuration.LogDebug(exceptionForHR.Message ?? "");
					}
				}
			}
			finally
			{
				if (fileStream != null)
				{
					try
					{
						fileStream.Close();
						fileStream.Dispose();
					}
					catch
					{
					}
				}
			}
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00007E5C File Offset: 0x0000605C
		public void ArchiveLog()
		{
			if ((DateTime.Now - this.LastArchiveAttempt).TotalMinutes < 5.0)
			{
				return;
			}
			if (string.IsNullOrWhiteSpace(Configuration.Current.LogArchiveFolder) || !Directory.Exists(Configuration.Current.LogArchiveFolder) || !File.Exists(this.LogFilePath))
			{
				return;
			}
			lock (this._LogMaintenanceSyncObject)
			{
				bool isEnabled = this.IsEnabled;
				long num = ((this.LogFileStream != null) ? this.LogFileStream.BaseStream.Position : 0L);
				this.SetMonitoringStatus(false, true);
				int num2 = 0;
				bool flag2 = false;
				string text = Path.Combine(Configuration.Current.LogArchiveFolder, string.Format("{0}_{1:yyyyMMdd}_{1:HHmmss}{2}", Path.GetFileNameWithoutExtension(this.LogFilePath), DateTime.Now, Path.GetExtension(this.LogFilePath)));
				while (!flag2 && num2 < 10)
				{
					try
					{
						num2++;
						File.Move(this.LogFilePath, text);
						flag2 = true;
					}
					catch
					{
						Thread.Sleep(50);
					}
				}
				if (flag2)
				{
					this.LastArchiveDate = DateTime.Today;
					base.Dispatcher.Invoke(DispatcherPriority.Normal, new ThreadStart(delegate
					{
						Configuration.SaveConfiguration(false);
					}));
					if (num > 0L)
					{
						try
						{
							FileInfo fileInfo = new FileInfo(text);
							if (fileInfo.Length > num)
							{
								StreamReader streamReader = new StreamReader(File.Open(text, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
								streamReader.BaseStream.Position = num;
								this.ProcessStream(streamReader);
								streamReader.BaseStream.Dispose();
								streamReader.Dispose();
							}
						}
						catch
						{
						}
					}
					if (Configuration.Current.CompressArchivedLogs)
					{
						Thread thread = new Thread(new ParameterizedThreadStart(this.CompressFile));
						thread.Start(text);
					}
				}
				this.SetMonitoringStatus(isEnabled, false);
			}
		}

		// Token: 0x0600019F RID: 415 RVA: 0x000080CC File Offset: 0x000062CC
		public void PurgeLogs()
		{
			if (Configuration.Current.ArchiveLogs && Configuration.Current.PurgeArchivedLogs)
			{
				lock (this._LogMaintenanceSyncObject)
				{
					List<LogSearcher.ArchiveFileInfo> archiveFileDateRanges = new LogSearcher(this, LogSearchTypes.Text, string.Empty, DateTime.MinValue, DateTime.MaxValue).ArchiveFileDateRanges;
					foreach (LogSearcher.ArchiveFileInfo archiveFileInfo in archiveFileDateRanges.Where((LogSearcher.ArchiveFileInfo o) => o.EndDate < DateTime.Today.AddDays((double)(-(double)Configuration.Current.ArchivePurgeDaysToKeep))))
					{
						try
						{
							File.Delete(archiveFileInfo.FileName);
						}
						catch
						{
						}
					}
				}
			}
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x000081B4 File Offset: 0x000063B4
		public void SetMonitoringStatus(bool isEnabled, bool moveToEnd = true)
		{
			lock (this._SetMonitoringStatusLockObject)
			{
				if (!isEnabled)
				{
					if (this.LogFileWatcher != null)
					{
						this.LogFileWatcher.EnableRaisingEvents = false;
						this.LogFileWatcher = null;
					}
					if (this.LogFileStream != null)
					{
						this.CloseFile();
					}
				}
				else if (this.LogFileWatcher == null && !string.IsNullOrWhiteSpace(this.LogFilePath) && Directory.Exists(Path.GetDirectoryName(this.LogFilePath)))
				{
					if (this.ArchiveNeeded)
					{
						this.ArchiveLog();
					}
					this.PurgeLogs();
					this.LogFileWatcher = new FileSystemWatcher(Path.GetDirectoryName(this.LogFilePath), Path.GetFileName(this.LogFilePath));
					this.LogFileWatcher.Changed += this.LogFileWatcher_Changed;
					this.LogFileWatcher.Created += this.LogFileWatcher_Changed;
					this.LogFileWatcher.Deleted += this.LogFileWatcher_Deleted;
					this.OpenFile(this.LogFilePath, moveToEnd);
					this.LogFileWatcher.EnableRaisingEvents = true;
				}
			}
			this._IsEnabled = isEnabled;
			base.RaisePropertyChanged("IsEnabled");
			base.RaisePropertyChanged("HasInvalidFile");
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00008310 File Offset: 0x00006510
		public void StopMatches()
		{
			foreach (BehaviorGroup behaviorGroup in BehaviorGroup.All)
			{
				behaviorGroup.ClearMatches(this);
			}
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x0000835C File Offset: 0x0000655C
		public void ResetMatchCounters()
		{
			foreach (TriggerFilter triggerFilter in this.Filters)
			{
				triggerFilter.ResetMatchCount();
			}
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00008410 File Offset: 0x00006610
		private void ProcessText(string str)
		{
			string text = null;
			List<TimerEarlyEndFilter> earlyEndFilters = null;
			bool enableProfiler = Configuration.Current.EnableProfiler;
			Interlocked.Increment(ref this._ProfiledLines);
			if (this.IsEverquestCharacter == true)
			{
				text = ((str.Length > Configuration.EverquestFastSearchSkip) ? str.Substring(Configuration.EverquestFastSearchSkip) : str);
			}
			else if (this.IsEverquestCharacter == null)
			{
				this.GetIsEverquestCharacterFromLog(str);
				text = ((this.IsEverquestCharacter.Value && str.Length > Configuration.EverquestFastSearchSkip) ? str.Substring(Configuration.EverquestFastSearchSkip) : str);
			}
			if (str.Contains("{GINA:"))
			{
				Match match = Regex.Match(text.Replace(this.Name, "{C}"), Configuration.EverquestGINACommandFormat, RegexOptions.IgnoreCase);
				string text2;
				if (match.Success && (text2 = match.Groups["cmd"].Value.ToLower()) != null)
				{
					if (text2 == "stop")
					{
						this.StopMatches();
						return;
					}
					if (text2 == "resetcounts")
					{
						this.ResetMatchCounters();
						return;
					}
				}
				if (Configuration.Current.AllowSharedPackages)
				{
					ShareDetectedEventArgs shareDetectedEventArgs = null;
					Match match2 = Regex.Match(str, Configuration.EverquestShareFormat);
					Guid guid;
					if (match2.Success && Guid.TryParse(match2.Groups["sessionid"].Value, out guid))
					{
						shareDetectedEventArgs = new ShareDetectedEventArgs(PackageShareType.GINAShare, new Guid(match2.Groups["sessionid"].Value), match2.Groups["name"].Value, null);
					}
					else
					{
						match2 = Regex.Match(text, Configuration.DefaultShareFormat);
						if (match2.Success && Guid.TryParse(match2.Groups["sessionid"].Value, out guid))
						{
							shareDetectedEventArgs = new ShareDetectedEventArgs(PackageShareType.GINAShare, new Guid(match2.Groups["sessionid"].Value), null, null);
						}
					}
					if (shareDetectedEventArgs != null)
					{
						this.OnShareDetected(shareDetectedEventArgs);
					}
				}
			}
			if (Configuration.Current.AllowGamTextTriggerShares && str.Contains(Configuration.GamTextTriggerFastMatch))
			{
				Trigger trigger = Trigger.CreateFromGamTextTriggerString(text);
				if (trigger != null)
				{
					try
					{
						Package package = new Package();
						TriggerGroup triggerGroup = new TriggerGroup
						{
							Name = "GamTextTriggers"
						};
						triggerGroup.AddTrigger(trigger, null);
						package.RootGroup.AddGroup(triggerGroup, null);
						Match match3 = Regex.Match(text, Configuration.GamTextTriggerShareRegex);
						string text3 = (match3.Success ? match3.Groups["name"].Value : null);
						if (text3 != null && (string.Compare(text3, Configuration.Current.ReferenceToSelf, true) == 0 || string.Compare(text3, this.Name, true) == 0))
						{
							return;
						}
						this.OnShareDetected(new ShareDetectedEventArgs(PackageShareType.GamTextTriggersShare, package, text3)
						{
							SessionId = Guid.NewGuid()
						});
						return;
					}
					catch
					{
						return;
					}
				}
			}
			lock (this.Filters)
			{
				long num;
				while ((num = Interlocked.Read(ref this._RegenerateNeededCount)) > 0L)
				{
					this.RegenerateFilters();
					Interlocked.Add(ref this._RegenerateNeededCount, -num);
				}
				foreach (TriggerFilter triggerFilter in this.Filters)
				{
					bool flag2;
					if (enableProfiler)
					{
						this.ComparisonStopwatch.Reset();
						this.ComparisonStopwatch.Start();
						flag2 = triggerFilter.IsMatch(text);
						this.ComparisonStopwatch.Stop();
						triggerFilter.Trigger.AddComparison(Convert.ToInt64(this.ComparisonStopwatch.ElapsedTicks / (Stopwatch.Frequency / 1000000m)), flag2 ? ProfilerComparisonType.Matched : ProfilerComparisonType.Unmatched);
					}
					else
					{
						flag2 = triggerFilter.IsMatch(text);
					}
					if (flag2)
					{
						this.OnTriggerMatched(new TriggerMatchedEventArgs(triggerFilter, str, MatchTypes.Standard));
						if (Configuration.Current.StopSearchingAfterFirstMatch)
						{
							break;
						}
					}
				}
				earlyEndFilters = null;
				lock (this.TimerEarlyEndFilters)
				{
					if (this.TimerEarlyEndFilters.Count > 0)
					{
						foreach (TimerEarlyEndFilter timerEarlyEndFilter in this.TimerEarlyEndFilters)
						{
							bool flag2;
							if (enableProfiler)
							{
								this.ComparisonStopwatch.Reset();
								this.ComparisonStopwatch.Start();
								flag2 = timerEarlyEndFilter.IsMatch(text);
								this.ComparisonStopwatch.Stop();
								timerEarlyEndFilter.TriggerMatchedEventArgs.TriggerFilter.Trigger.AddComparison(Convert.ToInt64(this.ComparisonStopwatch.ElapsedTicks / (Stopwatch.Frequency / 1000000m)), ProfilerComparisonType.EarlyEndFilter);
							}
							else
							{
								flag2 = timerEarlyEndFilter.IsMatch(text);
							}
							if (flag2)
							{
								if (earlyEndFilters == null)
								{
									earlyEndFilters = new List<TimerEarlyEndFilter>();
								}
								earlyEndFilters.Add(timerEarlyEndFilter);
								if (Configuration.Current.StopSearchingAfterFirstMatch)
								{
									break;
								}
							}
						}
					}
				}
				if (earlyEndFilters != null)
				{
					base.Dispatcher.Invoke(DispatcherPriority.Normal, new ThreadStart(delegate
					{
						foreach (TimerEarlyEndFilter timerEarlyEndFilter2 in earlyEndFilters)
						{
							timerEarlyEndFilter2.TriggerMatchedEventArgs.EndTimer();
						}
					}));
				}
			}
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00008A4C File Offset: 0x00006C4C
		public void SyncCategories()
		{
			lock (this.Categories)
			{
				using (IEnumerator<TriggerCategory> enumerator = TriggerCategory.All.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						TriggerCategory cat2 = enumerator.Current;
						if (!this.Categories.Any((CharacterCategory o) => o.Category == cat2))
						{
							this.Categories.Add(new CharacterCategory
							{
								Category = cat2,
								TextStyleSource = InheritanceSources.FromCategory,
								TimerStyleSource = InheritanceSources.FromCategory
							});
						}
					}
				}
				using (List<CharacterCategory>.Enumerator enumerator2 = this.Categories.ToList<CharacterCategory>().GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						CharacterCategory cat = enumerator2.Current;
						if (!TriggerCategory.All.Any((TriggerCategory o) => o == cat.Category))
						{
							this.Categories.Remove(cat);
						}
					}
				}
			}
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00008B88 File Offset: 0x00006D88
		private void ProcessStream(StreamReader sr)
		{
			lock (this._ProcessStreamLockObject)
			{
				string text;
				while (this._IsEnabled && (text = sr.ReadLine()) != null)
				{
					if (text != string.Empty)
					{
						this.ProcessText(text);
					}
				}
			}
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00008BEC File Offset: 0x00006DEC
		private void LogFileWatcher_Changed(object sender, FileSystemEventArgs e)
		{
			if (sender == null || sender != this.LogFileWatcher)
			{
				return;
			}
			if (this.LogFileStream == null)
			{
				this.OpenFile(e.FullPath, e.ChangeType != WatcherChangeTypes.Created);
			}
			this.ProcessStream(this.LogFileStream);
			try
			{
				if (Configuration.Current.ArchiveLogs && Configuration.Current.LogArchiveMethod == ArchiveMethods.BySize && this.LogFileStream.BaseStream.Position > Configuration.Current.LogArchiveThresholdSize)
				{
					this.ArchiveLog();
				}
			}
			catch
			{
			}
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00008C84 File Offset: 0x00006E84
		private void LogFileWatcher_Deleted(object sender, FileSystemEventArgs e)
		{
			if (sender == null || sender != this.LogFileWatcher)
			{
				return;
			}
			this.CloseFile();
			base.RaisePropertyChanged("HasInvalidFile");
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00008CE8 File Offset: 0x00006EE8
		public static GINACharacter LoadFromXml(XmlElement element)
		{
			GINACharacter ginacharacter = new GINACharacter
			{
				Name = element.GetElementValue("Name", null),
				DisplayName = element.GetElementValue("DisplayName", null),
				AutoMonitor = element.GetElementValue("AutoMonitor", false),
				VoiceName = element.GetElementValue("VoiceName", Configuration.DefaultVoiceName),
				VoiceSpeed = element.GetElementValue("VoiceSpeed", 0),
				Volume = element.GetElementValue("Volume", 100),
				LogFilePath = element.GetElementValue("LogFilePath", null),
				LastArchiveDate = element.GetElementValue("LastArchiveDate", DateTime.Today)
			};
			ginacharacter.TextStyle.LoadFromXml(element.SelectSingleNode("TextStyle[1]") as XmlElement);
			ginacharacter.TimerStyle.LoadFromXml(element.SelectSingleNode("TimerStyle[1]") as XmlElement);
			List<XmlElement> list = element.SelectNodes("TriggerGroups/TriggerGroup").Cast<XmlElement>().ToList<XmlElement>();
			foreach (XmlElement xmlElement in list)
			{
				int groupId = 0;
				if (int.TryParse(xmlElement.GetAttribute("GroupId"), out groupId))
				{
					TriggerGroup triggerGroup = TriggerGroup.All.SingleOrDefault((TriggerGroup o) => o.GroupId == groupId);
					if (triggerGroup != null)
					{
						ginacharacter.TriggerGroups.Add(triggerGroup);
					}
				}
			}
			ginacharacter.Categories.Clear();
			List<XmlElement> list2 = element.SelectNodes("Categories/Category").Cast<XmlElement>().ToList<XmlElement>();
			using (List<XmlElement>.Enumerator enumerator2 = list2.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					XmlElement cat = enumerator2.Current;
					TriggerCategory triggerCategory = TriggerCategory.All.SingleOrDefault((TriggerCategory o) => o.Name == cat.GetElementValue("Name", ""));
					if (triggerCategory != null)
					{
						CharacterCategory characterCategory = new CharacterCategory
						{
							Category = triggerCategory
						};
						characterCategory.TextOverlaySource = cat.GetElementValue("TextOverlaySource", InheritanceSources.FromCategory);
						characterCategory.TextOverlay = BehaviorGroup.GetGroupByName(BehaviorTypes.Text, cat.GetElementValue("TextOverlay", ""), BehaviorGroup.DefaultTextGroup);
						characterCategory.TextStyleSource = cat.GetElementValue("TextStyleSource", InheritanceSources.FromCategory);
						characterCategory.TextStyle.LoadFromXml(cat.SelectSingleNode("TextStyle[1]") as XmlElement);
						characterCategory.TimerOverlaySource = cat.GetElementValue("TimerOverlaySource", InheritanceSources.FromCategory);
						characterCategory.TimerOverlay = BehaviorGroup.GetGroupByName(BehaviorTypes.Timer, cat.GetElementValue("TimerOverlay", ""), BehaviorGroup.DefaultTimerGroup);
						characterCategory.TimerStyleSource = cat.GetElementValue("TimerStyleSource", InheritanceSources.FromCategory);
						characterCategory.TimerStyle.LoadFromXml(cat.SelectSingleNode("TimerStyle[1]") as XmlElement);
						ginacharacter.Categories.Add(characterCategory);
					}
				}
			}
			ginacharacter.SyncCategories();
			if (ginacharacter.AutoMonitor)
			{
				ginacharacter.SetMonitoringStatus(true, true);
			}
			return ginacharacter;
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00009058 File Offset: 0x00007258
		public void SaveToXml(XmlElement element)
		{
			XmlDocument ownerDocument = element.OwnerDocument;
			XmlElement xmlElement = ownerDocument.CreateElement("Character");
			element.AppendChild(xmlElement);
			xmlElement.AppendChild(ownerDocument.NewElement("Name", this.Name ?? ""));
			xmlElement.AppendChild(ownerDocument.NewElement("DisplayName", this.DisplayName ?? ""));
			xmlElement.AppendChild(ownerDocument.NewElement("AutoMonitor", this.AutoMonitor));
			xmlElement.AppendChild(ownerDocument.NewElement("VoiceName", this.VoiceName ?? ""));
			xmlElement.AppendChild(ownerDocument.NewElement("VoiceSpeed", this.VoiceSpeed));
			xmlElement.AppendChild(ownerDocument.NewElement("Volume", this.Volume));
			xmlElement.AppendChild(ownerDocument.NewElement("LogFilePath", this.LogFilePath ?? ""));
			xmlElement.AppendChild(ownerDocument.NewElement("LastArchiveDate", this.LastArchiveDate));
			this.TextStyle.SaveToXml(xmlElement.AppendChild(ownerDocument.CreateElement("TextStyle")) as XmlElement);
			this.TimerStyle.SaveToXml(xmlElement.AppendChild(ownerDocument.CreateElement("TimerStyle")) as XmlElement);
			if (this.TriggerGroups.Any<TriggerGroup>())
			{
				XmlElement xmlElement2 = ownerDocument.CreateElement("TriggerGroups");
				xmlElement.AppendChild(xmlElement2);
				foreach (TriggerGroup triggerGroup in this.TriggerGroups)
				{
					XmlElement xmlElement3 = ownerDocument.CreateElement("TriggerGroup");
					xmlElement3.SetAttribute("GroupId", triggerGroup.GroupId.ToString());
					xmlElement2.AppendChild(xmlElement3);
				}
			}
			if (this.Categories.Any<CharacterCategory>())
			{
				XmlElement xmlElement4 = ownerDocument.CreateElement("Categories");
				xmlElement.AppendChild(xmlElement4);
				foreach (CharacterCategory characterCategory in this.Categories)
				{
					XmlElement xmlElement5 = ownerDocument.CreateElement("Category");
					xmlElement4.AppendChild(xmlElement5);
					xmlElement5.AppendChild(ownerDocument.NewElement("Name", characterCategory.Category.Name));
					xmlElement5.AppendChild(ownerDocument.NewElement("TextOverlaySource", characterCategory.TextOverlaySource));
					xmlElement5.AppendChild(ownerDocument.NewElement("TextOverlay", characterCategory.TextOverlay.Name));
					xmlElement5.AppendChild(ownerDocument.NewElement("TextStyleSource", characterCategory.TextStyleSource));
					characterCategory.TextStyle.SaveToXml(xmlElement5.AppendChild(ownerDocument.CreateElement("TextStyle")) as XmlElement);
					xmlElement5.AppendChild(ownerDocument.NewElement("TimerOverlaySource", characterCategory.TimerOverlaySource));
					xmlElement5.AppendChild(ownerDocument.NewElement("TimerOverlay", characterCategory.TimerOverlay.Name));
					xmlElement5.AppendChild(ownerDocument.NewElement("TimerStyleSource", characterCategory.TimerStyleSource));
					characterCategory.TimerStyle.SaveToXml(xmlElement5.AppendChild(ownerDocument.CreateElement("TimerStyle")) as XmlElement);
				}
			}
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00009404 File Offset: 0x00007604
		public override void Dispose(bool disposing)
		{
			if (!this._Disposed)
			{
				this._Disposed = true;
				if (disposing)
				{
					this.SetMonitoringStatus(false, true);
				}
			}
		}

		// Token: 0x0400009C RID: 156
		private const int FSCTL_SET_COMPRESSION = 639040;

		// Token: 0x0400009D RID: 157
		private const short COMPRESSION_FORMAT_DEFAULT = 1;

		// Token: 0x0400009E RID: 158
		private static ObservableCollection<GINACharacter> _All;

		// Token: 0x0400009F RID: 159
		private static DispatcherTimer ScheduledArchiveTimer;

		// Token: 0x040000A2 RID: 162
		private FileSystemWatcher LogFileWatcher;

		// Token: 0x040000A3 RID: 163
		private StreamReader LogFileStream;

		// Token: 0x040000A4 RID: 164
		internal List<TriggerFilter> Filters = new List<TriggerFilter>();

		// Token: 0x040000A5 RID: 165
		private List<TimerEarlyEndFilter> TimerEarlyEndFilters = new List<TimerEarlyEndFilter>();

		// Token: 0x040000A6 RID: 166
		private bool? IsEverquestCharacter = null;

		// Token: 0x040000A7 RID: 167
		private DateTime LastArchiveAttempt = DateTime.Today.AddDays(-2.0);

		// Token: 0x040000A8 RID: 168
		private Stopwatch ComparisonStopwatch = new Stopwatch();

		// Token: 0x040000A9 RID: 169
		private long _ProfiledLines;

		// Token: 0x040000AA RID: 170
		private long _RegenerateNeededCount;

		// Token: 0x040000AB RID: 171
		private bool _IsEnabled;

		// Token: 0x040000AC RID: 172
		private object _LogMaintenanceSyncObject = new object();

		// Token: 0x040000AD RID: 173
		private object _SetMonitoringStatusLockObject = new object();

		// Token: 0x040000AE RID: 174
		private object _ProcessStreamLockObject = new object();

		// Token: 0x040000AF RID: 175
		private string[] SplitTokens = new string[] { "\r\n" };
	}
}
