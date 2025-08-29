using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;
using BusinessShared;
using GimaSoft.Business.GINA;
using GimaSoft.Communication.GINA;
using GimaSoft.Service.GINA;
using WPFShared;

namespace GimaSoft.GINA
{
	// Token: 0x02000005 RID: 5
	public class DownloadControl : BindableControl
	{
		// Token: 0x06000055 RID: 85 RVA: 0x00002E92 File Offset: 0x00001092
		static DownloadControl()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DownloadControl), new FrameworkPropertyMetadata(typeof(DownloadControl)));
			BindableControl.SetDependentProperties(typeof(DownloadControl));
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00002ED0 File Offset: 0x000010D0
		public static void RegisterDependentProperties(Type type)
		{
			BindableControl.RegisterDependentProperty(type, "SharingPrompt", "PackageInProgress", null);
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00003118 File Offset: 0x00001318
		public DownloadControl()
		{
			this.AutomaticMerge = true;
			base.PropertyChanged += delegate(object o, PropertyChangedEventArgs e)
			{
				if (e.PropertyName == "SelectedCategory" || e.PropertyName == "SelectedSubCategory" || e.PropertyName == "SearchText")
				{
					this.UpdateFilter();
				}
			};
			Package.SharesDetected.CollectionChanged += delegate(object o, NotifyCollectionChangedEventArgs e)
			{
				if (e.NewItems != null)
				{
					this.StartNextPackage();
				}
			};
			this._Transfer.ConnectionEstablished += delegate(object o, ConnectionEstablishedEventArgs e)
			{
				Configuration.LogDebug("Started Download Control: Transfer Connection Established");
				this.Status = ((this.IsRepository && this.PackageInProgress.SessionId == Guid.Empty) ? 7 : 3);
				Configuration.LogDebug("Completed Download Control: Transfer Connection Established");
			};
			this._Transfer.RepositoryLoaded += delegate(object o, RepositoryLoadedEventArgs e)
			{
				this.Status = 8;
				Configuration.Current.RepositoryLastViewed = new DateTimeOffset?(DateTimeOffset.Now);
				this.RepositoryEntries = this._Transfer.RepositoryEntries.Select((RepositoryEntry n) => new RepositoryEntryFacade(n)).ToList<RepositoryEntryFacade>();
				foreach (RepositoryEntryFacade repositoryEntryFacade in this.RepositoryEntries)
				{
					repositoryEntryFacade.PropertyChanged += delegate(object oi, PropertyChangedEventArgs ei)
					{
						if (ei.PropertyName == "IsSelected")
						{
							base.RaisePropertyChanged("EntriesSelected");
						}
					};
				}
				base.RaisePropertyChanged("EntriesSelected");
				base.RaisePropertyChanged("Categories");
			};
			this._Transfer.ConnectionVersionFailed += delegate(object o, ConnectionVersionFailedEventArgs e)
			{
				this.Status = 9;
				this.ErrorMessage = e.VersionMessage;
			};
			this._Transfer.ConnectionFailed += delegate(object o, ConnectionFailedEventArgs e)
			{
				Configuration.LogDebug("Started Download Control: Transfer Connection Failed");
				this.ErrorMessage = "Unable to connect to Sharing Service";
				Configuration.LogDebug("Completed Download Control: Transfer Connection Failed");
			};
			this._Transfer.ConnectionError += delegate(object o, ConnectionErrorEventArgs e)
			{
				Configuration.LogDebug("Started Download Control: Conenction Error");
				this.ErrorMessage = string.Format("Connection error occurred: {0}", e.ErrorMessage);
			};
			this._Transfer.ChunkTransferred += delegate(object o, ChunkTransferredEventArgs e)
			{
				Configuration.LogDebug("Started Download Control: Transfer ChunkTransferred");
				this.DownloadedBytes = e.TotalTransferred;
				this.TotalBytes = e.TotalSize;
				Configuration.LogDebug("Completed Download Control: Transfer ChunkTransferred");
			};
			this._Transfer.ChunkFailed += delegate(object o, ChunkFailedEventArgs e)
			{
				this.ErrorMessage = "Error occurred while downloading package";
			};
			this._Transfer.DownloadCompleted += delegate(object o, TransferCompletedEventArgs e)
			{
				Configuration.LogDebug("Started Download Control: Transfer DownloadCompleted");
				this.PostDownload();
				Configuration.LogDebug("Completed Download Control: Transfer DownloadCompleted");
			};
			this.StartNextPackage();
		}

		// Token: 0x06000058 RID: 88 RVA: 0x0000329C File Offset: 0x0000149C
		public void StartNextPackage()
		{
			List<ShareDetectedEventArgs> list = Package.SharesDetected.Where((ShareDetectedEventArgs o) => !o.ProcessInvitation && o != this.PackageInProgress).ToList<ShareDetectedEventArgs>();
			foreach (ShareDetectedEventArgs shareDetectedEventArgs in list)
			{
				Package.DiscardShare(shareDetectedEventArgs.SessionId);
			}
			if (Package.SharesDetected.Any<ShareDetectedEventArgs>() && Configuration.Current.AllowSharedPackages && this.PackageInProgress == null)
			{
				this.PackageInProgress = Package.SharesDetected.First<ShareDetectedEventArgs>();
				this.Status = 0;
				this.ErrorMessage = null;
				this.DownloadedBytes = 0;
				this.TotalBytes = 1;
				if (this.PackageInProgress.ShareType == PackageShareType.Repository)
				{
					this.Status = 2;
					this._Transfer.GetRepositoryEntries();
					return;
				}
				if (this.PackageInProgress.IsFileImport)
				{
					this._Transfer.DownloadedPackage = this.PackageInProgress.FilePackage;
					this.SetupTrees();
					this.Status = 4;
					return;
				}
				if (this.PackageInProgress.AutoMerge)
				{
					this.Download();
					return;
				}
				this.Status = 1;
			}
		}

		// Token: 0x06000059 RID: 89 RVA: 0x000033CC File Offset: 0x000015CC
		private void Download()
		{
			if (this.PackageInProgress.FilePackage != null)
			{
				this._Transfer.DownloadedPackage = this.PackageInProgress.FilePackage;
				this.PostDownload();
				return;
			}
			this.Status = 2;
			this._Transfer.Download(this.PackageInProgress);
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00003424 File Offset: 0x00001624
		private void PostDownload()
		{
			if (this.PackageInProgress.AutoMerge)
			{
				base.Dispatcher.Invoke(DispatcherPriority.Normal, new ThreadStart(delegate
				{
					this.Merge();
				}));
				return;
			}
			this.SetupTrees();
			this.Status = 4;
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00003470 File Offset: 0x00001670
		private string ResolveMediaFileName(string filename, byte[] hash)
		{
			string directoryName = Path.GetDirectoryName(filename);
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filename);
			string extension = Path.GetExtension(filename);
			int num = 0;
			Match match = Regex.Match(fileNameWithoutExtension, ".+_GV(?<version>\\d+)$");
			if (match.Success)
			{
				num = Convert.ToInt32(match.Groups["version"].Value);
			}
			byte[] array = null;
			do
			{
				filename = Path.Combine(directoryName, string.Format("{0}{1}{2}", fileNameWithoutExtension, (num == 0) ? "" : ("_GV" + num.ToString()), extension));
				if (File.Exists(filename))
				{
					using (FileStream fileStream = File.OpenRead(filename))
					{
						array = DownloadControl.Hasher.ComputeHash(fileStream);
					}
				}
				num++;
			}
			while (File.Exists(filename) && !StructuralComparisons.StructuralEqualityComparer.Equals(hash, array));
			return filename;
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00003614 File Offset: 0x00001814
		private void Merge()
		{
			TriggerGroup triggerGroup = (this.AutomaticMerge ? this._Transfer.DownloadedPackage.RootGroup : this.TargetRootGroup);
			List<GimaSoft.Business.GINA.Trigger> list = (from o in (from o in triggerGroup.DescendantTree.SelectMany((TriggerGroup o) => o.Triggers)
					where o.NeedsMerge
					select o).SelectMany((GimaSoft.Business.GINA.Trigger o) => new GimaSoft.Business.GINA.Trigger[] { o, o.TimerEndingTrigger, o.TimerEndedTrigger })
				where o != null && o.MediaFileId != null
				select o).ToList<GimaSoft.Business.GINA.Trigger>();
			List<int> list2 = list.Select((GimaSoft.Business.GINA.Trigger o) => o.MediaFileId.Value).Distinct<int>().ToList<int>();
			using (IEnumerator<Package.PrerecordedFile> enumerator = (from o in list2
				join n in this._Transfer.DownloadedPackage.MediaFiles on o equals n.FileId
				select n).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Package.PrerecordedFile file = enumerator.Current;
					try
					{
						if (!Directory.Exists(Configuration.Current.ImportedMediaFileFolder))
						{
							Directory.CreateDirectory(Configuration.Current.ImportedMediaFileFolder);
						}
						string text = this.ResolveMediaFileName(Path.Combine(Configuration.Current.ImportedMediaFileFolder, file.Filename), DownloadControl.Hasher.ComputeHash(file.Data));
						if (!File.Exists(text))
						{
							File.WriteAllBytes(text, file.Data);
						}
						foreach (GimaSoft.Business.GINA.Trigger trigger in list.Where((GimaSoft.Business.GINA.Trigger o) => o.MediaFileId == file.FileId).ToList<GimaSoft.Business.GINA.Trigger>())
						{
							trigger.MediaFileName = text;
						}
					}
					catch
					{
					}
				}
			}
			foreach (TriggerGroup triggerGroup2 in triggerGroup.Groups)
			{
				triggerGroup2.Merge(TriggerGroup.RootGroup, false);
			}
			Configuration.SaveConfiguration(false);
			this.FinalizePackage();
		}

		// Token: 0x0600005D RID: 93 RVA: 0x000038F8 File Offset: 0x00001AF8
		private void DisposeTrees()
		{
			if (this.SourceRootVM != null)
			{
				this.SourceRootVM.Dispose();
				this.SourceRootVM = null;
			}
			if (this.SourceRootGroup != null)
			{
				this.SourceRootGroup.Dispose();
				this.SourceRootGroup = null;
			}
			if (this.TargetRootVM != null)
			{
				this.TargetRootVM.Dispose();
				this.TargetRootVM = null;
			}
			if (this.TargetRootGroup != null)
			{
				this.TargetRootGroup.Dispose();
				this.TargetRootGroup = null;
			}
		}

		// Token: 0x0600005E RID: 94 RVA: 0x000039D4 File Offset: 0x00001BD4
		private void SetupTrees()
		{
			base.Dispatcher.Invoke(DispatcherPriority.Normal, new ThreadStart(delegate
			{
				this.DisposeTrees();
				this.SourceRootGroup = this._Transfer.DownloadedPackage.RootGroup.Clone();
				this.SourceRootVM = new TriggerGroupViewModel(null, this.SourceRootGroup, false);
				this.TargetRootGroup = TriggerGroup.RootGroup.Clone();
				this.TargetRootVM = new TriggerGroupViewModel(null, this.TargetRootGroup, false);
			}));
		}

		// Token: 0x0600005F RID: 95 RVA: 0x000039F0 File Offset: 0x00001BF0
		private void FinalizePackage()
		{
			Package.MarkProcessedShare(this.PackageInProgress);
			this.DisposeTrees();
			this.PackageInProgress = null;
			this.StartNextPackage();
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000060 RID: 96 RVA: 0x00003A10 File Offset: 0x00001C10
		// (set) Token: 0x06000061 RID: 97 RVA: 0x00003A1D File Offset: 0x00001C1D
		public ShareDetectedEventArgs PackageInProgress
		{
			get
			{
				return base.Get<ShareDetectedEventArgs>("PackageInProgress");
			}
			set
			{
				base.Set("PackageInProgress", value);
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000062 RID: 98 RVA: 0x00003A2B File Offset: 0x00001C2B
		// (set) Token: 0x06000063 RID: 99 RVA: 0x00003A38 File Offset: 0x00001C38
		public int Status
		{
			get
			{
				return base.Get<int>("Status");
			}
			set
			{
				base.Set("Status", value);
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000064 RID: 100 RVA: 0x00003A4B File Offset: 0x00001C4B
		// (set) Token: 0x06000065 RID: 101 RVA: 0x00003A58 File Offset: 0x00001C58
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

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000066 RID: 102 RVA: 0x00003A66 File Offset: 0x00001C66
		// (set) Token: 0x06000067 RID: 103 RVA: 0x00003A73 File Offset: 0x00001C73
		public int TotalBytes
		{
			get
			{
				return base.Get<int>("TotalBytes");
			}
			set
			{
				base.Set("TotalBytes", value);
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000068 RID: 104 RVA: 0x00003A86 File Offset: 0x00001C86
		// (set) Token: 0x06000069 RID: 105 RVA: 0x00003A93 File Offset: 0x00001C93
		public int DownloadedBytes
		{
			get
			{
				return base.Get<int>("DownloadedBytes");
			}
			set
			{
				base.Set("DownloadedBytes", value);
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600006A RID: 106 RVA: 0x00003AA6 File Offset: 0x00001CA6
		public string SharingPrompt
		{
			get
			{
				return string.Format("{0} is attempting to share a trigger package with you.", (this.PackageInProgress != null && this.PackageInProgress.Sharer != null) ? this.PackageInProgress.Sharer : "Someone");
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600006B RID: 107 RVA: 0x00003AD9 File Offset: 0x00001CD9
		// (set) Token: 0x0600006C RID: 108 RVA: 0x00003AE6 File Offset: 0x00001CE6
		public bool AutomaticMerge
		{
			get
			{
				return base.Get<bool>("AutomaticMerge");
			}
			set
			{
				base.Set("AutomaticMerge", value);
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600006D RID: 109 RVA: 0x00003AF9 File Offset: 0x00001CF9
		// (set) Token: 0x0600006E RID: 110 RVA: 0x00003B06 File Offset: 0x00001D06
		public TriggerGroup SourceRootGroup
		{
			get
			{
				return base.Get<TriggerGroup>("SourceRootGroup");
			}
			set
			{
				base.Set("SourceRootGroup", value);
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600006F RID: 111 RVA: 0x00003B14 File Offset: 0x00001D14
		// (set) Token: 0x06000070 RID: 112 RVA: 0x00003B21 File Offset: 0x00001D21
		public TriggerGroup TargetRootGroup
		{
			get
			{
				return base.Get<TriggerGroup>("TargetRootGroup");
			}
			set
			{
				base.Set("TargetRootGroup", value);
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000071 RID: 113 RVA: 0x00003B2F File Offset: 0x00001D2F
		// (set) Token: 0x06000072 RID: 114 RVA: 0x00003B3C File Offset: 0x00001D3C
		public TriggerGroupViewModel SourceRootVM
		{
			get
			{
				return base.Get<TriggerGroupViewModel>("SourceRootVM");
			}
			set
			{
				base.Set("SourceRootVM", value);
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000073 RID: 115 RVA: 0x00003B4A File Offset: 0x00001D4A
		// (set) Token: 0x06000074 RID: 116 RVA: 0x00003B57 File Offset: 0x00001D57
		public TriggerGroupViewModel TargetRootVM
		{
			get
			{
				return base.Get<TriggerGroupViewModel>("TargetRootVM");
			}
			set
			{
				base.Set("TargetRootVM", value);
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000075 RID: 117 RVA: 0x00003B65 File Offset: 0x00001D65
		private bool IsRepository
		{
			get
			{
				return this.PackageInProgress.ShareType == PackageShareType.Repository;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000076 RID: 118 RVA: 0x00003B75 File Offset: 0x00001D75
		// (set) Token: 0x06000077 RID: 119 RVA: 0x00003B82 File Offset: 0x00001D82
		public List<RepositoryEntryFacade> RepositoryEntries
		{
			get
			{
				return base.Get<List<RepositoryEntryFacade>>("RepositoryEntries");
			}
			set
			{
				base.Set("RepositoryEntries", value);
				this.RepositoryEntriesView = CollectionViewSource.GetDefaultView(value);
				this.UpdateFilter();
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000078 RID: 120 RVA: 0x00003BAA File Offset: 0x00001DAA
		public int EntriesSelected
		{
			get
			{
				if (this.RepositoryEntries == null)
				{
					return 0;
				}
				return this.RepositoryEntries.Count((RepositoryEntryFacade o) => o.IsSelected);
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000079 RID: 121 RVA: 0x00003BDE File Offset: 0x00001DDE
		// (set) Token: 0x0600007A RID: 122 RVA: 0x00003BEB File Offset: 0x00001DEB
		public ICollectionView RepositoryEntriesView
		{
			get
			{
				return base.Get<ICollectionView>("RepositoryEntriesView");
			}
			set
			{
				base.Set("RepositoryEntriesView", value);
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600007B RID: 123 RVA: 0x00003C0C File Offset: 0x00001E0C
		public IEnumerable<string> Categories
		{
			get
			{
				if (this.RepositoryEntries == null)
				{
					return null;
				}
				return from o in this.RepositoryEntries.Select((RepositoryEntryFacade o) => o.Entry.Category).Distinct<string>().Union(new string[] { "" })
					orderby o
					select o;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600007C RID: 124 RVA: 0x00003CB0 File Offset: 0x00001EB0
		public IEnumerable<string> SubCategories
		{
			get
			{
				if (this.RepositoryEntries == null)
				{
					return null;
				}
				return from o in (from o in this.RepositoryEntries
						where o.Entry.Category == this.SelectedCategory
						select o.Entry.SubCategory).Distinct<string>().Union(new string[] { "" })
					orderby o
					select o;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600007D RID: 125 RVA: 0x00003D3C File Offset: 0x00001F3C
		// (set) Token: 0x0600007E RID: 126 RVA: 0x00003D49 File Offset: 0x00001F49
		public string SelectedCategory
		{
			get
			{
				return base.Get<string>("SelectedCategory");
			}
			set
			{
				base.Set("SelectedCategory", value);
				this.SelectedSubCategory = null;
				base.RaisePropertyChanged("SubCategories");
				this.UpdateFilter();
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600007F RID: 127 RVA: 0x00003D6F File Offset: 0x00001F6F
		// (set) Token: 0x06000080 RID: 128 RVA: 0x00003D7C File Offset: 0x00001F7C
		public string SelectedSubCategory
		{
			get
			{
				return base.Get<string>("SelectedSubCategory");
			}
			set
			{
				base.Set("SelectedSubCategory", value);
				this.UpdateFilter();
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000081 RID: 129 RVA: 0x00003D90 File Offset: 0x00001F90
		// (set) Token: 0x06000082 RID: 130 RVA: 0x00003D9D File Offset: 0x00001F9D
		public string SearchText
		{
			get
			{
				return base.Get<string>("SearchText");
			}
			set
			{
				base.Set("SearchText", value);
				this.UpdateFilter();
			}
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00003EAC File Offset: 0x000020AC
		private void UpdateFilter()
		{
			if (this.RepositoryEntriesView != null)
			{
				base.Dispatcher.Invoke(DispatcherPriority.Normal, new ThreadStart(delegate
				{
					ICollectionView repositoryEntriesView = this.RepositoryEntriesView;
					repositoryEntriesView.Filter = (Predicate<object>)Delegate.Combine(repositoryEntriesView.Filter, new Predicate<object>(delegate(object o)
					{
						RepositoryEntry entry = (o as RepositoryEntryFacade).Entry;
						return (string.IsNullOrWhiteSpace(this.SelectedCategory) || entry.Category == this.SelectedCategory) && (string.IsNullOrWhiteSpace(this.SelectedSubCategory) || entry.SubCategory == this.SelectedSubCategory) && (string.IsNullOrWhiteSpace(this.SearchText) || entry.Name.Contains(this.SearchText, true) || entry.Credits.Contains(this.SearchText, true) || entry.Category.Contains(this.SearchText, true) || entry.SubCategory.Contains(this.SearchText, true) || entry.Comments.Contains(this.SearchText, true));
					}));
				}));
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000084 RID: 132 RVA: 0x00003EEC File Offset: 0x000020EC
		public GenericCommand AcceptCommand
		{
			get
			{
				return new GenericCommand
				{
					Execute = delegate(object p)
					{
						this.Download();
					}
				};
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000085 RID: 133 RVA: 0x00003F1C File Offset: 0x0000211C
		public GenericCommand DeclineCommand
		{
			get
			{
				return new GenericCommand
				{
					Execute = delegate(object p)
					{
						this.FinalizePackage();
					}
				};
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000086 RID: 134 RVA: 0x00003F4C File Offset: 0x0000214C
		public GenericCommand CloseErrorCommand
		{
			get
			{
				return new GenericCommand
				{
					Execute = delegate(object p)
					{
						this.FinalizePackage();
					}
				};
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000087 RID: 135 RVA: 0x00003FE4 File Offset: 0x000021E4
		public GenericCommand IsDropAllowedCommand
		{
			get
			{
				GenericCommand genericCommand = new GenericCommand();
				genericCommand.Execute = delegate(object p)
				{
					TreeViewDropBehavior.CanDropEventArgs canDropEventArgs = p as TreeViewDropBehavior.CanDropEventArgs;
					if (canDropEventArgs == null)
					{
						return;
					}
					if (canDropEventArgs.DestinationObject == null)
					{
						if (canDropEventArgs.SourceObject is TriggerGroupViewModel && ((TriggerGroupViewModel)canDropEventArgs.SourceObject).TriggerGroup.ParentGroup != null)
						{
							canDropEventArgs.Effects = DragDropEffects.Move;
							return;
						}
						canDropEventArgs.Effects = DragDropEffects.None;
						return;
					}
					else
					{
						if (canDropEventArgs.DestinationObject is TriggerGroupViewModel)
						{
							canDropEventArgs.Effects = DragDropEffects.Move;
							return;
						}
						canDropEventArgs.Effects = DragDropEffects.None;
						return;
					}
				};
				return genericCommand;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000088 RID: 136 RVA: 0x000040F0 File Offset: 0x000022F0
		public GenericCommand MoveTriggerOrGroupCommand
		{
			get
			{
				return new GenericCommand
				{
					Execute = delegate(object p)
					{
						TreeViewDropBehavior.CanDropEventArgs canDropEventArgs = p as TreeViewDropBehavior.CanDropEventArgs;
						if (canDropEventArgs.DestinationObject is TriggerViewModel)
						{
							return;
						}
						TriggerGroupViewModel triggerGroupViewModel = canDropEventArgs.DestinationObject as TriggerGroupViewModel;
						TriggerGroup triggerGroup = ((triggerGroupViewModel != null) ? triggerGroupViewModel.TriggerGroup : this.TargetRootGroup);
						if (canDropEventArgs.SourceObject is TriggerGroupViewModel)
						{
							TriggerGroup triggerGroup2 = ((TriggerGroupViewModel)canDropEventArgs.SourceObject).TriggerGroup;
							if ((triggerGroup2.Name ?? "").ToLower() == (triggerGroup.Name ?? "").ToLower())
							{
								triggerGroup = triggerGroup.ParentGroup;
							}
							triggerGroup2.Move(triggerGroup, -1);
							return;
						}
						if (canDropEventArgs.SourceObject is TriggerViewModel)
						{
							GimaSoft.Business.GINA.Trigger trigger = ((TriggerViewModel)canDropEventArgs.SourceObject).Trigger;
							if (triggerGroup != null)
							{
								trigger.Move(triggerGroup, null);
							}
						}
					}
				};
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000089 RID: 137 RVA: 0x0000411E File Offset: 0x0000231E
		public GenericCommand MergeCommand
		{
			get
			{
				return new GenericCommand(delegate(object p)
				{
					this.Merge();
				});
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x0600008A RID: 138 RVA: 0x00004200 File Offset: 0x00002400
		public GenericCommand DownloadRepositoryEntriesCommand
		{
			get
			{
				return new GenericCommand
				{
					Execute = delegate(object p)
					{
						if (!this.RepositoryEntries.Any((RepositoryEntryFacade o) => o.IsSelected))
						{
							return;
						}
						this.PackageInProgress.SessionId = Guid.NewGuid();
						this.PackageInProgress.SessionIds = (from o in this.RepositoryEntries
							where o.IsSelected
							select o.Entry.RepositoryId).ToList<Guid>();
						this.Status = 2;
						this.Download();
					}
				};
			}
		}

		// Token: 0x04000010 RID: 16
		private const int None = 0;

		// Token: 0x04000011 RID: 17
		private const int Prompt = 1;

		// Token: 0x04000012 RID: 18
		private const int Connecting = 2;

		// Token: 0x04000013 RID: 19
		private const int Downloading = 3;

		// Token: 0x04000014 RID: 20
		private const int DownloadCompleted = 4;

		// Token: 0x04000015 RID: 21
		private const int Merged = 5;

		// Token: 0x04000016 RID: 22
		private const int Error = 6;

		// Token: 0x04000017 RID: 23
		private const int GetRepository = 7;

		// Token: 0x04000018 RID: 24
		private const int BrowseRepository = 8;

		// Token: 0x04000019 RID: 25
		private const int InvalidVersion = 9;

		// Token: 0x0400001A RID: 26
		private static MD5 Hasher = MD5.Create();

		// Token: 0x0400001B RID: 27
		private TransferHelper _Transfer = new TransferHelper();
	}
}
