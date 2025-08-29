using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using GimaSoft.Business.GINA;
using WPFShared;

namespace GimaSoft.GINA
{
	// Token: 0x02000016 RID: 22
	public class GINAData : BindableControl
	{
		// Token: 0x1700011D RID: 285
		// (get) Token: 0x060002C8 RID: 712 RVA: 0x000093D0 File Offset: 0x000075D0
		public static GINAData Current
		{
			get
			{
				lock (new object())
				{
					if (GINAData._Current == null)
					{
						GINAData._Current = new GINAData();
					}
				}
				return GINAData._Current;
			}
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x00009B6C File Offset: 0x00007D6C
		public GINAData()
		{
			this.LoggedMatches = new ObservableCollection<TriggerMatchedEventArgs>();
			this.Characters = GINACharacter.All.Select((GINACharacter o) => new CharacterViewModel(o)).ToObservableCollection<CharacterViewModel>();
			GINACharacter.All.CollectionChanged += delegate(object o, NotifyCollectionChangedEventArgs e)
			{
				if (e.Action == NotifyCollectionChangedAction.Add)
				{
					int newStartingIndex = e.NewStartingIndex;
					using (IEnumerator<GINACharacter> enumerator3 = e.NewItems.Cast<GINACharacter>().GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							GINACharacter ginacharacter = enumerator3.Current;
							this.Characters.Insert(newStartingIndex++, new CharacterViewModel(ginacharacter));
						}
						return;
					}
				}
				if (e.Action == NotifyCollectionChangedAction.Reset)
				{
					this.Characters.Clear();
					return;
				}
				if (e.Action == NotifyCollectionChangedAction.Move)
				{
					this.Characters.Move(e.OldStartingIndex, e.NewStartingIndex);
					return;
				}
				if (e.Action == NotifyCollectionChangedAction.Remove)
				{
					using (IEnumerator<GINACharacter> enumerator4 = e.OldItems.Cast<GINACharacter>().GetEnumerator())
					{
						while (enumerator4.MoveNext())
						{
							GINACharacter item = enumerator4.Current;
							this.Characters.Remove(this.Characters.Single((CharacterViewModel n) => n.Character == item));
						}
					}
				}
			};
			this.TextBehaviorWindows = (from o in BehaviorGroup.All
				where o.BehaviorType == BehaviorTypes.Text
				orderby o.Name
				select o).ToDictionary((BehaviorGroup o) => o, (BehaviorGroup n) => TextWindow.NewWindow(n));
			if (Configuration.Current.EnableText)
			{
				foreach (KeyValuePair<BehaviorGroup, TextWindow> keyValuePair in this.TextBehaviorWindows)
				{
					keyValuePair.Value.Show();
				}
			}
			BehaviorGroup.All.CollectionChanged += delegate(object o, NotifyCollectionChangedEventArgs e)
			{
				if (e.Action == NotifyCollectionChangedAction.Add)
				{
					using (IEnumerator<BehaviorGroup> enumerator5 = (from BehaviorGroup n in e.NewItems
						where n.BehaviorType == BehaviorTypes.Text
						select n).GetEnumerator())
					{
						while (enumerator5.MoveNext())
						{
							BehaviorGroup behaviorGroup = enumerator5.Current;
							TextWindow textWindow = TextWindow.NewWindow(behaviorGroup);
							this.TextBehaviorWindows.Add(behaviorGroup, textWindow);
						}
						goto IL_01C4;
					}
				}
				if (e.Action == NotifyCollectionChangedAction.Remove || e.Action == NotifyCollectionChangedAction.Reset)
				{
					List<BehaviorGroup> list;
					if (e.Action != NotifyCollectionChangedAction.Remove)
					{
						list = (from n in this.TextBehaviorWindows
							where n.Key.BehaviorType == BehaviorTypes.Text
							select n.Key).ToList<BehaviorGroup>();
					}
					else
					{
						list = this.TextBehaviorWindows.Join(e.OldItems.Cast<BehaviorGroup>(), (KeyValuePair<BehaviorGroup, TextWindow> m) => m.Key, (BehaviorGroup n) => n, (KeyValuePair<BehaviorGroup, TextWindow> n, BehaviorGroup m) => n.Key).ToList<BehaviorGroup>();
					}
					List<BehaviorGroup> list2 = list;
					foreach (BehaviorGroup behaviorGroup2 in list2)
					{
						this.TextBehaviorWindows[behaviorGroup2].DataContext = null;
						if (this.TextBehaviorWindows[behaviorGroup2].IsLoaded)
						{
							this.TextBehaviorWindows[behaviorGroup2].Close();
						}
						this.TextBehaviorWindows.Remove(behaviorGroup2);
					}
				}
				IL_01C4:
				base.RaisePropertyChanged("TextBehaviorWindows");
				base.RaisePropertyChanged("TextBehaviorGroups");
			};
			this.TimerBehaviorWindows = (from o in BehaviorGroup.All
				where o.BehaviorType == BehaviorTypes.Timer
				orderby o.Name
				select o).ToDictionary((BehaviorGroup o) => o, (BehaviorGroup n) => TimerWindow.NewWindow(n));
			if (Configuration.Current.EnableTimers)
			{
				foreach (KeyValuePair<BehaviorGroup, TimerWindow> keyValuePair2 in this.TimerBehaviorWindows)
				{
					keyValuePair2.Value.Show();
				}
			}
			BehaviorGroup.All.CollectionChanged += delegate(object o, NotifyCollectionChangedEventArgs e)
			{
				if (e.Action == NotifyCollectionChangedAction.Add)
				{
					using (IEnumerator<BehaviorGroup> enumerator7 = (from BehaviorGroup n in e.NewItems
						where n.BehaviorType == BehaviorTypes.Timer
						select n).GetEnumerator())
					{
						while (enumerator7.MoveNext())
						{
							BehaviorGroup behaviorGroup3 = enumerator7.Current;
							TimerWindow timerWindow = TimerWindow.NewWindow(behaviorGroup3);
							this.TimerBehaviorWindows.Add(behaviorGroup3, timerWindow);
						}
						goto IL_01C4;
					}
				}
				if (e.Action == NotifyCollectionChangedAction.Remove || e.Action == NotifyCollectionChangedAction.Reset)
				{
					List<BehaviorGroup> list3;
					if (e.Action != NotifyCollectionChangedAction.Remove)
					{
						list3 = (from n in this.TimerBehaviorWindows
							where n.Key.BehaviorType == BehaviorTypes.Timer
							select n.Key).ToList<BehaviorGroup>();
					}
					else
					{
						list3 = this.TimerBehaviorWindows.Join(e.OldItems.Cast<BehaviorGroup>(), (KeyValuePair<BehaviorGroup, TimerWindow> m) => m.Key, (BehaviorGroup n) => n, (KeyValuePair<BehaviorGroup, TimerWindow> n, BehaviorGroup m) => n.Key).ToList<BehaviorGroup>();
					}
					List<BehaviorGroup> list4 = list3;
					foreach (BehaviorGroup behaviorGroup4 in list4)
					{
						this.TimerBehaviorWindows[behaviorGroup4].DataContext = null;
						if (this.TimerBehaviorWindows[behaviorGroup4].IsLoaded)
						{
							this.TimerBehaviorWindows[behaviorGroup4].Close();
						}
						this.TimerBehaviorWindows.Remove(behaviorGroup4);
					}
				}
				IL_01C4:
				base.RaisePropertyChanged("TimerBehaviorWindows");
				base.RaisePropertyChanged("TimerBehaviorGroups");
			};
			Configuration.Current.PropertyChanged += delegate(object o, PropertyChangedEventArgs e)
			{
				string propertyName;
				if ((propertyName = e.PropertyName) != null)
				{
					if (!(propertyName == "EnableText"))
					{
						if (!(propertyName == "EnableTimers"))
						{
							return;
						}
					}
					else
					{
						using (Dictionary<BehaviorGroup, TextWindow>.Enumerator enumerator9 = this.TextBehaviorWindows.GetEnumerator())
						{
							while (enumerator9.MoveNext())
							{
								KeyValuePair<BehaviorGroup, TextWindow> keyValuePair3 = enumerator9.Current;
								if (!keyValuePair3.Value.IsVisible && Configuration.Current.EnableText)
								{
									keyValuePair3.Value.Show();
								}
								else if (keyValuePair3.Value.IsVisible && !Configuration.Current.EnableText)
								{
									keyValuePair3.Value.Hide();
								}
							}
							return;
						}
					}
					foreach (KeyValuePair<BehaviorGroup, TimerWindow> keyValuePair4 in this.TimerBehaviorWindows)
					{
						if (!keyValuePair4.Value.IsVisible && Configuration.Current.EnableTimers)
						{
							keyValuePair4.Value.Show();
						}
						else if (keyValuePair4.Value.IsVisible && !Configuration.Current.EnableTimers)
						{
							keyValuePair4.Value.Hide();
						}
					}
				}
			};
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x060002CA RID: 714 RVA: 0x00009E0C File Offset: 0x0000800C
		// (set) Token: 0x060002CB RID: 715 RVA: 0x00009E19 File Offset: 0x00008019
		public ObservableCollection<CharacterViewModel> Characters
		{
			get
			{
				return base.Get<ObservableCollection<CharacterViewModel>>("Characters");
			}
			set
			{
				base.Set("Characters", value);
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x060002CC RID: 716 RVA: 0x00009E27 File Offset: 0x00008027
		// (set) Token: 0x060002CD RID: 717 RVA: 0x00009E34 File Offset: 0x00008034
		public ObservableCollection<TriggerMatchedEventArgs> LoggedMatches
		{
			get
			{
				return base.Get<ObservableCollection<TriggerMatchedEventArgs>>("LoggedMatches");
			}
			set
			{
				base.Set("LoggedMatches", value);
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x060002CE RID: 718 RVA: 0x00009E42 File Offset: 0x00008042
		// (set) Token: 0x060002CF RID: 719 RVA: 0x00009E4F File Offset: 0x0000804F
		public Dictionary<BehaviorGroup, TextWindow> TextBehaviorWindows
		{
			get
			{
				return base.Get<Dictionary<BehaviorGroup, TextWindow>>("TextBehaviorWindows");
			}
			set
			{
				base.Set("TextBehaviorWindows", value);
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x060002D0 RID: 720 RVA: 0x00009E5D File Offset: 0x0000805D
		// (set) Token: 0x060002D1 RID: 721 RVA: 0x00009E6A File Offset: 0x0000806A
		public Dictionary<BehaviorGroup, TimerWindow> TimerBehaviorWindows
		{
			get
			{
				return base.Get<Dictionary<BehaviorGroup, TimerWindow>>("TimerBehaviorWindows");
			}
			set
			{
				base.Set("TimerBehaviorWindows", value);
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x060002D2 RID: 722 RVA: 0x00009E78 File Offset: 0x00008078
		public Configuration Configuration
		{
			get
			{
				return Configuration.Current;
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x060002D3 RID: 723 RVA: 0x00009E8A File Offset: 0x0000808A
		public IEnumerable<BehaviorGroup> TextBehaviorGroups
		{
			get
			{
				return BehaviorGroup.All.Where((BehaviorGroup o) => o.BehaviorType == BehaviorTypes.Text);
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x060002D4 RID: 724 RVA: 0x00009EBE File Offset: 0x000080BE
		public IEnumerable<BehaviorGroup> TimerBehaviorGroups
		{
			get
			{
				return BehaviorGroup.All.Where((BehaviorGroup o) => o.BehaviorType == BehaviorTypes.Timer);
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x060002D5 RID: 725 RVA: 0x00009EE7 File Offset: 0x000080E7
		public ObservableCollection<TriggerCategory> TriggerCategories
		{
			get
			{
				return TriggerCategory.All;
			}
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x00009EF0 File Offset: 0x000080F0
		public void LogTriggerMatch(TriggerMatchedEventArgs e)
		{
			if (Configuration.Current.DisplayMatches)
			{
				if (this.LoggedMatches.Count > Configuration.Current.MatchDisplayLimit)
				{
					int num = this.LoggedMatches.Count - Configuration.Current.MatchDisplayLimit;
					for (int i = 0; i < num; i++)
					{
						this.LoggedMatches.RemoveAt(this.LoggedMatches.Count - 1);
					}
				}
				this.LoggedMatches.Insert(0, e);
			}
			if (Configuration.Current.LogMatchesToFile && !string.IsNullOrWhiteSpace(Configuration.Current.MatchLogFileName))
			{
				try
				{
					lock (this.LogFileLockObject)
					{
						File.AppendAllText(Configuration.Current.MatchLogFileName, e.ExportText + "\r\n");
					}
				}
				catch
				{
				}
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x060002D7 RID: 727 RVA: 0x00009FE4 File Offset: 0x000081E4
		// (set) Token: 0x060002D8 RID: 728 RVA: 0x00009FF1 File Offset: 0x000081F1
		public string ErrorTitle
		{
			get
			{
				return base.Get<string>("ErrorTitle");
			}
			set
			{
				base.Set("ErrorTitle", value);
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x060002D9 RID: 729 RVA: 0x00009FFF File Offset: 0x000081FF
		// (set) Token: 0x060002DA RID: 730 RVA: 0x0000A00C File Offset: 0x0000820C
		public string ErrorMessage
		{
			get
			{
				return base.Get<string>("ErrorMessage");
			}
			set
			{
				base.Set("ErrorMessage", value);
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x060002DB RID: 731 RVA: 0x0000A023 File Offset: 0x00008223
		public GenericCommand CloseErrorCommand
		{
			get
			{
				return new GenericCommand(delegate(object p)
				{
					this.ErrorMessage = null;
				});
			}
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0000A036 File Offset: 0x00008236
		public void ShowError(string errorTitle, string errorMessage)
		{
			this.ErrorTitle = errorTitle;
			this.ErrorMessage = errorMessage;
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0000A046 File Offset: 0x00008246
		public void CopyTextToClipboard(string text)
		{
			if (!Clipboard.SetText(text))
			{
				this.ShowError("Copy Failed", "An error occurred when attempting to copy the data to the clipboard.  This is typically caused by another application locking access to the Clipboard.");
			}
		}

		// Token: 0x04000065 RID: 101
		private static GINAData _Current;

		// Token: 0x04000066 RID: 102
		private object LogFileLockObject = new object();
	}
}
