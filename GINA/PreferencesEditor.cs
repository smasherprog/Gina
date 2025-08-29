using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using GimaSoft.Business.GINA;
using WPFShared;

namespace GimaSoft.GINA
{
	// Token: 0x02000009 RID: 9
	public class PreferencesEditor : BindableControl
	{
		// Token: 0x06000134 RID: 308 RVA: 0x00005442 File Offset: 0x00003642
		static PreferencesEditor()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(PreferencesEditor), new FrameworkPropertyMetadata(typeof(PreferencesEditor)));
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00005467 File Offset: 0x00003667
		public PreferencesEditor()
		{
			this.ShareWhiteList = new ObservableCollection<string>();
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000136 RID: 310 RVA: 0x0000547A File Offset: 0x0000367A
		public Configuration Settings
		{
			get
			{
				return Configuration.Current;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000137 RID: 311 RVA: 0x00005481 File Offset: 0x00003681
		public App App
		{
			get
			{
				return (App)global::System.Windows.Application.Current;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000138 RID: 312 RVA: 0x0000548D File Offset: 0x0000368D
		// (set) Token: 0x06000139 RID: 313 RVA: 0x0000549A File Offset: 0x0000369A
		public bool EnableSound
		{
			get
			{
				return base.Get<bool>("EnableSound");
			}
			set
			{
				base.Set("EnableSound", value);
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x0600013A RID: 314 RVA: 0x000054AD File Offset: 0x000036AD
		// (set) Token: 0x0600013B RID: 315 RVA: 0x000054BA File Offset: 0x000036BA
		public int MasterVolume
		{
			get
			{
				return base.Get<int>("MasterVolume");
			}
			set
			{
				base.Set("MasterVolume", value);
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x0600013C RID: 316 RVA: 0x000054CD File Offset: 0x000036CD
		// (set) Token: 0x0600013D RID: 317 RVA: 0x000054DA File Offset: 0x000036DA
		public bool EnableText
		{
			get
			{
				return base.Get<bool>("EnableText");
			}
			set
			{
				base.Set("EnableText", value);
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x0600013E RID: 318 RVA: 0x000054ED File Offset: 0x000036ED
		// (set) Token: 0x0600013F RID: 319 RVA: 0x000054FA File Offset: 0x000036FA
		public bool EnableTimers
		{
			get
			{
				return base.Get<bool>("EnableTimers");
			}
			set
			{
				base.Set("EnableTimers", value);
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000140 RID: 320 RVA: 0x0000550D File Offset: 0x0000370D
		// (set) Token: 0x06000141 RID: 321 RVA: 0x0000551A File Offset: 0x0000371A
		public bool DisplayMatches
		{
			get
			{
				return base.Get<bool>("DisplayMatches");
			}
			set
			{
				base.Set("DisplayMatches", value);
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000142 RID: 322 RVA: 0x0000552D File Offset: 0x0000372D
		// (set) Token: 0x06000143 RID: 323 RVA: 0x0000553A File Offset: 0x0000373A
		public int MatchDisplayLimit
		{
			get
			{
				return base.Get<int>("MatchDisplayLimit");
			}
			set
			{
				base.Set("MatchDisplayLimit", value);
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000144 RID: 324 RVA: 0x0000554D File Offset: 0x0000374D
		// (set) Token: 0x06000145 RID: 325 RVA: 0x0000555A File Offset: 0x0000375A
		public bool LogMatchesToFile
		{
			get
			{
				return base.Get<bool>("LogMatchesToFile");
			}
			set
			{
				base.Set("LogMatchesToFile", value);
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000146 RID: 326 RVA: 0x0000556D File Offset: 0x0000376D
		// (set) Token: 0x06000147 RID: 327 RVA: 0x0000557A File Offset: 0x0000377A
		public string MatchLogFileName
		{
			get
			{
				return base.Get<string>("MatchLogFileName");
			}
			set
			{
				base.Set("MatchLogFileName", value);
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000148 RID: 328 RVA: 0x00005588 File Offset: 0x00003788
		// (set) Token: 0x06000149 RID: 329 RVA: 0x00005595 File Offset: 0x00003795
		public string ShareFormat
		{
			get
			{
				return base.Get<string>("ShareFormat");
			}
			set
			{
				base.Set("ShareFormat", value);
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600014A RID: 330 RVA: 0x000055A3 File Offset: 0x000037A3
		// (set) Token: 0x0600014B RID: 331 RVA: 0x000055B0 File Offset: 0x000037B0
		public string ShareServiceUri
		{
			get
			{
				return base.Get<string>("ShareServiceUri");
			}
			set
			{
				base.Set("ShareServiceUri", value);
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600014C RID: 332 RVA: 0x000055BE File Offset: 0x000037BE
		// (set) Token: 0x0600014D RID: 333 RVA: 0x000055CB File Offset: 0x000037CB
		public string LogLineFormat
		{
			get
			{
				return base.Get<string>("LogLineFormat");
			}
			set
			{
				base.Set("LogLineFormat", value);
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600014E RID: 334 RVA: 0x000055D9 File Offset: 0x000037D9
		// (set) Token: 0x0600014F RID: 335 RVA: 0x000055E6 File Offset: 0x000037E6
		public string DataFolder
		{
			get
			{
				return base.Get<string>("DataFolder");
			}
			set
			{
				base.Set("DataFolder", value);
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000150 RID: 336 RVA: 0x000055F4 File Offset: 0x000037F4
		// (set) Token: 0x06000151 RID: 337 RVA: 0x00005601 File Offset: 0x00003801
		public string ImportedMediaFileFolder
		{
			get
			{
				return base.Get<string>("ImportedMediaFileFolder");
			}
			set
			{
				base.Set("ImportedMediaFileFolder", value);
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000152 RID: 338 RVA: 0x0000560F File Offset: 0x0000380F
		// (set) Token: 0x06000153 RID: 339 RVA: 0x0000561C File Offset: 0x0000381C
		public string EverquestFolder
		{
			get
			{
				return base.Get<string>("EverquestFolder");
			}
			set
			{
				base.Set("EverquestFolder", value);
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000154 RID: 340 RVA: 0x0000562A File Offset: 0x0000382A
		// (set) Token: 0x06000155 RID: 341 RVA: 0x00005637 File Offset: 0x00003837
		public bool AutoUpdate
		{
			get
			{
				return base.Get<bool>("AutoUpdate");
			}
			set
			{
				base.Set("AutoUpdate", value);
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000156 RID: 342 RVA: 0x0000564A File Offset: 0x0000384A
		// (set) Token: 0x06000157 RID: 343 RVA: 0x00005657 File Offset: 0x00003857
		public bool MinimizeToSystemTray
		{
			get
			{
				return base.Get<bool>("MinimizeToSystemTray");
			}
			set
			{
				base.Set("MinimizeToSystemTray", value);
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000158 RID: 344 RVA: 0x0000566A File Offset: 0x0000386A
		// (set) Token: 0x06000159 RID: 345 RVA: 0x00005677 File Offset: 0x00003877
		public string ReferenceToSelf
		{
			get
			{
				return base.Get<string>("ReferenceToSelf");
			}
			set
			{
				base.Set("ReferenceToSelf", value);
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x0600015A RID: 346 RVA: 0x00005685 File Offset: 0x00003885
		// (set) Token: 0x0600015B RID: 347 RVA: 0x00005692 File Offset: 0x00003892
		public bool AllowSharedPackages
		{
			get
			{
				return base.Get<bool>("AllowSharedPackages");
			}
			set
			{
				base.Set("AllowSharedPackages", value);
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x0600015C RID: 348 RVA: 0x000056A5 File Offset: 0x000038A5
		// (set) Token: 0x0600015D RID: 349 RVA: 0x000056B2 File Offset: 0x000038B2
		public bool AllowGamTextTriggerShares
		{
			get
			{
				return base.Get<bool>("AllowGamTextTriggerShares");
			}
			set
			{
				base.Set("AllowGamTextTriggerShares", value);
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x0600015E RID: 350 RVA: 0x000056C5 File Offset: 0x000038C5
		// (set) Token: 0x0600015F RID: 351 RVA: 0x000056D2 File Offset: 0x000038D2
		public bool StopSearchingAfterFirstMatch
		{
			get
			{
				return base.Get<bool>("StopSearchingAfterFirstMatch");
			}
			set
			{
				base.Set("StopSearchingAfterFirstMatch", value);
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000160 RID: 352 RVA: 0x000056E5 File Offset: 0x000038E5
		// (set) Token: 0x06000161 RID: 353 RVA: 0x000056F2 File Offset: 0x000038F2
		public ShareLevel AcceptShareLevel
		{
			get
			{
				return base.Get<ShareLevel>("AcceptShareLevel");
			}
			set
			{
				base.Set("AcceptShareLevel", value);
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000162 RID: 354 RVA: 0x00005705 File Offset: 0x00003905
		// (set) Token: 0x06000163 RID: 355 RVA: 0x00005712 File Offset: 0x00003912
		public ShareLevel AutoMergeShareLevel
		{
			get
			{
				return base.Get<ShareLevel>("AutoMergeShareLevel");
			}
			set
			{
				base.Set("AutoMergeShareLevel", value);
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000164 RID: 356 RVA: 0x00005725 File Offset: 0x00003925
		// (set) Token: 0x06000165 RID: 357 RVA: 0x00005732 File Offset: 0x00003932
		public string CTagClipboardReplacement
		{
			get
			{
				return base.Get<string>("CTagClipboardReplacement");
			}
			set
			{
				base.Set("CTagClipboardReplacement", value);
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000166 RID: 358 RVA: 0x00005740 File Offset: 0x00003940
		// (set) Token: 0x06000167 RID: 359 RVA: 0x0000574D File Offset: 0x0000394D
		public bool EnableDebugLog
		{
			get
			{
				return base.Get<bool>("EnableDebugLog");
			}
			set
			{
				base.Set("EnableDebugLog", value);
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000168 RID: 360 RVA: 0x00005760 File Offset: 0x00003960
		// (set) Token: 0x06000169 RID: 361 RVA: 0x0000576D File Offset: 0x0000396D
		public string NewTrustedName
		{
			get
			{
				return base.Get<string>("NewTrustedName");
			}
			set
			{
				base.Set("NewTrustedName", value);
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x0600016A RID: 362 RVA: 0x0000577B File Offset: 0x0000397B
		// (set) Token: 0x0600016B RID: 363 RVA: 0x00005788 File Offset: 0x00003988
		public string SelectedTrustedName
		{
			get
			{
				return base.Get<string>("SelectedTrustedName");
			}
			set
			{
				base.Set("SelectedTrustedName", value);
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600016C RID: 364 RVA: 0x00005796 File Offset: 0x00003996
		// (set) Token: 0x0600016D RID: 365 RVA: 0x000057A3 File Offset: 0x000039A3
		public ObservableCollection<string> ShareWhiteList
		{
			get
			{
				return base.Get<ObservableCollection<string>>("ShareWhiteList");
			}
			set
			{
				base.Set("ShareWhiteList", value);
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600016E RID: 366 RVA: 0x000057B1 File Offset: 0x000039B1
		// (set) Token: 0x0600016F RID: 367 RVA: 0x000057BE File Offset: 0x000039BE
		public bool ArchiveLogs
		{
			get
			{
				return base.Get<bool>("ArchiveLogs");
			}
			set
			{
				base.Set("ArchiveLogs", value);
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000170 RID: 368 RVA: 0x000057D1 File Offset: 0x000039D1
		// (set) Token: 0x06000171 RID: 369 RVA: 0x000057DE File Offset: 0x000039DE
		public bool CompressArchivedLogs
		{
			get
			{
				return base.Get<bool>("CompressArchivedLogs");
			}
			set
			{
				base.Set("CompressArchivedLogs", value);
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000172 RID: 370 RVA: 0x000057F1 File Offset: 0x000039F1
		// (set) Token: 0x06000173 RID: 371 RVA: 0x000057FE File Offset: 0x000039FE
		public string LogArchiveFolder
		{
			get
			{
				return base.Get<string>("LogArchiveFolder");
			}
			set
			{
				base.Set("LogArchiveFolder", value);
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000174 RID: 372 RVA: 0x0000580C File Offset: 0x00003A0C
		// (set) Token: 0x06000175 RID: 373 RVA: 0x00005819 File Offset: 0x00003A19
		public ArchiveMethods LogArchiveMethod
		{
			get
			{
				return base.Get<ArchiveMethods>("LogArchiveMethod");
			}
			set
			{
				base.Set("LogArchiveMethod", value);
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000176 RID: 374 RVA: 0x0000582C File Offset: 0x00003A2C
		// (set) Token: 0x06000177 RID: 375 RVA: 0x00005839 File Offset: 0x00003A39
		public ArchiveSchedules LogArchiveSchedule
		{
			get
			{
				return base.Get<ArchiveSchedules>("LogArchiveSchedule");
			}
			set
			{
				base.Set("LogArchiveSchedule", value);
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000178 RID: 376 RVA: 0x0000584C File Offset: 0x00003A4C
		// (set) Token: 0x06000179 RID: 377 RVA: 0x00005859 File Offset: 0x00003A59
		public long LogArchiveThresholdSize
		{
			get
			{
				return base.Get<long>("LogArchiveThresholdSize");
			}
			set
			{
				base.Set("LogArchiveThresholdSize", value);
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x0600017A RID: 378 RVA: 0x0000586C File Offset: 0x00003A6C
		// (set) Token: 0x0600017B RID: 379 RVA: 0x00005879 File Offset: 0x00003A79
		public bool PurgeArchivedLogs
		{
			get
			{
				return base.Get<bool>("PurgeArchivedLogs");
			}
			set
			{
				base.Set("PurgeArchivedLogs", value);
				if (value && !this.Settings.PurgeArchivedLogs)
				{
					this.App.Data.ShowError("Warning", "Enabling this feature will delete log files from your log archive folder permanently.");
				}
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x0600017C RID: 380 RVA: 0x000058B6 File Offset: 0x00003AB6
		// (set) Token: 0x0600017D RID: 381 RVA: 0x000058C3 File Offset: 0x00003AC3
		public int ArchivePurgeDaysToKeep
		{
			get
			{
				return base.Get<int>("ArchivePurgeDaysToKeep");
			}
			set
			{
				base.Set("ArchivePurgeDaysToKeep", value);
			}
		}

		// Token: 0x0600017E RID: 382 RVA: 0x000058DC File Offset: 0x00003ADC
		private void SetConfigurationFields()
		{
			this.Settings.EnableSound = this.EnableSound;
			this.Settings.MasterVolume = this.MasterVolume;
			this.Settings.EnableText = this.EnableText;
			this.Settings.EnableTimers = this.EnableTimers;
			this.Settings.DisplayMatches = this.DisplayMatches;
			this.Settings.MatchDisplayLimit = this.MatchDisplayLimit;
			this.Settings.LogMatchesToFile = this.LogMatchesToFile;
			this.Settings.MatchLogFileName = this.MatchLogFileName;
			this.Settings.ShareServiceUri = this.ShareServiceUri;
			this.Settings.ImportedMediaFileFolder = this.ImportedMediaFileFolder;
			this.Settings.EverquestFolder = this.EverquestFolder;
			this.Settings.AutoUpdate = this.AutoUpdate;
			this.Settings.MinimizeToSystemTray = this.MinimizeToSystemTray;
			this.Settings.ReferenceToSelf = this.ReferenceToSelf;
			this.Settings.AllowSharedPackages = this.AllowSharedPackages;
			this.Settings.AllowGamTextTriggerShares = this.AllowGamTextTriggerShares;
			this.Settings.StopSearchingAfterFirstMatch = this.StopSearchingAfterFirstMatch;
			this.Settings.AcceptShareLevel = this.AcceptShareLevel;
			this.Settings.AutoMergeShareLevel = this.AutoMergeShareLevel;
			this.Settings.CTagClipboardReplacement = this.CTagClipboardReplacement;
			this.Settings.EnableDebugLog = this.EnableDebugLog;
			this.Settings.ShareWhiteList.Clear();
			this.Settings.ShareWhiteList.AddRange(this.ShareWhiteList.OrderBy((string o) => o));
			this.Settings.ArchiveLogs = this.ArchiveLogs;
			this.Settings.CompressArchivedLogs = this.CompressArchivedLogs;
			this.Settings.LogArchiveFolder = this.LogArchiveFolder;
			this.Settings.LogArchiveMethod = this.LogArchiveMethod;
			this.Settings.LogArchiveSchedule = this.LogArchiveSchedule;
			this.Settings.LogArchiveThresholdSize = this.LogArchiveThresholdSize;
			this.Settings.PurgeArchivedLogs = this.PurgeArchivedLogs;
			this.Settings.ArchivePurgeDaysToKeep = this.ArchivePurgeDaysToKeep;
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00005B24 File Offset: 0x00003D24
		private void SetFields()
		{
			this.EnableSound = this.Settings.EnableSound;
			this.MasterVolume = this.Settings.MasterVolume;
			this.EnableText = this.Settings.EnableText;
			this.EnableTimers = this.Settings.EnableTimers;
			this.DisplayMatches = this.Settings.DisplayMatches;
			this.MatchDisplayLimit = this.Settings.MatchDisplayLimit;
			this.LogMatchesToFile = this.Settings.LogMatchesToFile;
			this.MatchLogFileName = this.Settings.MatchLogFileName;
			this.DataFolder = Configuration.DataFolder;
			this.ShareServiceUri = this.Settings.ShareServiceUri;
			this.ImportedMediaFileFolder = this.Settings.ImportedMediaFileFolder;
			this.EverquestFolder = this.Settings.EverquestFolder;
			this.AutoUpdate = this.Settings.AutoUpdate;
			this.MinimizeToSystemTray = this.Settings.MinimizeToSystemTray;
			this.ReferenceToSelf = this.Settings.ReferenceToSelf;
			this.AllowSharedPackages = this.Settings.AllowSharedPackages;
			this.AllowGamTextTriggerShares = this.Settings.AllowGamTextTriggerShares;
			this.StopSearchingAfterFirstMatch = this.Settings.StopSearchingAfterFirstMatch;
			this.AcceptShareLevel = this.Settings.AcceptShareLevel;
			this.AutoMergeShareLevel = this.Settings.AutoMergeShareLevel;
			this.CTagClipboardReplacement = this.Settings.CTagClipboardReplacement;
			this.EnableDebugLog = this.Settings.EnableDebugLog;
			this.ShareWhiteList.Clear();
			foreach (string text in this.Settings.ShareWhiteList.OrderBy((string o) => o))
			{
				this.ShareWhiteList.Add(text);
			}
			this.ArchiveLogs = this.Settings.ArchiveLogs;
			this.CompressArchivedLogs = this.Settings.CompressArchivedLogs;
			this.LogArchiveFolder = this.Settings.LogArchiveFolder;
			this.LogArchiveMethod = this.Settings.LogArchiveMethod;
			this.LogArchiveSchedule = this.Settings.LogArchiveSchedule;
			this.LogArchiveThresholdSize = this.Settings.LogArchiveThresholdSize;
			this.PurgeArchivedLogs = this.Settings.PurgeArchivedLogs;
			this.ArchivePurgeDaysToKeep = this.Settings.ArchivePurgeDaysToKeep;
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00005DA0 File Offset: 0x00003FA0
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000181 RID: 385 RVA: 0x00005DC0 File Offset: 0x00003FC0
		public GenericCommand SetEverquestDefaultsCommand
		{
			get
			{
				return new GenericCommand
				{
					Execute = delegate(object p)
					{
						this.LogLineFormat = Configuration.EverquestLogLineFormat;
						this.ShareFormat = Configuration.EverquestShareFormat;
					}
				};
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000182 RID: 386 RVA: 0x00005E44 File Offset: 0x00004044
		public GenericCommand FindArchiveFolderCommand
		{
			get
			{
				return new GenericCommand
				{
					Execute = delegate(object p)
					{
						FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
						folderBrowserDialog.ShowNewFolderButton = true;
						folderBrowserDialog.Description = "Please select the folder in which you wish to archive log files.";
						try
						{
							folderBrowserDialog.SelectedPath = this.LogArchiveFolder;
						}
						catch
						{
						}
						if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
						{
							this.LogArchiveFolder = folderBrowserDialog.SelectedPath;
						}
					}
				};
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000183 RID: 387 RVA: 0x00005EC8 File Offset: 0x000040C8
		public GenericCommand FindDataFolderCommand
		{
			get
			{
				return new GenericCommand
				{
					Execute = delegate(object p)
					{
						FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
						folderBrowserDialog.ShowNewFolderButton = true;
						folderBrowserDialog.Description = "Please select the folder in which you wish to store GINA data files.";
						try
						{
							folderBrowserDialog.SelectedPath = this.DataFolder;
						}
						catch
						{
						}
						if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
						{
							this.DataFolder = folderBrowserDialog.SelectedPath;
						}
					}
				};
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000184 RID: 388 RVA: 0x00005F4C File Offset: 0x0000414C
		public GenericCommand FindMediaFolderCommand
		{
			get
			{
				return new GenericCommand
				{
					Execute = delegate(object p)
					{
						FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
						folderBrowserDialog.ShowNewFolderButton = true;
						folderBrowserDialog.Description = "Please select the folder in which you wish to store downloaded trigger media files.";
						try
						{
							folderBrowserDialog.SelectedPath = this.ImportedMediaFileFolder;
						}
						catch
						{
						}
						if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
						{
							this.ImportedMediaFileFolder = folderBrowserDialog.SelectedPath;
						}
					}
				};
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000185 RID: 389 RVA: 0x00005FD0 File Offset: 0x000041D0
		public GenericCommand FindEverquestFolderCommand
		{
			get
			{
				return new GenericCommand
				{
					Execute = delegate(object p)
					{
						FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
						folderBrowserDialog.Description = "Please select the folder in which Everquest is installed.";
						folderBrowserDialog.ShowNewFolderButton = false;
						try
						{
							folderBrowserDialog.SelectedPath = this.EverquestFolder;
						}
						catch
						{
						}
						if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
						{
							this.EverquestFolder = folderBrowserDialog.SelectedPath;
						}
					}
				};
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000186 RID: 390 RVA: 0x000060E4 File Offset: 0x000042E4
		public GenericCommand FindMatchLogCommand
		{
			get
			{
				return new GenericCommand
				{
					Execute = delegate(object p)
					{
						OpenFileDialog openFileDialog = new OpenFileDialog();
						openFileDialog.AddExtension = true;
						openFileDialog.DefaultExt = "txt";
						openFileDialog.CheckFileExists = false;
						openFileDialog.CheckPathExists = true;
						openFileDialog.ShowHelp = true;
						openFileDialog.HelpRequest += delegate(object o, EventArgs e)
						{
							global::System.Windows.Forms.MessageBox.Show("Please select the log file of the character that you wish to monitor.");
						};
						try
						{
							openFileDialog.FileName = Path.GetFileName(this.MatchLogFileName);
						}
						catch
						{
						}
						try
						{
							openFileDialog.InitialDirectory = (string.IsNullOrWhiteSpace(this.MatchLogFileName) ? Configuration.DataFolder : Path.GetDirectoryName(this.MatchLogFileName));
						}
						catch
						{
						}
						openFileDialog.Multiselect = false;
						openFileDialog.Filter = "Text Files|*.txt|All Files|*.*";
						if (openFileDialog.ShowDialog() == DialogResult.OK)
						{
							this.MatchLogFileName = openFileDialog.FileName;
						}
					}
				};
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000187 RID: 391 RVA: 0x00006160 File Offset: 0x00004360
		public GenericCommand SaveCommand
		{
			get
			{
				return new GenericCommand
				{
					Execute = delegate(object p)
					{
						if (Configuration.DataFolder != this.DataFolder)
						{
							Configuration.DataFolder = this.DataFolder;
							this.App.Data.LoggedMatches.Clear();
						}
						else
						{
							this.SetConfigurationFields();
							Configuration.SaveConfiguration(false);
						}
						base.Visibility = Visibility.Collapsed;
					}
				};
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000188 RID: 392 RVA: 0x00006190 File Offset: 0x00004390
		public GenericCommand CancelCommand
		{
			get
			{
				return new GenericCommand
				{
					Execute = delegate(object p)
					{
						base.Visibility = Visibility.Collapsed;
					}
				};
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000189 RID: 393 RVA: 0x000061C8 File Offset: 0x000043C8
		public GenericCommand VisibilityChangedCommand
		{
			get
			{
				return new GenericCommand
				{
					Execute = delegate(object p)
					{
						if ((Visibility)p == Visibility.Visible)
						{
							this.SetFields();
						}
					}
				};
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x0600018A RID: 394 RVA: 0x0000626C File Offset: 0x0000446C
		public GenericCommand AddTrustedNameCommand
		{
			get
			{
				return new GenericCommand
				{
					Execute = delegate(object p)
					{
						if (string.IsNullOrWhiteSpace(this.NewTrustedName))
						{
							return;
						}
						if (!this.ShareWhiteList.Select((string o) => o.ToLower()).Contains(this.NewTrustedName.ToLower()))
						{
							this.ShareWhiteList.Add(this.NewTrustedName);
						}
						this.NewTrustedName = string.Empty;
					}
				};
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x0600018B RID: 395 RVA: 0x000062B4 File Offset: 0x000044B4
		public GenericCommand RemoveTrustedNameCommand
		{
			get
			{
				return new GenericCommand
				{
					Execute = delegate(object p)
					{
						if (string.IsNullOrWhiteSpace(this.SelectedTrustedName))
						{
							return;
						}
						this.ShareWhiteList.Remove(this.SelectedTrustedName);
					}
				};
			}
		}
	}
}
