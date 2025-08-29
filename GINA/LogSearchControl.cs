using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using GimaSoft.Business.GINA;
using WPFShared;

namespace GimaSoft.GINA
{
	// Token: 0x02000008 RID: 8
	public class LogSearchControl : BindableControl
	{
		// Token: 0x060000DE RID: 222 RVA: 0x00004798 File Offset: 0x00002998
		static LogSearchControl()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(LogSearchControl), new FrameworkPropertyMetadata(typeof(LogSearchControl)));
			BindableControl.SetDependentProperties(typeof(LogSearchControl));
		}

		// Token: 0x060000DF RID: 223 RVA: 0x000047CC File Offset: 0x000029CC
		public static void RegisterDependentProperties(Type type)
		{
			BindableControl.RegisterDependentProperty(type, "AllowSearch", "Character", null);
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x000047E0 File Offset: 0x000029E0
		public LogSearchControl()
		{
			this.StartDate = DateTime.Today;
			this.EndDate = DateTime.Today.AddDays(1.0);
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x0000481A File Offset: 0x00002A1A
		public App App
		{
			get
			{
				return (App)Application.Current;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x00004826 File Offset: 0x00002A26
		// (set) Token: 0x060000E3 RID: 227 RVA: 0x00004833 File Offset: 0x00002A33
		public CharacterViewModel Character
		{
			get
			{
				return base.Get<CharacterViewModel>("Character");
			}
			set
			{
				base.Set("Character", value);
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x00004841 File Offset: 0x00002A41
		// (set) Token: 0x060000E5 RID: 229 RVA: 0x0000484E File Offset: 0x00002A4E
		public string SearchText
		{
			get
			{
				return base.Get<string>("SearchText");
			}
			set
			{
				base.Set("SearchText", value);
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x0000485C File Offset: 0x00002A5C
		// (set) Token: 0x060000E7 RID: 231 RVA: 0x00004869 File Offset: 0x00002A69
		public LogSearchTypes SearchType
		{
			get
			{
				return base.Get<LogSearchTypes>("SearchType");
			}
			set
			{
				base.Set("SearchType", value);
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x0000487C File Offset: 0x00002A7C
		// (set) Token: 0x060000E9 RID: 233 RVA: 0x00004889 File Offset: 0x00002A89
		public DateTime StartDate
		{
			get
			{
				return base.Get<DateTime>("StartDate");
			}
			set
			{
				base.Set("StartDate", value);
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060000EA RID: 234 RVA: 0x0000489C File Offset: 0x00002A9C
		// (set) Token: 0x060000EB RID: 235 RVA: 0x000048A9 File Offset: 0x00002AA9
		public DateTime EndDate
		{
			get
			{
				return base.Get<DateTime>("EndDate");
			}
			set
			{
				base.Set("EndDate", value);
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060000EC RID: 236 RVA: 0x000048BC File Offset: 0x00002ABC
		// (set) Token: 0x060000ED RID: 237 RVA: 0x000048C9 File Offset: 0x00002AC9
		public bool IncludeArchives
		{
			get
			{
				return base.Get<bool>("IncludeArchives");
			}
			set
			{
				base.Set("IncludeArchives", value);
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060000EE RID: 238 RVA: 0x000048DC File Offset: 0x00002ADC
		// (set) Token: 0x060000EF RID: 239 RVA: 0x000048E9 File Offset: 0x00002AE9
		public ObservableCollection<LogMatch> Matches
		{
			get
			{
				return base.Get<ObservableCollection<LogMatch>>("Matches");
			}
			set
			{
				base.Set("Matches", value);
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x000048F7 File Offset: 0x00002AF7
		// (set) Token: 0x060000F1 RID: 241 RVA: 0x00004904 File Offset: 0x00002B04
		public int MatchProgress
		{
			get
			{
				return base.Get<int>("MatchProgress");
			}
			set
			{
				base.Set("MatchProgress", value);
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x00004917 File Offset: 0x00002B17
		// (set) Token: 0x060000F3 RID: 243 RVA: 0x00004924 File Offset: 0x00002B24
		public long? LinesRead
		{
			get
			{
				return base.Get<long?>("LinesRead");
			}
			set
			{
				base.Set("LinesRead", value);
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x00004937 File Offset: 0x00002B37
		// (set) Token: 0x060000F5 RID: 245 RVA: 0x00004944 File Offset: 0x00002B44
		public long? LinesReadPerSecond
		{
			get
			{
				return base.Get<long?>("LinesReadPerSecond");
			}
			set
			{
				base.Set("LinesReadPerSecond", value);
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x00004957 File Offset: 0x00002B57
		// (set) Token: 0x060000F7 RID: 247 RVA: 0x00004964 File Offset: 0x00002B64
		public bool SearchInProgress
		{
			get
			{
				return base.Get<bool>("SearchInProgress");
			}
			set
			{
				base.Set("SearchInProgress", value);
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x00004977 File Offset: 0x00002B77
		// (set) Token: 0x060000F9 RID: 249 RVA: 0x00004984 File Offset: 0x00002B84
		public string NewTriggerText
		{
			get
			{
				return base.Get<string>("NewTriggerText");
			}
			set
			{
				base.Set("NewTriggerText", value);
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060000FA RID: 250 RVA: 0x00004992 File Offset: 0x00002B92
		// (set) Token: 0x060000FB RID: 251 RVA: 0x0000499F File Offset: 0x00002B9F
		public TriggerGroup SelectedTriggerGroup
		{
			get
			{
				return base.Get<TriggerGroup>("SelectedTriggerGroup");
			}
			set
			{
				base.Set("SelectedTriggerGroup", value);
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060000FC RID: 252 RVA: 0x000049AD File Offset: 0x00002BAD
		// (set) Token: 0x060000FD RID: 253 RVA: 0x000049BA File Offset: 0x00002BBA
		public GimaSoft.Business.GINA.Trigger EditingTrigger
		{
			get
			{
				return base.Get<GimaSoft.Business.GINA.Trigger>("EditingTrigger");
			}
			set
			{
				base.Set("EditingTrigger", value);
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060000FE RID: 254 RVA: 0x000049C8 File Offset: 0x00002BC8
		// (set) Token: 0x060000FF RID: 255 RVA: 0x000049D5 File Offset: 0x00002BD5
		public bool ShowTriggerEditor
		{
			get
			{
				return base.Get<bool>("ShowTriggerEditor");
			}
			set
			{
				base.Set("ShowTriggerEditor", value);
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000100 RID: 256 RVA: 0x000049E8 File Offset: 0x00002BE8
		// (set) Token: 0x06000101 RID: 257 RVA: 0x000049F5 File Offset: 0x00002BF5
		public bool ShowGroupSelection
		{
			get
			{
				return base.Get<bool>("ShowGroupSelection");
			}
			set
			{
				base.Set("ShowGroupSelection", value);
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000102 RID: 258 RVA: 0x00004A08 File Offset: 0x00002C08
		// (set) Token: 0x06000103 RID: 259 RVA: 0x00004A15 File Offset: 0x00002C15
		public TriggerGroupViewModel RootGroupViewModel
		{
			get
			{
				return base.Get<TriggerGroupViewModel>("RootGroupViewModel");
			}
			set
			{
				base.Set("RootGroupViewModel", value);
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000104 RID: 260 RVA: 0x00004A23 File Offset: 0x00002C23
		// (set) Token: 0x06000105 RID: 261 RVA: 0x00004A30 File Offset: 0x00002C30
		public bool ShowTriggerGroupEditor
		{
			get
			{
				return base.Get<bool>("ShowTriggerGroupEditor");
			}
			set
			{
				base.Set("ShowTriggerGroupEditor", value);
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000106 RID: 262 RVA: 0x00004A43 File Offset: 0x00002C43
		public bool AllowSearch
		{
			get
			{
				return this.Character != null;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000107 RID: 263 RVA: 0x00004A51 File Offset: 0x00002C51
		// (set) Token: 0x06000108 RID: 264 RVA: 0x00004A5E File Offset: 0x00002C5E
		public List<LogMatch> SelectedResults
		{
			get
			{
				return base.Get<List<LogMatch>>("SelectedResults");
			}
			set
			{
				base.Set("SelectedResults", value);
			}
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00004A6C File Offset: 0x00002C6C
		private void CleanupSearch()
		{
			if (this._Searcher != null)
			{
				this._Searcher.Cancel();
				this._Searcher.FileRangeLocated -= this.Searcher_FileRangeLocated;
				this._Searcher.MatchFound -= this.Searcher_MatchFound;
				this._Searcher.MatchProgressChanged -= this.Searcher_MatchProgressChanged;
				this._Searcher.SearchCompleted -= this.Searcher_SearchCompleted;
				this._Searcher.SearchCancelled -= this.Searcher_SearchCancelled;
				this._Searcher.SearchFailed -= this.Searcher_SearchFailed;
			}
			this.Matches = null;
			this.LinesRead = null;
			this.LinesReadPerSecond = null;
			this.SearchInProgress = false;
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00004B45 File Offset: 0x00002D45
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property == UIElement.VisibilityProperty && base.Visibility != Visibility.Visible)
			{
				this.CleanupSearch();
				this.RootGroupViewModel = null;
			}
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00004B71 File Offset: 0x00002D71
		private void Searcher_FileRangeLocated(object sender, EventArgs e)
		{
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00004BA8 File Offset: 0x00002DA8
		private void Searcher_MatchFound(object sender, LogSearcher.LogSearchMatchedEventArgs e)
		{
			base.Dispatcher.Invoke(DispatcherPriority.Normal, new ThreadStart(delegate
			{
				if (this.Matches != null)
				{
					this.Matches.Add(e.Match);
				}
			}));
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00004BE3 File Offset: 0x00002DE3
		private void Searcher_MatchProgressChanged(object sender, LogSearcher.MatchProgressChangedEventArgs e)
		{
			this.MatchProgress = e.Progress;
			this.LinesRead = new long?(e.LinesRead);
			this.LinesReadPerSecond = new long?(e.LinesPerSecond);
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00004C13 File Offset: 0x00002E13
		private void Searcher_SearchCompleted(object sender, EventArgs e)
		{
			this.SearchInProgress = false;
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00004C1C File Offset: 0x00002E1C
		private void Searcher_SearchCancelled(object sender, EventArgs e)
		{
			this.SearchInProgress = false;
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00004C28 File Offset: 0x00002E28
		private void Searcher_SearchFailed(object sender, LogSearcher.SearchFailedEventArgs e)
		{
			this.SearchInProgress = false;
			string text;
			switch (e.Reason)
			{
			case LogSearcher.SearchFailures.NoFile:
				text = "The character's log file was not found.";
				break;
			case LogSearcher.SearchFailures.FileAccess:
				text = "The character's log file could not be accessed.";
				break;
			case LogSearcher.SearchFailures.NoDataInRange:
				text = "No log entries were found during the requested time period.";
				break;
			default:
				text = "An error occurred while performing the search.";
				break;
			}
			this.App.Data.ShowError("Search Failed", text);
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000111 RID: 273 RVA: 0x00004DCC File Offset: 0x00002FCC
		public GenericCommand SearchCommand
		{
			get
			{
				return new GenericCommand(delegate(object p)
				{
					this.CleanupSearch();
					this._Searcher = new LogSearcher(this.Character.Character, this.SearchType, this.SearchText ?? "", (this.StartDate < this.EndDate) ? this.StartDate : this.EndDate, (this.EndDate > this.StartDate) ? this.EndDate : this.StartDate);
					this._Searcher.IncludeArchives = this.IncludeArchives;
					this._Searcher.FileRangeLocated += this.Searcher_FileRangeLocated;
					this._Searcher.MatchFound += this.Searcher_MatchFound;
					this._Searcher.MatchProgressChanged += this.Searcher_MatchProgressChanged;
					this._Searcher.SearchCompleted += this.Searcher_SearchCompleted;
					this._Searcher.SearchCancelled += this.Searcher_SearchCancelled;
					this._Searcher.SearchFailed += this.Searcher_SearchFailed;
					this.Matches = new ObservableCollection<LogMatch>();
					this.SearchInProgress = true;
					this._Searcher.Search();
				});
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000112 RID: 274 RVA: 0x00004DFB File Offset: 0x00002FFB
		public GenericCommand CancelSearchCommand
		{
			get
			{
				return new GenericCommand(delegate(object p)
				{
					if (this._Searcher != null)
					{
						this._Searcher.Cancel();
					}
					this.SearchInProgress = false;
				});
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000113 RID: 275 RVA: 0x00004E1D File Offset: 0x0000301D
		public GenericCommand CloseCommand
		{
			get
			{
				return new GenericCommand(delegate(object p)
				{
					this.CleanupSearch();
					base.Visibility = Visibility.Collapsed;
				});
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000114 RID: 276 RVA: 0x00004E64 File Offset: 0x00003064
		public GenericCommand EditTriggerCommand
		{
			get
			{
				return new GenericCommand(delegate(object p)
				{
					GimaSoft.Business.GINA.Trigger trigger = p as GimaSoft.Business.GINA.Trigger;
					if (p == null)
					{
						return;
					}
					this.EditingTrigger = trigger;
					this.SelectedTriggerGroup = null;
					this.NewTriggerText = null;
					this.ShowTriggerEditor = true;
				});
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000115 RID: 277 RVA: 0x00004EBC File Offset: 0x000030BC
		public GenericCommand SelectTriggerGroupCommand
		{
			get
			{
				return new GenericCommand(delegate(object p)
				{
					LogMatch logMatch = p as LogMatch;
					if (logMatch != null)
					{
						this.NewTriggerText = logMatch.SuggestedTriggerText;
					}
					if (this.RootGroupViewModel == null)
					{
						this.RootGroupViewModel = new TriggerGroupViewModel(null, TriggerGroup.RootGroup, true);
					}
					this.ShowGroupSelection = true;
				});
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000116 RID: 278 RVA: 0x00004F55 File Offset: 0x00003155
		public GenericCommand SelectionChangedCommand
		{
			get
			{
				return new GenericCommand(delegate(object p)
				{
					TreeCollection.TreeSelectionChangedEventArgs treeSelectionChangedEventArgs = p as TreeCollection.TreeSelectionChangedEventArgs;
					if (treeSelectionChangedEventArgs == null)
					{
						return;
					}
					this.SelectedTriggerGroup = (from o in treeSelectionChangedEventArgs.SelectedItems
						where o is TriggerGroupViewModel
						select ((TriggerGroupViewModel)o).TriggerGroup).FirstOrDefault<TriggerGroup>();
				});
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000117 RID: 279 RVA: 0x00004F71 File Offset: 0x00003171
		public GenericCommand CancelGroupSelectionCommand
		{
			get
			{
				return new GenericCommand(delegate(object p)
				{
					this.ShowGroupSelection = false;
				});
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000118 RID: 280 RVA: 0x00004FA4 File Offset: 0x000031A4
		public GenericCommand AddTriggerCommand
		{
			get
			{
				return new GenericCommand(delegate(object p)
				{
					this.ShowGroupSelection = false;
					if (this.SelectedTriggerGroup == null)
					{
						return;
					}
					this.EditingTrigger = null;
					this.ShowTriggerEditor = true;
				});
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000119 RID: 281 RVA: 0x00004FC9 File Offset: 0x000031C9
		public GenericCommand AddTriggerGroupCommand
		{
			get
			{
				return new GenericCommand(delegate(object p)
				{
					if (this.SelectedTriggerGroup == null)
					{
						return;
					}
					this.ShowTriggerGroupEditor = true;
				});
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600011A RID: 282 RVA: 0x0000501B File Offset: 0x0000321B
		public GenericCommand ReplaceTriggerTextCommand
		{
			get
			{
				return new GenericCommand(delegate(object p)
				{
					LogMatch logMatch = p as LogMatch;
					if (logMatch == null || logMatch.MatchedTrigger == null)
					{
						return;
					}
					this.EditingTrigger = logMatch.MatchedTrigger;
					this.NewTriggerText = logMatch.SuggestedTriggerText;
					this.ShowTriggerEditor = true;
				});
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600011B RID: 283 RVA: 0x000050F0 File Offset: 0x000032F0
		public GenericCommand SearchNextCommand
		{
			get
			{
				return new GenericCommand(delegate(object p)
				{
					List<object> list = p as List<object>;
					DateTime dateTime = (DateTime)list[0];
					int num = (int)list[1];
					string text = (string)list[2];
					this.StartDate = dateTime;
					string text2;
					if ((text2 = text) != null)
					{
						if (!(text2 == "S"))
						{
							if (!(text2 == "M"))
							{
								if (text2 == "H")
								{
									dateTime = dateTime.AddHours((double)num);
								}
							}
							else
							{
								dateTime = dateTime.AddMinutes((double)num);
							}
						}
						else
						{
							dateTime = dateTime.AddSeconds((double)num);
						}
					}
					this.EndDate = dateTime;
					this.SearchType = LogSearchTypes.Text;
					this.SearchText = string.Empty;
					this.SearchCommand.Execute(null);
				});
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600011C RID: 284 RVA: 0x000051C7 File Offset: 0x000033C7
		public GenericCommand SearchPrevCommand
		{
			get
			{
				return new GenericCommand(delegate(object p)
				{
					List<object> list = p as List<object>;
					DateTime dateTime = (DateTime)list[0];
					int num = (int)list[1];
					string text = (string)list[2];
					this.EndDate = dateTime;
					string text2;
					if ((text2 = text) != null)
					{
						if (!(text2 == "S"))
						{
							if (!(text2 == "M"))
							{
								if (text2 == "H")
								{
									dateTime = dateTime.AddHours((double)(-(double)num));
								}
							}
							else
							{
								dateTime = dateTime.AddMinutes((double)(-(double)num));
							}
						}
						else
						{
							dateTime = dateTime.AddSeconds((double)(-(double)num));
						}
					}
					this.StartDate = dateTime;
					this.SearchType = LogSearchTypes.Text;
					this.SearchText = string.Empty;
					this.SearchCommand.Execute(null);
				});
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600011D RID: 285 RVA: 0x0000528B File Offset: 0x0000348B
		public GenericCommand SetTimePeriod
		{
			get
			{
				return new GenericCommand(delegate(object p)
				{
					this.EndDate = DateTime.Now;
					string text;
					if ((text = p as string) != null)
					{
						if (text == "H")
						{
							this.StartDate = this.EndDate.AddHours(-1.0);
							return;
						}
						if (text == "D")
						{
							this.StartDate = this.EndDate.AddDays(-1.0);
							return;
						}
						if (!(text == "A"))
						{
							return;
						}
						this.StartDate = new DateTime(1990, 1, 1);
						this.EndDate = new DateTime(2100, 12, 31);
					}
				});
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600011E RID: 286 RVA: 0x000052D4 File Offset: 0x000034D4
		public GenericCommand ResultSelectionChangedCommand
		{
			get
			{
				return new GenericCommand(delegate(object p)
				{
					SelectionChangedEventArgs selectionChangedEventArgs = p as SelectionChangedEventArgs;
					this.SelectedResults = (selectionChangedEventArgs.Source as ListView).SelectedItems.Cast<LogMatch>().ToList<LogMatch>();
				});
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600011F RID: 287 RVA: 0x000053C4 File Offset: 0x000035C4
		public GenericCommand CopyResultsCommand
		{
			get
			{
				return new GenericCommand(delegate(object p)
				{
					StringBuilder stringBuilder = new StringBuilder();
					if (this.SelectedResults != null)
					{
						foreach (LogMatch logMatch in this.SelectedResults.Where((LogMatch o) => o.LoggedTime != null))
						{
							stringBuilder.AppendLine(string.Format("{0}\t{1}", logMatch.LoggedTime.Value.ToString("MM/dd/yy hh:mm:ss tt"), logMatch.MatchedText));
						}
					}
					this.App.Data.CopyTextToClipboard(stringBuilder.ToString());
				});
			}
		}

		// Token: 0x06000120 RID: 288 RVA: 0x000053EC File Offset: 0x000035EC
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			ListView listView = base.Template.FindName("SearchResults", this) as ListView;
			if (listView != null)
			{
				listView.CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, delegate(object o, ExecutedRoutedEventArgs e)
				{
					this.CopyResultsCommand.Execute(null);
				}));
			}
		}

		// Token: 0x04000036 RID: 54
		private LogSearcher _Searcher;
	}
}
