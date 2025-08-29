using GimaSoft.Business.GINA;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;
using WPFShared;

namespace GimaSoft.GINA
{
    // Token: 0x02000027 RID: 39
    public class MainWindowViewModel : GINAViewModel
    {
        // Token: 0x060003DC RID: 988 RVA: 0x0000D700 File Offset: 0x0000B900
        static MainWindowViewModel()
        {
            BindableObject.SetDependentProperties(typeof(MainWindowViewModel));
        }

        // Token: 0x060003DD RID: 989 RVA: 0x0000D714 File Offset: 0x0000B914
        public static void RegisterDependentProperties(Type type)
        {
            BindableObject.RegisterDependentProperty(type, "SelectedEntries", "SelectedViewModels", null);
            BindableObject.RegisterDependentProperty(type, "SelectedTrigger", "SelectedViewModels", null);
            BindableObject.RegisterDependentProperty(type, "SelectedTriggerGroup", "SelectedViewModels", null);
            BindableObject.RegisterDependentProperty(type, "SelectedTriggerGroupViewModel", "SelectedViewModels", null);
            BindableObject.RegisterDependentProperty(type, "SelectedIsExportable", "SelectedViewModels", null);
            BindableObject.RegisterDependentProperty(type, "CanPasteEntries", "SelectedViewModels", null);
        }

        // Token: 0x060003DE RID: 990 RVA: 0x0000D820 File Offset: 0x0000BA20
        public MainWindowViewModel()
        {
            RootTriggerGroup = new TriggerGroupViewModel(this, TriggerGroup.RootGroup, false);
            AvailableProcessors = new List<MainWindowViewModel.ProcessorItem>();
            for (var i = 1; i <= Environment.ProcessorCount; i++)
            {
                var processorItem = new MainWindowViewModel.ProcessorItem
                {
                    CPUNumber = i
                };
                AvailableProcessors.Add(processorItem);
            }
            TriggerGroup.GroupsChanged += delegate
            {
                if (ClipboardedEntries != null)
                {
                    ClipboardedEntries = null;
                }
            };
            _Timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(60.0),
                IsEnabled = true
            };
            _Timer.Tick += delegate (object o, EventArgs e)
            {
                GC.Collect(2);
            };
            if (Settings.EnableProfiler)
            {
                RefreshProfiler();
            }
            _ProfilerTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(Settings.ProfilerRefreshInterval)
            };
            _ProfilerTimer.Tick += delegate (object o, EventArgs e)
            {
                RefreshProfiler();
            };
            Settings.PropertyChanged += delegate (object o, PropertyChangedEventArgs e)
            {
                if (e.PropertyName == "EnableProfiler")
                {
                    ProfilerTriggers = new ObservableCollection<Trigger>();
                    base.RaisePropertyChanged("ProfilerTriggers");
                    ProfilerRunning = Settings.EnableProfiler;
                    return;
                }
                if (e.PropertyName == "ProfilerRefreshInterval")
                {
                    _ProfilerTimer.Interval = TimeSpan.FromSeconds(Settings.ProfilerRefreshInterval);
                }
            };
            ProfilerRunning = Settings.EnableProfiler;
        }

        // Token: 0x17000167 RID: 359
        // (get) Token: 0x060003DF RID: 991 RVA: 0x0000D98C File Offset: 0x0000BB8C
        // (set) Token: 0x060003E0 RID: 992 RVA: 0x0000D994 File Offset: 0x0000BB94
        internal MainWindow Window { get; set; }

        // Token: 0x17000168 RID: 360
        // (get) Token: 0x060003E1 RID: 993 RVA: 0x0000D99D File Offset: 0x0000BB9D
        // (set) Token: 0x060003E2 RID: 994 RVA: 0x0000D9AA File Offset: 0x0000BBAA
        public TriggerGroupViewModel RootTriggerGroup
        {
            get => base.Get<TriggerGroupViewModel>("RootTriggerGroup"); set => base.Set("RootTriggerGroup", value);
        }

        // Token: 0x17000169 RID: 361
        // (get) Token: 0x060003E3 RID: 995 RVA: 0x0000D9C8 File Offset: 0x0000BBC8
        public IEnumerable<GINACharacter> SelectedCharacters => (from o in base.App.Data.Characters
                                                                 where o.IsSelected
                                                                 select o.Character).ToList<GINACharacter>();

        // Token: 0x1700016A RID: 362
        // (get) Token: 0x060003E4 RID: 996 RVA: 0x0000DA2E File Offset: 0x0000BC2E
        // (set) Token: 0x060003E5 RID: 997 RVA: 0x0000DA3B File Offset: 0x0000BC3B
        public GINACharacter CharacterToEdit
        {
            get => base.Get<GINACharacter>("CharacterToEdit"); set => base.Set("CharacterToEdit", value);
        }

        // Token: 0x1700016B RID: 363
        // (get) Token: 0x060003E6 RID: 998 RVA: 0x0000DA49 File Offset: 0x0000BC49
        // (set) Token: 0x060003E7 RID: 999 RVA: 0x0000DA56 File Offset: 0x0000BC56
        public bool ShowCharacterEditor
        {
            get => base.Get<bool>("ShowCharacterEditor"); set => base.Set("ShowCharacterEditor", value);
        }

        // Token: 0x1700016C RID: 364
        // (get) Token: 0x060003E8 RID: 1000 RVA: 0x0000DA69 File Offset: 0x0000BC69
        // (set) Token: 0x060003E9 RID: 1001 RVA: 0x0000DA76 File Offset: 0x0000BC76
        public TriggerGroup ParentTriggerGroup
        {
            get => base.Get<TriggerGroup>("ParentTriggerGroup"); set => base.Set("ParentTriggerGroup", value);
        }

        // Token: 0x1700016D RID: 365
        // (get) Token: 0x060003EA RID: 1002 RVA: 0x0000DA84 File Offset: 0x0000BC84
        // (set) Token: 0x060003EB RID: 1003 RVA: 0x0000DA91 File Offset: 0x0000BC91
        public List<TriggerAndGroupViewModel> SelectedViewModels
        {
            get => base.Get<List<TriggerAndGroupViewModel>>("SelectedViewModels"); set => base.Set("SelectedViewModels", value);
        }

        // Token: 0x1700016E RID: 366
        // (get) Token: 0x060003EC RID: 1004 RVA: 0x0000DAC0 File Offset: 0x0000BCC0
        public TriggerGroupViewModel SelectedTriggerGroupViewModel
        {
            get
            {
                if (SelectedViewModels == null)
                {
                    return null;
                }
                var group = SelectedViewModels.FirstOrDefault((TriggerAndGroupViewModel o) => o is TriggerGroupViewModel);
                if (group == null || !SelectedViewModels.All((TriggerAndGroupViewModel o) => o == group))
                {
                    return null;
                }
                return group as TriggerGroupViewModel;
            }
        }

        // Token: 0x1700016F RID: 367
        // (get) Token: 0x060003ED RID: 1005 RVA: 0x0000DB3C File Offset: 0x0000BD3C
        // (set) Token: 0x060003EE RID: 1006 RVA: 0x0000DB5B File Offset: 0x0000BD5B
        public TriggerGroup SelectedTriggerGroup
        {
            get
            {
                var selectedTriggerGroupViewModel = SelectedTriggerGroupViewModel;
                if (selectedTriggerGroupViewModel == null)
                {
                    return null;
                }
                return selectedTriggerGroupViewModel.TriggerGroup;
            }
            set
            {
            }
        }

        // Token: 0x17000170 RID: 368
        // (get) Token: 0x060003EF RID: 1007 RVA: 0x0000DB7C File Offset: 0x0000BD7C
        public Trigger SelectedTrigger
        {
            get
            {
                if (SelectedViewModels == null)
                {
                    return null;
                }
                var trigger = SelectedViewModels.FirstOrDefault((TriggerAndGroupViewModel o) => o is TriggerViewModel);
                if (trigger == null || !SelectedViewModels.All((TriggerAndGroupViewModel o) => o == trigger))
                {
                    return null;
                }
                return (trigger as TriggerViewModel).Trigger;
            }
        }

        // Token: 0x17000171 RID: 369
        // (get) Token: 0x060003F0 RID: 1008 RVA: 0x0000DBFA File Offset: 0x0000BDFA
        // (set) Token: 0x060003F1 RID: 1009 RVA: 0x0000DC07 File Offset: 0x0000BE07
        public TriggerCategory SelectedTriggerCategory
        {
            get => base.Get<TriggerCategory>("SelectedTriggerCategory"); set => base.Set("SelectedTriggerCategory", value);
        }

        // Token: 0x17000172 RID: 370
        // (get) Token: 0x060003F2 RID: 1010 RVA: 0x0000DC15 File Offset: 0x0000BE15
        public IEnumerable<ITriggerLibraryEntry> SelectedEntries => GetEffectiveEntries(SelectedViewModels);

        // Token: 0x17000173 RID: 371
        // (get) Token: 0x060003F3 RID: 1011 RVA: 0x0000DC23 File Offset: 0x0000BE23
        public bool SelectedIsExportable => SelectedViewModels != null && SelectedViewModels.Any<TriggerAndGroupViewModel>();

        // Token: 0x17000174 RID: 372
        // (get) Token: 0x060003F4 RID: 1012 RVA: 0x0000DC3A File Offset: 0x0000BE3A
        // (set) Token: 0x060003F5 RID: 1013 RVA: 0x0000DC47 File Offset: 0x0000BE47
        public List<TriggerAndGroupViewModel> ClipboardedEntries
        {
            get => base.Get<List<TriggerAndGroupViewModel>>("ClipboardedEntries"); set => base.Set("ClipboardedEntries", value);
        }

        // Token: 0x17000175 RID: 373
        // (get) Token: 0x060003F6 RID: 1014 RVA: 0x0000DC60 File Offset: 0x0000BE60
        public bool CanPasteEntries
        {
            get
            {
                if (ClipboardedEntries != null && ClipboardedEntries.Any<TriggerAndGroupViewModel>())
                {
                    if (SelectedTriggerGroupViewModel == null)
                    {
                        if (ClipboardedEntries.Any((TriggerAndGroupViewModel o) => o is TriggerViewModel))
                        {
                            return false;
                        }
                    }
                    return IsMoveAllowed(ClipboardedEntries, SelectedTriggerGroupViewModel);
                }
                return false;
            }
        }

        // Token: 0x17000176 RID: 374
        // (get) Token: 0x060003F7 RID: 1015 RVA: 0x0000DCC8 File Offset: 0x0000BEC8
        // (set) Token: 0x060003F8 RID: 1016 RVA: 0x0000DCD5 File Offset: 0x0000BED5
        public TriggerGroup EditingTriggerGroup
        {
            get => base.Get<TriggerGroup>("EditingTriggerGroup"); set => base.Set("EditingTriggerGroup", value);
        }

        // Token: 0x17000177 RID: 375
        // (get) Token: 0x060003F9 RID: 1017 RVA: 0x0000DCE3 File Offset: 0x0000BEE3
        // (set) Token: 0x060003FA RID: 1018 RVA: 0x0000DCF0 File Offset: 0x0000BEF0
        public Trigger EditingTrigger
        {
            get => base.Get<Trigger>("EditingTrigger"); set => base.Set("EditingTrigger", value);
        }

        // Token: 0x17000178 RID: 376
        // (get) Token: 0x060003FB RID: 1019 RVA: 0x0000DCFE File Offset: 0x0000BEFE
        // (set) Token: 0x060003FC RID: 1020 RVA: 0x0000DD0B File Offset: 0x0000BF0B
        public bool ShowPhoneticDictionaryEditor
        {
            get => base.Get<bool>("ShowPhoneticDictionaryEditor"); set => base.Set("ShowPhoneticDictionaryEditor", value);
        }

        // Token: 0x17000179 RID: 377
        // (get) Token: 0x060003FD RID: 1021 RVA: 0x0000DD1E File Offset: 0x0000BF1E
        // (set) Token: 0x060003FE RID: 1022 RVA: 0x0000DD2B File Offset: 0x0000BF2B
        public bool ShowTriggerEditor
        {
            get => base.Get<bool>("ShowTriggerEditor"); set => base.Set("ShowTriggerEditor", value);
        }

        // Token: 0x1700017A RID: 378
        // (get) Token: 0x060003FF RID: 1023 RVA: 0x0000DD3E File Offset: 0x0000BF3E
        // (set) Token: 0x06000400 RID: 1024 RVA: 0x0000DD4B File Offset: 0x0000BF4B
        public bool ShowTriggerGroupEditor
        {
            get => base.Get<bool>("ShowTriggerGroupEditor"); set => base.Set("ShowTriggerGroupEditor", value);
        }

        // Token: 0x1700017B RID: 379
        // (get) Token: 0x06000401 RID: 1025 RVA: 0x0000DD5E File Offset: 0x0000BF5E
        // (set) Token: 0x06000402 RID: 1026 RVA: 0x0000DD6B File Offset: 0x0000BF6B
        public bool ShowPreferencesEditor
        {
            get => base.Get<bool>("ShowPreferencesEditor"); set => base.Set("ShowPreferencesEditor", value);
        }

        // Token: 0x1700017C RID: 380
        // (get) Token: 0x06000403 RID: 1027 RVA: 0x0000DD7E File Offset: 0x0000BF7E
        // (set) Token: 0x06000404 RID: 1028 RVA: 0x0000DD8B File Offset: 0x0000BF8B
        public bool ShowCategoriesEditor
        {
            get => base.Get<bool>("ShowCategoriesEditor"); set => base.Set("ShowCategoriesEditor", value);
        }

        // Token: 0x1700017D RID: 381
        // (get) Token: 0x06000405 RID: 1029 RVA: 0x0000DD9E File Offset: 0x0000BF9E
        // (set) Token: 0x06000406 RID: 1030 RVA: 0x0000DDAB File Offset: 0x0000BFAB
        public bool ShowDeleteCharacterConfirm
        {
            get => base.Get<bool>("ShowDeleteCharacterConfirm"); set => base.Set("ShowDeleteCharacterConfirm", value);
        }

        // Token: 0x1700017E RID: 382
        // (get) Token: 0x06000407 RID: 1031 RVA: 0x0000DDBE File Offset: 0x0000BFBE
        // (set) Token: 0x06000408 RID: 1032 RVA: 0x0000DDCB File Offset: 0x0000BFCB
        public bool ShowDeleteTriggerGroupConfirm
        {
            get => base.Get<bool>("ShowDeleteTriggerGroupConfirm"); set => base.Set("ShowDeleteTriggerGroupConfirm", value);
        }

        // Token: 0x1700017F RID: 383
        // (get) Token: 0x06000409 RID: 1033 RVA: 0x0000DDDE File Offset: 0x0000BFDE
        // (set) Token: 0x0600040A RID: 1034 RVA: 0x0000DDEB File Offset: 0x0000BFEB
        public bool ShowDeleteTriggerConfirm
        {
            get => base.Get<bool>("ShowDeleteTriggerConfirm"); set => base.Set("ShowDeleteTriggerConfirm", value);
        }

        // Token: 0x17000180 RID: 384
        // (get) Token: 0x0600040B RID: 1035 RVA: 0x0000DDFE File Offset: 0x0000BFFE
        // (set) Token: 0x0600040C RID: 1036 RVA: 0x0000DE0B File Offset: 0x0000C00B
        public bool ShowDeleteTriggerCategoryConfirm
        {
            get => base.Get<bool>("ShowDeleteTriggerCategoryConfirm"); set => base.Set("ShowDeleteTriggerCategoryConfirm", value);
        }

        // Token: 0x17000181 RID: 385
        // (get) Token: 0x0600040D RID: 1037 RVA: 0x0000DE1E File Offset: 0x0000C01E
        public Configuration Settings => Configuration.Current;

        // Token: 0x17000182 RID: 386
        // (get) Token: 0x0600040E RID: 1038 RVA: 0x0000DE25 File Offset: 0x0000C025
        public ObservableCollection<ShareDetectedEventArgs> SharesDetected => Package.SharesDetected;

        // Token: 0x17000183 RID: 387
        // (get) Token: 0x0600040F RID: 1039 RVA: 0x0000DE2C File Offset: 0x0000C02C
        // (set) Token: 0x06000410 RID: 1040 RVA: 0x0000DE39 File Offset: 0x0000C039
        public bool ShowUploadDialog
        {
            get => base.Get<bool>("ShowUploadDialog"); set => base.Set("ShowUploadDialog", value);
        }

        // Token: 0x17000184 RID: 388
        // (get) Token: 0x06000411 RID: 1041 RVA: 0x0000DE4C File Offset: 0x0000C04C
        // (set) Token: 0x06000412 RID: 1042 RVA: 0x0000DE59 File Offset: 0x0000C059
        public bool ShowAppUpdateDialog
        {
            get => base.Get<bool>("ShowAppUpdateDialog"); set => base.Set("ShowAppUpdateDialog", value);
        }

        // Token: 0x17000185 RID: 389
        // (get) Token: 0x06000413 RID: 1043 RVA: 0x0000DE6C File Offset: 0x0000C06C
        // (set) Token: 0x06000414 RID: 1044 RVA: 0x0000DE79 File Offset: 0x0000C079
        public bool UseAutoUpdateMode
        {
            get => base.Get<bool>("UseAutoUpdateMode"); set => base.Set("UseAutoUpdateMode", value);
        }

        // Token: 0x17000186 RID: 390
        // (get) Token: 0x06000415 RID: 1045 RVA: 0x0000DE8C File Offset: 0x0000C08C
        // (set) Token: 0x06000416 RID: 1046 RVA: 0x0000DE99 File Offset: 0x0000C099
        public bool ShowEQImportDialog
        {
            get => base.Get<bool>("ShowEQImportDialog"); set => base.Set("ShowEQImportDialog", value);
        }

        // Token: 0x17000187 RID: 391
        // (get) Token: 0x06000417 RID: 1047 RVA: 0x0000DEAC File Offset: 0x0000C0AC
        // (set) Token: 0x06000418 RID: 1048 RVA: 0x0000DEB9 File Offset: 0x0000C0B9
        public bool ShowEQExportDialog
        {
            get => base.Get<bool>("ShowEQExportDialog"); set => base.Set("ShowEQExportDialog", value);
        }

        // Token: 0x17000188 RID: 392
        // (get) Token: 0x06000419 RID: 1049 RVA: 0x0000DECC File Offset: 0x0000C0CC
        // (set) Token: 0x0600041A RID: 1050 RVA: 0x0000DED9 File Offset: 0x0000C0D9
        public bool ShowAboutDialog
        {
            get => base.Get<bool>("ShowAboutDialog"); set => base.Set("ShowAboutDialog", value);
        }

        // Token: 0x17000189 RID: 393
        // (get) Token: 0x0600041B RID: 1051 RVA: 0x0000DEEC File Offset: 0x0000C0EC
        // (set) Token: 0x0600041C RID: 1052 RVA: 0x0000DEF9 File Offset: 0x0000C0F9
        public bool ShowLogSearchDialog
        {
            get => base.Get<bool>("ShowLogSearchDialog"); set => base.Set("ShowLogSearchDialog", value);
        }

        // Token: 0x1700018A RID: 394
        // (get) Token: 0x0600041D RID: 1053 RVA: 0x0000DF0C File Offset: 0x0000C10C
        // (set) Token: 0x0600041E RID: 1054 RVA: 0x0000DF19 File Offset: 0x0000C119
        public bool ShowTriggerSearchDialog
        {
            get => base.Get<bool>("ShowTriggerSearchDialog"); set => base.Set("ShowTriggerSearchDialog", value);
        }

        // Token: 0x1700018B RID: 395
        // (get) Token: 0x0600041F RID: 1055 RVA: 0x0000DF2C File Offset: 0x0000C12C
        // (set) Token: 0x06000420 RID: 1056 RVA: 0x0000DF39 File Offset: 0x0000C139
        public bool ShowErrorUploadDialog
        {
            get => base.Get<bool>("ShowErrorUploadDialog"); set => base.Set("ShowErrorUploadDialog", value);
        }

        // Token: 0x1700018C RID: 396
        // (get) Token: 0x06000421 RID: 1057 RVA: 0x0000DF4C File Offset: 0x0000C14C
        // (set) Token: 0x06000422 RID: 1058 RVA: 0x0000DF59 File Offset: 0x0000C159
        public bool IsRepositoryUpload
        {
            get => base.Get<bool>("IsRepositoryUpload"); set => base.Set("IsRepositoryUpload", value);
        }

        // Token: 0x1700018D RID: 397
        // (get) Token: 0x06000423 RID: 1059 RVA: 0x0000DF6C File Offset: 0x0000C16C
        // (set) Token: 0x06000424 RID: 1060 RVA: 0x0000DF79 File Offset: 0x0000C179
        public bool UploadToClipboard
        {
            get => base.Get<bool>("UploadToClipboard"); set => base.Set("UploadToClipboard", value);
        }

        // Token: 0x1700018E RID: 398
        // (get) Token: 0x06000425 RID: 1061 RVA: 0x0000DF8C File Offset: 0x0000C18C
        public string AppVersion => Application.ProductVersion;

        // Token: 0x1700018F RID: 399
        // (get) Token: 0x06000426 RID: 1062 RVA: 0x0000DF93 File Offset: 0x0000C193
        public ObservableCollection<Trigger> ProfilerTriggers { get; private set; } = new ObservableCollection<Trigger>();

        // Token: 0x17000190 RID: 400
        // (get) Token: 0x06000427 RID: 1063 RVA: 0x0000DF9B File Offset: 0x0000C19B
        // (set) Token: 0x06000428 RID: 1064 RVA: 0x0000DFA8 File Offset: 0x0000C1A8
        public bool ProfilerRunning
        {
            get => _ProfilerTimer.IsEnabled;
            set
            {
                _ProfilerTimer.IsEnabled = Settings.EnableProfiler && value;
                base.RaisePropertyChanged("ProfilerRunning");
                if (ProfilerRunning)
                {
                    RefreshProfiler();
                }
            }
        }

        // Token: 0x17000191 RID: 401
        // (get) Token: 0x06000429 RID: 1065 RVA: 0x0000DFE0 File Offset: 0x0000C1E0
        public List<Tuple<string, int>> RefreshIntervals
        {
            get
            {
                if (_RefreshIntervals == null)
                {
                    _RefreshIntervals = new List<Tuple<string, int>>();
                    for (var i = 5; i <= 60; i += 5)
                    {
                        _RefreshIntervals.Add(new Tuple<string, int>(i.ToString() + " seconds", i));
                    }
                }
                return _RefreshIntervals;
            }
        }

        // Token: 0x17000192 RID: 402
        // (get) Token: 0x0600042A RID: 1066 RVA: 0x0000E035 File Offset: 0x0000C235
        // (set) Token: 0x0600042B RID: 1067 RVA: 0x0000E03D File Offset: 0x0000C23D
        public List<MainWindowViewModel.ProcessorItem> AvailableProcessors { get; set; }

        // Token: 0x17000193 RID: 403
        // (get) Token: 0x0600042C RID: 1068 RVA: 0x0000E050 File Offset: 0x0000C250
        public double LinesPerSecond
        {
            get
            {
                var totalSeconds = (DateTime.Now - _ProfilerStartTime).TotalSeconds;
                var num = GINACharacter.All.Sum((GINACharacter o) => o.ProfiledLines);
                if (totalSeconds != 0.0)
                {
                    return Math.Round((double)(num * 1f) / totalSeconds, 2);
                }
                return 0.0;
            }
        }

        // Token: 0x17000194 RID: 404
        // (get) Token: 0x0600042D RID: 1069 RVA: 0x0000E0C5 File Offset: 0x0000C2C5
        public long MemoryUsed => Process.GetCurrentProcess().PrivateMemorySize64;

        // Token: 0x0600042E RID: 1070 RVA: 0x0000E0F8 File Offset: 0x0000C2F8
        private string GetNewTriggerGroupName(IEnumerable<TriggerGroup> groups)
        {
            var str = "New Trigger Group";
            var num = 1;
            while (groups.Any((TriggerGroup o) => o.Name.ToLower() == str.ToLower()))
            {
                str = "New Trigger Group " + num++.ToString();
            }
            return str;
        }

        // Token: 0x0600042F RID: 1071 RVA: 0x0000E154 File Offset: 0x0000C354
        private bool EnsureEverquestFolder()
        {
            if (!File.Exists(Path.Combine(Settings.EverquestFolder, Configuration.EverquestGameFile)))
            {
                var folderBrowserDialog = new FolderBrowserDialog
                {
                    ShowNewFolderButton = false,
                    Description = "Please select the folder in which Everquest is installed."
                };
                var dialogResult = folderBrowserDialog.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    while (dialogResult == DialogResult.OK && !File.Exists(Path.Combine(folderBrowserDialog.SelectedPath, Configuration.EverquestGameFile)))
                    {
                        dialogResult = folderBrowserDialog.ShowDialog();
                    }
                    if (dialogResult == DialogResult.OK)
                    {
                        Settings.EverquestFolder = folderBrowserDialog.SelectedPath;
                        Configuration.SaveConfiguration(false);
                    }
                }
                return dialogResult == DialogResult.OK;
            }
            return true;
        }

        // Token: 0x06000430 RID: 1072 RVA: 0x0000E1E5 File Offset: 0x0000C3E5
        private void CheckForUpdates(bool autoCheck)
        {
            UseAutoUpdateMode = autoCheck;
            if (UseAutoUpdateMode && !Settings.AutoUpdate)
            {
                return;
            }
            ShowAppUpdateDialog = true;
        }

        // Token: 0x06000431 RID: 1073 RVA: 0x0000E28C File Offset: 0x0000C48C
        private IEnumerable<TriggerAndGroupViewModel> GetEffectiveSelection(IEnumerable<TriggerAndGroupViewModel> selectedItems)
        {
            if (selectedItems == null)
            {
                return null;
            }
            var list = selectedItems.ToList<TriggerAndGroupViewModel>();
            var toRemove = new List<TriggerAndGroupViewModel>();
            using (var enumerator = selectedItems.Where((TriggerAndGroupViewModel o) => o is TriggerGroupViewModel).Cast<TriggerGroupViewModel>().GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    var grp = enumerator.Current;
                    toRemove.AddRange(selectedItems.Where((TriggerAndGroupViewModel o) => o != grp && grp.DescendantTree.Contains(o)).Distinct<TriggerAndGroupViewModel>());
                }
            }
            _ = list.RemoveAll((TriggerAndGroupViewModel o) => toRemove.Contains(o));
            var effGroups = list.Where((TriggerAndGroupViewModel o) => o is TriggerGroupViewModel).ToList<TriggerAndGroupViewModel>();
            _ = list.RemoveAll((TriggerAndGroupViewModel o) => o is TriggerViewModel && ((TriggerViewModel)o).GroupVM != null && effGroups.Contains(((TriggerViewModel)o).GroupVM));
            return list;
        }

        // Token: 0x06000432 RID: 1074 RVA: 0x0000E3CC File Offset: 0x0000C5CC
        private IEnumerable<ITriggerLibraryEntry> GetEffectiveEntries(IEnumerable<TriggerAndGroupViewModel> selectedItems)
        {
            if (selectedItems == null)
            {
                return null;
            }
            var effectiveSelection = GetEffectiveSelection(selectedItems);
            return effectiveSelection.Select<TriggerAndGroupViewModel, ITriggerLibraryEntry>(o =>
            {
                if (o is TriggerGroupViewModel groupViewModel)
                {
                    return groupViewModel.TriggerGroup;
                }
                else if (o is TriggerViewModel triggerViewModel)
                {
                    return triggerViewModel.Trigger;
                }
                return null;
            });
        }

        // Token: 0x06000433 RID: 1075 RVA: 0x0000E444 File Offset: 0x0000C644
        private IEnumerable<Trigger> GetEffectiveTriggers(IEnumerable<TriggerAndGroupViewModel> selectedItems)
        {
            if (selectedItems == null)
            {
                return null;
            }
            var effectiveEntries = GetEffectiveEntries(selectedItems);
            return effectiveEntries.Where((ITriggerLibraryEntry o) => o is TriggerGroup).SelectMany((ITriggerLibraryEntry o) => ((TriggerGroup)o).DescendantTree).SelectMany((TriggerGroup o) => o.Triggers)
                .Union(from o in effectiveEntries
                       where o is Trigger
                       select o as Trigger)
                .Distinct<Trigger>()
                .ToList<Trigger>();
        }

        // Token: 0x06000434 RID: 1076 RVA: 0x0000E55C File Offset: 0x0000C75C
        private bool IsMoveAllowed(IEnumerable<ITreeItem> draggedItems, ITreeItem dropItem)
        {
            if (draggedItems == null || !draggedItems.Any<ITreeItem>())
            {
                return false;
            }
            var effectiveSelection = GetEffectiveSelection(draggedItems.Cast<TriggerAndGroupViewModel>());
            if (dropItem == null)
            {
                if (effectiveSelection.Any((TriggerAndGroupViewModel o) => o is TriggerViewModel))
                {
                    return false;
                }
                return !effectiveSelection.Where((TriggerAndGroupViewModel o) => o is TriggerGroupViewModel).Any((TriggerAndGroupViewModel o) => ((TriggerGroupViewModel)o).TriggerGroup.ParentGroup == null);
            }
            else
            {
                if (dropItem is TriggerGroupViewModel)
                {
                    return !effectiveSelection.Where((TriggerAndGroupViewModel o) => o is TriggerGroupViewModel).SelectMany((TriggerAndGroupViewModel o) => (o as TriggerGroupViewModel).DescendantTree).Contains(dropItem);
                }
                return false;
            }
        }

        // Token: 0x06000435 RID: 1077 RVA: 0x0000E654 File Offset: 0x0000C854
        private void MoveEntries(IEnumerable<ITreeItem> draggedItems, ITreeItem dropItem)
        {
            if (!IsMoveAllowed(draggedItems, dropItem))
            {
                return;
            }
            var effectiveSelection = GetEffectiveSelection(draggedItems.Cast<TriggerAndGroupViewModel>());
            foreach (var triggerAndGroupViewModel in effectiveSelection)
            {
                var triggerGroupViewModel = dropItem as TriggerGroupViewModel;
                var triggerGroup = triggerGroupViewModel?.TriggerGroup;
                if (triggerAndGroupViewModel is TriggerGroupViewModel)
                {
                    var triggerGroup2 = ((TriggerGroupViewModel)triggerAndGroupViewModel).TriggerGroup;
                    triggerGroup2.Move(triggerGroup ?? TriggerGroup.RootGroup, -1);
                }
                else if (triggerAndGroupViewModel is TriggerViewModel)
                {
                    var trigger = ((TriggerViewModel)triggerAndGroupViewModel).Trigger;
                    if (triggerGroup != null)
                    {
                        trigger.Move(triggerGroup, null);
                    }
                }
            }
            Configuration.SaveConfiguration(false);
        }

        // Token: 0x06000436 RID: 1078 RVA: 0x0000E9AC File Offset: 0x0000CBAC
        private void RefreshProfiler()
        {
            _ = base.Dispatcher.Invoke(DispatcherPriority.Normal, new ThreadStart(delegate
            {
                var flag = false;
                try
                {
                    ObservableCollection<Trigger> profilerTriggers;
                    Monitor.Enter(profilerTriggers = ProfilerTriggers, ref flag);
                    var triggers = (from m in GINACharacter.All.Where((GINACharacter m) => m.IsEnabled).SelectMany((GINACharacter m) => m.TriggerGroups.SelectMany((TriggerGroup n) => n.Triggers)).Distinct<Trigger>()
                                    orderby m.AverageMatchCost descending
                                    select m).ToList<Trigger>();
                    var list = triggers.Where((Trigger n) => !ProfilerTriggers.Contains(n)).ToList<Trigger>();
                    var list2 = ProfilerTriggers.Where((Trigger n) => !triggers.Contains(n)).ToList<Trigger>();
                    foreach (var trigger in list)
                    {
                        ProfilerTriggers.Add(trigger);
                    }
                    foreach (var trigger2 in list2)
                    {
                        _ = ProfilerTriggers.Remove(trigger2);
                    }
                    var sortDescription = Window.ProfilerGrid.Items.SortDescriptions.FirstOrDefault<SortDescription>();
                    Window.ProfilerGrid.Items.Refresh();
                    if (sortDescription.PropertyName != null)
                    {
                        Window.ProfilerGrid.Items.SortDescriptions.Add(sortDescription);
                    }
                    base.RaisePropertyChanged("LinesPerSecond");
                    base.RaisePropertyChanged("MemoryUsed");
                }
                finally
                {
                }
            }));
        }

        // Token: 0x17000195 RID: 405
        // (get) Token: 0x06000437 RID: 1079 RVA: 0x0000EA33 File Offset: 0x0000CC33
        public GenericCommand MainWindowLoadedCommand => new GenericCommand(delegate (object p)
                                                                      {
                                                                          var configurationLoadResult = Configuration.LoadConfiguration();
                                                                          if (configurationLoadResult == ConfigurationLoadResult.UsedLastKnownGood)
                                                                          {
                                                                              base.App.Data.ShowError("Error", "Could not load configuration file.  Using last successfully loaded configuration file.");
                                                                              return;
                                                                          }
                                                                          if (configurationLoadResult == ConfigurationLoadResult.FailedToDefault)
                                                                          {
                                                                              base.App.Data.ShowError("Error", "Could not load configuration file.  Using default settings.");
                                                                              return;
                                                                          }
                                                                          if (File.Exists(Configuration.CrashLogFilePre))
                                                                          {
                                                                              ShowErrorUploadDialog = true;
                                                                          }
                                                                          CheckForUpdates(true);
                                                                      });

        // Token: 0x17000196 RID: 406
        // (get) Token: 0x06000438 RID: 1080 RVA: 0x0000EA4F File Offset: 0x0000CC4F
        public GenericCommand EditPhoneticDictionaryCommand => new GenericCommand(delegate (object p)
                                                                            {
                                                                                ShowPhoneticDictionaryEditor = true;
                                                                            });

        // Token: 0x17000197 RID: 407
        // (get) Token: 0x06000439 RID: 1081 RVA: 0x0000EA6C File Offset: 0x0000CC6C
        public GenericCommand EditPreferencesCommand => new GenericCommand
        {
            Execute = delegate (object p)
            {
                ShowPreferencesEditor = true;
            }
        };

        // Token: 0x17000198 RID: 408
        // (get) Token: 0x0600043A RID: 1082 RVA: 0x0000EAC0 File Offset: 0x0000CCC0
        public GenericCommand CheckForUpdatesCommand => new GenericCommand(delegate (object p)
                                                                     {
                                                                         var flag = false;
                                                                         if (p is string)
                                                                         {
                                                                             _ = bool.TryParse(p as string, out flag);
                                                                         }
                                                                         CheckForUpdates(flag);
                                                                     });

        // Token: 0x17000199 RID: 409
        // (get) Token: 0x0600043B RID: 1083 RVA: 0x0000EAE4 File Offset: 0x0000CCE4
        public GenericCommand ToggleAboutDialogCommand => new GenericCommand(delegate (object p)
                                                                       {
                                                                           ShowAboutDialog = !ShowAboutDialog;
                                                                       });

        // Token: 0x1700019A RID: 410
        // (get) Token: 0x0600043C RID: 1084 RVA: 0x0000EB00 File Offset: 0x0000CD00
        public GenericCommand SearchLogCommand => new GenericCommand(delegate (object p)
                                                               {
                                                                   ShowLogSearchDialog = true;
                                                               });

        // Token: 0x1700019B RID: 411
        // (get) Token: 0x0600043D RID: 1085 RVA: 0x0000EB1C File Offset: 0x0000CD1C
        public GenericCommand SearchTriggersCommand => new GenericCommand(delegate (object p)
                                                                    {
                                                                        ShowTriggerSearchDialog = true;
                                                                    });

        // Token: 0x1700019C RID: 412
        // (get) Token: 0x0600043E RID: 1086 RVA: 0x0000EB40 File Offset: 0x0000CD40
        public GenericCommand AddCharacterCommand => new GenericCommand
        {
            Execute = delegate (object p)
            {
                CharacterToEdit = null;
                ShowCharacterEditor = true;
            }
        };

        // Token: 0x1700019D RID: 413
        // (get) Token: 0x0600043F RID: 1087 RVA: 0x0000EBE8 File Offset: 0x0000CDE8
        public GenericCommand AddTriggerGroupCommand => new GenericCommand
        {
            Execute = delegate (object p)
            {
                TriggerGroup triggerGroup = null;
                if (p == null)
                {
                    return;
                }
                if (p is string text)
                {
                    if (!(text == "selected"))
                    {
                        if (!(text == "root"))
                        {
                            if (text == "either")
                            {
                                triggerGroup = SelectedTriggerGroup ?? TriggerGroup.RootGroup;
                            }
                        }
                        else
                        {
                            triggerGroup = TriggerGroup.RootGroup;
                        }
                    }
                    else
                    {
                        triggerGroup = SelectedTriggerGroup;
                    }
                }
                if (triggerGroup == null)
                {
                    return;
                }
                EditingTriggerGroup = null;
                ParentTriggerGroup = triggerGroup;
                ShowTriggerGroupEditor = true;
            }
        };

        // Token: 0x1700019E RID: 414
        // (get) Token: 0x06000440 RID: 1088 RVA: 0x0000EC28 File Offset: 0x0000CE28
        public GenericCommand AddTriggerCommand => new GenericCommand
        {
            Execute = delegate (object p)
            {
                if (SelectedTriggerGroup == null)
                {
                    return;
                }
                EditingTrigger = null;
                ShowTriggerEditor = true;
            }
        };

        // Token: 0x1700019F RID: 415
        // (get) Token: 0x06000441 RID: 1089 RVA: 0x0000EDA7 File Offset: 0x0000CFA7
        public GenericCommand AddBehaviorGroupCommand => new GenericCommand(delegate (object p)
                                                                      {
                                                                          if (p is string && (p as string) == "timer")
                                                                          {
                                                                              var group2 = BehaviorGroup.Create(BehaviorTypes.Timer);
                                                                              var timerWindow = (from o in base.App.Data.TimerBehaviorWindows
                                                                                                 where o.Key == group2
                                                                                                 select o.Value).SingleOrDefault<TimerWindow>();
                                                                              if (timerWindow != null)
                                                                              {
                                                                                  (timerWindow.DataContext as TimerWindowViewModel).ShowOpaqueWindow = true;
                                                                              }
                                                                          }
                                                                          else
                                                                          {
                                                                              var group = BehaviorGroup.Create(BehaviorTypes.Text);
                                                                              var textWindow = (from o in base.App.Data.TextBehaviorWindows
                                                                                                where o.Key == @group
                                                                                                select o.Value).SingleOrDefault<TextWindow>();
                                                                              if (textWindow != null)
                                                                              {
                                                                                  (textWindow.DataContext as TextWindowViewModel).ShowOpaqueWindow = true;
                                                                              }
                                                                          }
                                                                          Configuration.SaveConfiguration(false);
                                                                      });

        // Token: 0x170001A0 RID: 416
        // (get) Token: 0x06000442 RID: 1090 RVA: 0x0000EDCB File Offset: 0x0000CFCB
        public GenericCommand AddTriggerCategoryCommand => new GenericCommand(delegate (object p)
                                                                        {
                                                                            SelectedTriggerCategory = TriggerCategory.Create(null, null, null, false);
                                                                        });

        // Token: 0x170001A1 RID: 417
        // (get) Token: 0x06000443 RID: 1091 RVA: 0x0000EE10 File Offset: 0x0000D010
        public GenericCommand EditCharacterCommand => new GenericCommand
        {
            Execute = delegate (object p)
            {
                if (SelectedCharacters.Count<GINACharacter>() != 1)
                {
                    return;
                }
                CharacterToEdit = SelectedCharacters.ToList<GINACharacter>()[0];
                ShowCharacterEditor = true;
            }
        };

        // Token: 0x170001A2 RID: 418
        // (get) Token: 0x06000444 RID: 1092 RVA: 0x0000EE90 File Offset: 0x0000D090
        public GenericCommand EditTriggerOrGroupCommand => new GenericCommand
        {
            Execute = delegate (object p)
            {
                if (p is MouseButtonEventArgs && (p as MouseButtonEventArgs).ChangedButton != MouseButton.Left)
                {
                    return;
                }
                if (SelectedTrigger != null)
                {
                    EditTriggerCommand.Execute(null);
                    return;
                }
                if (SelectedTriggerGroup != null)
                {
                    EditTriggerGroupCommand.Execute(null);
                }
            }
        };

        // Token: 0x170001A3 RID: 419
        // (get) Token: 0x06000445 RID: 1093 RVA: 0x0000EED4 File Offset: 0x0000D0D4
        public GenericCommand EditTriggerGroupCommand => new GenericCommand
        {
            Execute = delegate (object p)
            {
                ParentTriggerGroup = null;
                EditingTriggerGroup = SelectedTriggerGroup;
                ShowTriggerGroupEditor = true;
            }
        };

        // Token: 0x170001A4 RID: 420
        // (get) Token: 0x06000446 RID: 1094 RVA: 0x0000EF24 File Offset: 0x0000D124
        public GenericCommand EditTriggerCommand => new GenericCommand
        {
            Execute = delegate (object p)
            {
                EditingTrigger = (p as Trigger) ?? SelectedTrigger;
                if (EditingTrigger != null)
                {
                    ShowTriggerEditor = true;
                }
            }
        };

        // Token: 0x170001A5 RID: 421
        // (get) Token: 0x06000447 RID: 1095 RVA: 0x0000EFD9 File Offset: 0x0000D1D9
        public GenericCommand ToggleEditBehaviorGroupCommand => new GenericCommand(delegate (object p)
                                                                             {
                                                                                 var behaviorGroup = p as BehaviorGroup;
                                                                                 switch (behaviorGroup.BehaviorType)
                                                                                 {
                                                                                     case BehaviorTypes.Text:
                                                                                         {
                                                                                             var textWindowViewModel = base.App.Data.TextBehaviorWindows[behaviorGroup].DataContext as TextWindowViewModel;
                                                                                             textWindowViewModel.ShowOpaqueWindow = !textWindowViewModel.ShowOpaqueWindow;
                                                                                             return;
                                                                                         }
                                                                                     case BehaviorTypes.Timer:
                                                                                         {
                                                                                             var timerWindowViewModel = base.App.Data.TimerBehaviorWindows[behaviorGroup].DataContext as TimerWindowViewModel;
                                                                                             timerWindowViewModel.ShowOpaqueWindow = !timerWindowViewModel.ShowOpaqueWindow;
                                                                                             return;
                                                                                         }
                                                                                     default:
                                                                                         return;
                                                                                 }
                                                                             });

        // Token: 0x170001A6 RID: 422
        // (get) Token: 0x06000448 RID: 1096 RVA: 0x0000F067 File Offset: 0x0000D267
        public GenericCommand SaveBehaviorGroupCommand => new GenericCommand(delegate (object p)
                                                                       {
                                                                           var behaviorGroup = p as BehaviorGroup;
                                                                           switch (behaviorGroup.BehaviorType)
                                                                           {
                                                                               case BehaviorTypes.Text:
                                                                                   {
                                                                                       var textWindowViewModel = base.App.Data.TextBehaviorWindows[behaviorGroup].DataContext as TextWindowViewModel;
                                                                                       textWindowViewModel.SaveWindow();
                                                                                       return;
                                                                                   }
                                                                               case BehaviorTypes.Timer:
                                                                                   {
                                                                                       var timerWindowViewModel = base.App.Data.TimerBehaviorWindows[behaviorGroup].DataContext as TimerWindowViewModel;
                                                                                       timerWindowViewModel.SaveWindow();
                                                                                       return;
                                                                                   }
                                                                               default:
                                                                                   return;
                                                                           }
                                                                       });

        // Token: 0x170001A7 RID: 423
        // (get) Token: 0x06000449 RID: 1097 RVA: 0x0000F083 File Offset: 0x0000D283
        public GenericCommand EditCategoriesCommand => new GenericCommand(delegate (object p)
                                                                    {
                                                                        ShowCategoriesEditor = true;
                                                                    });

        // Token: 0x170001A8 RID: 424
        // (get) Token: 0x0600044A RID: 1098 RVA: 0x0000F0AC File Offset: 0x0000D2AC
        public GenericCommand SetTriggerCategoryAsDefaultCommand => new GenericCommand(delegate (object p)
                                                                                 {
                                                                                     if (SelectedTriggerCategory != null)
                                                                                     {
                                                                                         SelectedTriggerCategory.IsDefault = true;
                                                                                     }
                                                                                 });

        // Token: 0x170001A9 RID: 425
        // (get) Token: 0x0600044B RID: 1099 RVA: 0x0000F0C8 File Offset: 0x0000D2C8
        public GenericCommand ConfirmDeleteCharactersCommand => new GenericCommand(delegate (object p)
                                                                             {
                                                                                 ShowDeleteCharacterConfirm = true;
                                                                             });

        // Token: 0x170001AA RID: 426
        // (get) Token: 0x0600044C RID: 1100 RVA: 0x0000F148 File Offset: 0x0000D348
        public GenericCommand DeleteCharactersCommand => new GenericCommand
        {
            Execute = delegate (object p)
            {
                foreach (var ginacharacter in SelectedCharacters.ToList<GINACharacter>())
                {
                    _ = GINACharacter.All.Remove(ginacharacter);
                }
                Configuration.SaveConfiguration(false);
                ShowDeleteCharacterConfirm = false;
            }
        };

        // Token: 0x170001AB RID: 427
        // (get) Token: 0x0600044D RID: 1101 RVA: 0x0000F177 File Offset: 0x0000D377
        public GenericCommand ConfirmDeleteTriggerGroupCommand => new GenericCommand(delegate (object p)
                                                                               {
                                                                                   ShowDeleteTriggerGroupConfirm = true;
                                                                               });

        // Token: 0x170001AC RID: 428
        // (get) Token: 0x0600044E RID: 1102 RVA: 0x0000F1B0 File Offset: 0x0000D3B0
        public GenericCommand DeleteTriggerGroupCommand => new GenericCommand
        {
            Execute = delegate (object p)
            {
                if (SelectedTriggerGroup == null)
                {
                    return;
                }
                SelectedTriggerGroup.Remove();
                Configuration.SaveConfiguration(false);
                ShowDeleteTriggerGroupConfirm = false;
            }
        };

        // Token: 0x170001AD RID: 429
        // (get) Token: 0x0600044F RID: 1103 RVA: 0x0000F1DF File Offset: 0x0000D3DF
        public GenericCommand ConfirmDeleteTriggerCommand => new GenericCommand(delegate (object p)
                                                                          {
                                                                              ShowDeleteTriggerConfirm = true;
                                                                          });

        // Token: 0x170001AE RID: 430
        // (get) Token: 0x06000450 RID: 1104 RVA: 0x0000F218 File Offset: 0x0000D418
        public GenericCommand DeleteTriggerCommand => new GenericCommand
        {
            Execute = delegate (object p)
            {
                if (SelectedTrigger == null)
                {
                    return;
                }
                SelectedTrigger.Remove();
                Configuration.SaveConfiguration(false);
                ShowDeleteTriggerConfirm = false;
            }
        };

        // Token: 0x170001AF RID: 431
        // (get) Token: 0x06000451 RID: 1105 RVA: 0x0000F25C File Offset: 0x0000D45C
        public GenericCommand CancelDeleteCommand => new GenericCommand(delegate (object p)
                                                                  {
                                                                      ShowDeleteCharacterConfirm = false;
                                                                      ShowDeleteTriggerGroupConfirm = false;
                                                                      ShowDeleteTriggerConfirm = false;
                                                                      ShowDeleteTriggerCategoryConfirm = false;
                                                                  });

        // Token: 0x170001B0 RID: 432
        // (get) Token: 0x06000452 RID: 1106 RVA: 0x0000F450 File Offset: 0x0000D650
        public GenericCommand RemoveBehaviorGroupCommand => new GenericCommand(delegate (object p)
                                                                         {
                                                                             var behavior = p as BehaviorGroup;
                                                                             if (BehaviorGroup.All.Count((BehaviorGroup o) => o.BehaviorType == behavior.BehaviorType) <= 1)
                                                                             {
                                                                                 return;
                                                                             }
                                                                             var behaviorGroup = (behavior.BehaviorType == BehaviorTypes.Text) ? BehaviorGroup.DefaultTextGroup : BehaviorGroup.DefaultTimerGroup;
                                                                             if (behaviorGroup == behavior)
                                                                             {
                                                                                 behaviorGroup = BehaviorGroup.All.First((BehaviorGroup o) => o.BehaviorType == behavior.BehaviorType && o != behavior);
                                                                             }
                                                                             switch (behavior.BehaviorType)
                                                                             {
                                                                                 case BehaviorTypes.Text:
                                                                                     {
                                                                                         using (var enumerator = TriggerCategory.All.Where((TriggerCategory o) => o.TextOverlay == behavior).ToList<TriggerCategory>().GetEnumerator())
                                                                                         {
                                                                                             while (enumerator.MoveNext())
                                                                                             {
                                                                                                 var triggerCategory = enumerator.Current;
                                                                                                 triggerCategory.TextOverlay = behaviorGroup;
                                                                                             }
                                                                                             goto IL_013D;
                                                                                         }
                                                                                         break;
                                                                                     }
                                                                                 case BehaviorTypes.Timer:
                                                                                     break;
                                                                                 default:
                                                                                     goto IL_013D;
                                                                             }
                                                                             foreach (var triggerCategory2 in TriggerCategory.All.Where((TriggerCategory o) => o.TimerOverlay == behavior).ToList<TriggerCategory>())
                                                                             {
                                                                                 triggerCategory2.TimerOverlay = behaviorGroup;
                                                                             }
                                                                         IL_013D:
                                                                             _ = BehaviorGroup.All.Remove(behavior);
                                                                             Configuration.SaveConfiguration(false);
                                                                         });

        // Token: 0x170001B1 RID: 433
        // (get) Token: 0x06000453 RID: 1107 RVA: 0x0000F47D File Offset: 0x0000D67D
        public GenericCommand ConfirmDeleteTriggerCategoryCommand => new GenericCommand(delegate (object p)
                                                                                  {
                                                                                      ShowDeleteTriggerCategoryConfirm = true;
                                                                                  });

        // Token: 0x170001B2 RID: 434
        // (get) Token: 0x06000454 RID: 1108 RVA: 0x0000F4BF File Offset: 0x0000D6BF
        public GenericCommand DeleteTriggerCategoryCommand => new GenericCommand(delegate (object p)
                                                                           {
                                                                               if (TriggerCategory.All.Count > 1 && SelectedTriggerCategory != null)
                                                                               {
                                                                                   _ = TriggerCategory.All.Remove(SelectedTriggerCategory);
                                                                               }
                                                                               ShowDeleteTriggerCategoryConfirm = false;
                                                                           });

        // Token: 0x170001B3 RID: 435
        // (get) Token: 0x06000455 RID: 1109 RVA: 0x0000F4DC File Offset: 0x0000D6DC
        public GenericCommand OpenRepositoryCommand
        {
            get
            {
                var genericCommand = new GenericCommand
                {
                    Execute = delegate (object p)
                    {
                        Package.OpenRepository();
                    }
                };
                return genericCommand;
            }
        }

        // Token: 0x170001B4 RID: 436
        // (get) Token: 0x06000456 RID: 1110 RVA: 0x0000F548 File Offset: 0x0000D748
        public GenericCommand ShareCommand => new GenericCommand
        {
            Execute = delegate (object p)
            {
                if (!SelectedIsExportable)
                {
                    return;
                }
                IsRepositoryUpload = p is string && bool.Parse(p as string);
                UploadToClipboard = false;
                ShowUploadDialog = true;
            }
        };

        // Token: 0x170001B5 RID: 437
        // (get) Token: 0x06000457 RID: 1111 RVA: 0x0000F608 File Offset: 0x0000D808
        public GenericCommand ImportPackageCommand => new GenericCommand(delegate (object p)
                                                                   {
                                                                       var openFileDialog = new OpenFileDialog
                                                                       {
                                                                           AddExtension = true,
                                                                           DefaultExt = Configuration.GINAPackageFileExtension,
                                                                           CheckFileExists = true,
                                                                           CheckPathExists = true,
                                                                           Multiselect = false,
                                                                           Filter = "Package files|*." + Configuration.GINAPackageFileExtension + "|All Files|*.*"
                                                                       };
                                                                       if (openFileDialog.ShowDialog() == DialogResult.OK)
                                                                       {
                                                                           try
                                                                           {
                                                                               Package.OpenFilePackage(openFileDialog.FileName);
                                                                           }
                                                                           catch
                                                                           {
                                                                               base.App.Data.ShowError("Error", "An error occurred while attempting to import the file.");
                                                                           }
                                                                       }
                                                                   });

        // Token: 0x170001B6 RID: 438
        // (get) Token: 0x06000458 RID: 1112 RVA: 0x0000F738 File Offset: 0x0000D938
        public GenericCommand CreatePackageCommand => new GenericCommand(delegate (object p)
                                                                   {
                                                                       if (SelectedViewModels == null || !SelectedViewModels.Any<TriggerAndGroupViewModel>())
                                                                       {
                                                                           return;
                                                                       }
                                                                       var saveFileDialog = new SaveFileDialog
                                                                       {
                                                                           AddExtension = true,
                                                                           DefaultExt = "gtp",
                                                                           CheckFileExists = false,
                                                                           CheckPathExists = true,
                                                                           Filter = "Package files|*.gtp|All Files|*.*"
                                                                       };
                                                                       if (saveFileDialog.ShowDialog() == DialogResult.OK)
                                                                       {
                                                                           try
                                                                           {
                                                                               byte[] array = null;
                                                                               var enumerable = SelectedViewModels.Select<TriggerAndGroupViewModel, ITriggerLibraryEntry>(o =>
                                                                               {
                                                                                   if (!(o is TriggerGroupViewModel))
                                                                                   {
                                                                                       return ((TriggerViewModel)o).Trigger;
                                                                                   }
                                                                                   return ((TriggerGroupViewModel)o).TriggerGroup;
                                                                               });
                                                                               using (var package = Package.CreatePackage(enumerable))
                                                                               {
                                                                                   array = package.GetBytes();
                                                                               }
                                                                               File.WriteAllBytes(saveFileDialog.FileName, array);
                                                                           }
                                                                           catch
                                                                           {
                                                                               base.App.Data.ShowError("Error", "The file could not be created.");
                                                                           }
                                                                       }
                                                                   });

        // Token: 0x170001B7 RID: 439
        // (get) Token: 0x06000459 RID: 1113 RVA: 0x0000F75C File Offset: 0x0000D95C
        public GenericCommand ImportEverquestTriggersCommand => new GenericCommand(delegate (object p)
                                                                             {
                                                                                 if (EnsureEverquestFolder())
                                                                                 {
                                                                                     ShowEQImportDialog = true;
                                                                                 }
                                                                             });

        // Token: 0x170001B8 RID: 440
        // (get) Token: 0x0600045A RID: 1114 RVA: 0x0000F7A0 File Offset: 0x0000D9A0
        public GenericCommand ExportEverquestTriggersCommand => new GenericCommand(delegate (object p)
                                                                             {
                                                                                 if (EnsureEverquestFolder())
                                                                                 {
                                                                                     _ = Directory.CreateDirectory(Path.Combine(Settings.EverquestFolder, Configuration.EverquestAudioTriggersFolder, "_GINA"));
                                                                                     ShowEQExportDialog = true;
                                                                                 }
                                                                             });

        // Token: 0x170001B9 RID: 441
        // (get) Token: 0x0600045B RID: 1115 RVA: 0x0000F890 File Offset: 0x0000DA90
        public GenericCommand ImportGamTextTriggersCommand => new GenericCommand(delegate (object p)
                                                                           {
                                                                               var openFileDialog = new OpenFileDialog
                                                                               {
                                                                                   AddExtension = true,
                                                                                   DefaultExt = Configuration.GamTextTriggerExtension,
                                                                                   CheckFileExists = true,
                                                                                   CheckPathExists = true,
                                                                                   Multiselect = false
                                                                               };
                                                                               if (p is string text)
                                                                               {
                                                                                   if (!(text == "config"))
                                                                                   {
                                                                                       if (text == "export")
                                                                                       {
                                                                                           openFileDialog.Filter = "GamTextTrigger Export Files|*." + Configuration.GamTextTriggerExtension;
                                                                                       }
                                                                                   }
                                                                                   else
                                                                                   {
                                                                                       openFileDialog.Filter = "GamTextTrigger Config File|GamTextTriggers.ini";
                                                                                   }
                                                                               }
                                                                               var openFileDialog2 = openFileDialog;
                                                                               openFileDialog2.Filter += "|All Files|*.*";
                                                                               if (openFileDialog.ShowDialog() == DialogResult.OK)
                                                                               {
                                                                                   try
                                                                                   {
                                                                                       Package.OpenGamTextTriggerFile(openFileDialog.FileName);
                                                                                   }
                                                                                   catch
                                                                                   {
                                                                                       base.App.Data.ShowError("Error", "An error occurred while attempting to import the file.");
                                                                                   }
                                                                               }
                                                                           });

        // Token: 0x170001BA RID: 442
        // (get) Token: 0x0600045C RID: 1116 RVA: 0x0000F958 File Offset: 0x0000DB58
        public GenericCommand ExportGamTextTriggersCommand => new GenericCommand(delegate (object p)
                                                                           {
                                                                               if (SelectedViewModels == null || !SelectedViewModels.Any<TriggerAndGroupViewModel>())
                                                                               {
                                                                                   return;
                                                                               }
                                                                               var saveFileDialog = new SaveFileDialog
                                                                               {
                                                                                   AddExtension = true,
                                                                                   DefaultExt = Configuration.GamTextTriggerExtension,
                                                                                   CheckFileExists = false,
                                                                                   CheckPathExists = true,
                                                                                   Filter = "GamTextTrigger files|*." + Configuration.GamTextTriggerExtension + "|All Files|*.*"
                                                                               };
                                                                               if (saveFileDialog.ShowDialog() == DialogResult.OK)
                                                                               {
                                                                                   try
                                                                                   {
                                                                                       Package.CreateGamTextTriggerFile(saveFileDialog.FileName, GetEffectiveTriggers(SelectedViewModels));
                                                                                   }
                                                                                   catch
                                                                                   {
                                                                                       base.App.Data.ShowError("Error", "The file could not be created.");
                                                                                   }
                                                                               }
                                                                           });

        // Token: 0x170001BB RID: 443
        // (get) Token: 0x0600045D RID: 1117 RVA: 0x0000FA18 File Offset: 0x0000DC18
        public GenericCommand CharacterSelectionChangedCommand => new GenericCommand
        {
            Execute = delegate (object p)
            {
                var selectionChangedEventArgs = p as SelectionChangedEventArgs;
                foreach (var characterViewModel in selectionChangedEventArgs.AddedItems.Cast<CharacterViewModel>())
                {
                    characterViewModel.IsSelected = true;
                }
                foreach (var characterViewModel2 in selectionChangedEventArgs.RemovedItems.Cast<CharacterViewModel>())
                {
                    characterViewModel2.IsSelected = false;
                }
                base.RaisePropertyChanged("SelectedCharacters");
            }
        };

        // Token: 0x170001BC RID: 444
        // (get) Token: 0x0600045E RID: 1118 RVA: 0x0000FA68 File Offset: 0x0000DC68
        public GenericCommand ToggleMonitorCommand
        {
            get
            {
                var genericCommand = new GenericCommand
                {
                    Execute = delegate (object p)
                    {
                        if (!(p is GINACharacter ginacharacter))
                        {
                            return;
                        }
                        ginacharacter.SetMonitoringStatus(!ginacharacter.IsEnabled, true);
                    }
                };
                return genericCommand;
            }
        }

        // Token: 0x170001BD RID: 445
        // (get) Token: 0x0600045F RID: 1119 RVA: 0x0000FAEC File Offset: 0x0000DCEC
        public GenericCommand SelectedViewModelsChangedCommand => new GenericCommand
        {
            Execute = delegate (object p)
            {
                if (!(p is TreeCollection.TreeSelectionChangedEventArgs treeSelectionChangedEventArgs) || treeSelectionChangedEventArgs.SelectedItems == null || !treeSelectionChangedEventArgs.SelectedItems.Any<ITreeItem>())
                {
                    SelectedViewModels = null;
                    return;
                }
                SelectedViewModels = treeSelectionChangedEventArgs.SelectedItems.Cast<TriggerAndGroupViewModel>().ToList<TriggerAndGroupViewModel>();
            }
        };

        // Token: 0x170001BE RID: 446
        // (get) Token: 0x06000460 RID: 1120 RVA: 0x0000FB44 File Offset: 0x0000DD44
        public GenericCommand IsDropAllowedCommand => new GenericCommand
        {
            Execute = delegate (object p)
            {
                if (!(p is TreeList.QueryCanDropItemsEventArgs queryCanDropItemsEventArgs))
                {
                    return;
                }
                queryCanDropItemsEventArgs.CanDrop = IsMoveAllowed(queryCanDropItemsEventArgs.DraggedItems, queryCanDropItemsEventArgs.DropItem);
            }
        };

        // Token: 0x170001BF RID: 447
        // (get) Token: 0x06000461 RID: 1121 RVA: 0x0000FB98 File Offset: 0x0000DD98
        public GenericCommand MoveEntriesCommand => new GenericCommand
        {
            Execute = delegate (object p)
            {
                if (!(p is TreeList.DropItemsEventArgs dropItemsEventArgs))
                {
                    return;
                }
                MoveEntries(dropItemsEventArgs.DraggedItems, dropItemsEventArgs.DropItem);
            }
        };

        // Token: 0x170001C0 RID: 448
        // (get) Token: 0x06000462 RID: 1122 RVA: 0x0000FBDE File Offset: 0x0000DDDE
        public GenericCommand ClipboardQuickShareCommand => new GenericCommand(delegate (object p)
                                                                         {
                                                                             if (!SelectedIsExportable)
                                                                             {
                                                                                 return;
                                                                             }
                                                                             IsRepositoryUpload = false;
                                                                             UploadToClipboard = true;
                                                                             ShowUploadDialog = true;
                                                                         });

        // Token: 0x170001C1 RID: 449
        // (get) Token: 0x06000463 RID: 1123 RVA: 0x0000FCC7 File Offset: 0x0000DEC7
        public GenericCommand ClipboardGamTextTriggerCommand => new GenericCommand(delegate (object p)
                                                                             {
                                                                                 if (SelectedTrigger == null)
                                                                                 {
                                                                                     return;
                                                                                 }
                                                                                 var gamTextTriggerTranslation = new GamTextTriggerTranslation(SelectedTrigger);
                                                                                 if (gamTextTriggerTranslation.IsCompatible)
                                                                                 {
                                                                                     base.App.Data.CopyTextToClipboard(string.Format(Configuration.GamTextTriggerClipboardFormat, gamTextTriggerTranslation.Translation));
                                                                                     return;
                                                                                 }
                                                                                 switch (gamTextTriggerTranslation.Reason)
                                                                                 {
                                                                                     case GamTextTriggerTranslation.IncompatabilityReason.UsesCandSTags:
                                                                                         base.App.Data.ShowError("Incompatible", "GamTextTriggers will only support one wildcard [either {C} or {S}] in the search text.");
                                                                                         return;
                                                                                     case GamTextTriggerTranslation.IncompatabilityReason.UsesNumberedSTags:
                                                                                         base.App.Data.ShowError("Incompatible", "GamTextTriggers does not support numbered wildcards [{S1}, {S2}, etc].");
                                                                                         return;
                                                                                     case GamTextTriggerTranslation.IncompatabilityReason.UsesRegexGroups:
                                                                                         base.App.Data.ShowError("Incompatible", "GamTextTriggers does not support RegEx groups.");
                                                                                         return;
                                                                                     default:
                                                                                         base.App.Data.ShowError("Incompatible", "This trigger is not compatible with GamTextTriggers.");
                                                                                         return;
                                                                                 }
                                                                             });

        // Token: 0x170001C2 RID: 450
        // (get) Token: 0x06000464 RID: 1124 RVA: 0x0000FD33 File Offset: 0x0000DF33
        public GenericCommand ClipboardTriggerSearchTextCommand => new GenericCommand(delegate (object p)
                                                                                {
                                                                                    if (SelectedTrigger == null)
                                                                                    {
                                                                                        return;
                                                                                    }
                                                                                    base.App.Data.CopyTextToClipboard((SelectedTrigger.TriggerText ?? "").Replace("{C}", Configuration.Current.CTagClipboardReplacement ?? "{C}"));
                                                                                });

        // Token: 0x170001C3 RID: 451
        // (get) Token: 0x06000465 RID: 1125 RVA: 0x0000FE64 File Offset: 0x0000E064
        public GenericCommand ClipboardLogAsHTML => new GenericCommand(delegate (object o)
                                                                 {
                                                                     var stringBuilder = new StringBuilder();
                                                                     _ = stringBuilder.Append("<table border=\"2\" cellpadding=\"1\" cellspacing=\"0\">");
                                                                     _ = stringBuilder.Append("<tr><th>Character</th><th>Trigger</th><th>Log Time</th><th>Match Time</th></tr>");
                                                                     if (o != null)
                                                                     {
                                                                         var list = (o as IList).Cast<TriggerMatchedEventArgs>().ToList<TriggerMatchedEventArgs>();
                                                                         foreach (var triggerMatchedEventArgs in list)
                                                                         {
                                                                             _ = stringBuilder.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td></tr>", new object[]
                                                                             {
                                WebUtility.HtmlEncode(triggerMatchedEventArgs.TriggerFilter.Character.DisplayName),
                                WebUtility.HtmlEncode(triggerMatchedEventArgs.TriggerFilter.Trigger.TriggerPath),
                                triggerMatchedEventArgs.LoggedTime.ToString("yyyy/MM/dd hh:mm:ss tt"),
                                triggerMatchedEventArgs.MatchedTime.ToString("yyyy/MM/dd hh:mm:ss tt")
                                                                             });
                                                                         }
                                                                     }
                                                                     _ = stringBuilder.Append("</table>");
                                                                     base.App.Data.CopyTextToClipboard(stringBuilder.ToString());
                                                                 });

        // Token: 0x170001C4 RID: 452
        // (get) Token: 0x06000466 RID: 1126 RVA: 0x0000FE95 File Offset: 0x0000E095
        public GenericCommand CutEntriesCommand => new GenericCommand(delegate (object p)
                                                                {
                                                                    ClipboardedEntries = SelectedViewModels?.ToList<TriggerAndGroupViewModel>();
                                                                });

        // Token: 0x170001C5 RID: 453
        // (get) Token: 0x06000467 RID: 1127 RVA: 0x0000FEBC File Offset: 0x0000E0BC
        public GenericCommand PasteEntriesCommand => new GenericCommand(delegate (object p)
                                                                  {
                                                                      MoveEntries(ClipboardedEntries, SelectedTriggerGroupViewModel);
                                                                  });

        // Token: 0x170001C6 RID: 454
        // (get) Token: 0x06000468 RID: 1128 RVA: 0x0000FFA4 File Offset: 0x0000E1A4
        public GenericCommand CharacterInstructionCommand => new GenericCommand(delegate (object p)
                                                                          {
                                                                              if (p is string text)
                                                                              {
                                                                                  if (!(text == "stop"))
                                                                                  {
                                                                                      if (!(text == "resetcounters"))
                                                                                      {
                                                                                          return;
                                                                                      }
                                                                                  }
                                                                                  else
                                                                                  {
                                                                                      using (var enumerator = base.App.Data.Characters.GetEnumerator())
                                                                                      {
                                                                                          while (enumerator.MoveNext())
                                                                                          {
                                                                                              var characterViewModel = enumerator.Current;
                                                                                              characterViewModel.Character.StopMatches();
                                                                                          }
                                                                                          return;
                                                                                      }
                                                                                  }
                                                                                  foreach (var characterViewModel2 in base.App.Data.Characters)
                                                                                  {
                                                                                      characterViewModel2.Character.ResetMatchCounters();
                                                                                  }
                                                                              }
                                                                          });

        // Token: 0x170001C7 RID: 455
        // (get) Token: 0x06000469 RID: 1129 RVA: 0x00010088 File Offset: 0x0000E288
        public GenericCommand ResetProfilerCountersCommand => new GenericCommand
        {
            Execute = delegate (object p)
            {
                foreach (var trigger in TriggerGroup.All.SelectMany((TriggerGroup o) => o.Triggers).ToList<Trigger>())
                {
                    trigger.ResetComparisonCounters();
                }
                _ProfilerStartTime = DateTime.Now;
                foreach (var ginacharacter in GINACharacter.All)
                {
                    ginacharacter.ProfiledLines = 0L;
                }
                RefreshProfiler();
            }
        };

        // Token: 0x170001C8 RID: 456
        // (get) Token: 0x0600046A RID: 1130 RVA: 0x000100DC File Offset: 0x0000E2DC
        public GenericCommand StartProfilerCommand => new GenericCommand
        {
            Execute = delegate (object p)
            {
                if (!Settings.EnableProfiler)
                {
                    Settings.EnableProfiler = true;
                    return;
                }
                if (!ProfilerRunning)
                {
                    ProfilerRunning = true;
                }
            }
        };

        // Token: 0x170001C9 RID: 457
        // (get) Token: 0x0600046B RID: 1131 RVA: 0x00010124 File Offset: 0x0000E324
        public GenericCommand StopProfilerCommand => new GenericCommand
        {
            Execute = delegate (object p)
            {
                Settings.EnableProfiler = false;
                ResetProfilerCountersCommand.Execute(null);
            }
        };

        // Token: 0x170001CA RID: 458
        // (get) Token: 0x0600046C RID: 1132 RVA: 0x00010154 File Offset: 0x0000E354
        public GenericCommand PauseProfilerCommand => new GenericCommand
        {
            Execute = delegate (object p)
            {
                ProfilerRunning = false;
            }
        };

        // Token: 0x170001CB RID: 459
        // (get) Token: 0x0600046D RID: 1133 RVA: 0x00010184 File Offset: 0x0000E384
        public GenericCommand RefreshProfilerCommand => new GenericCommand
        {
            Execute = delegate (object p)
            {
                RefreshProfiler();
            }
        };

        // Token: 0x040000DD RID: 221
        private readonly DispatcherTimer _Timer;

        // Token: 0x040000DE RID: 222
        private readonly DispatcherTimer _ProfilerTimer;

        // Token: 0x040000E0 RID: 224
        private List<Tuple<string, int>> _RefreshIntervals;

        // Token: 0x040000E1 RID: 225
        private DateTime _ProfilerStartTime = DateTime.Now;

        // Token: 0x02000028 RID: 40
        public class ProcessorItem : BindableObject
        {
            // Token: 0x060004C6 RID: 1222 RVA: 0x000101CC File Offset: 0x0000E3CC
            public ProcessorItem()
            {
                Configuration.Current.PropertyChanged += delegate (object o, PropertyChangedEventArgs e)
                {
                    if (e.PropertyName == "ProcessorAffinity")
                    {
                        base.RaisePropertyChanged("IsSelected");
                    }
                };
            }

            // Token: 0x170001CC RID: 460
            // (get) Token: 0x060004C7 RID: 1223 RVA: 0x000101FC File Offset: 0x0000E3FC
            // (set) Token: 0x060004C8 RID: 1224 RVA: 0x00010209 File Offset: 0x0000E409
            public int CPUNumber
            {
                get => base.Get<int>("CPUNumber"); set => base.Set("CPUNumber", value);
            }

            // Token: 0x170001CD RID: 461
            // (get) Token: 0x060004C9 RID: 1225 RVA: 0x0001021C File Offset: 0x0000E41C
            public string ProcessorName => string.Format("CPU {0}", CPUNumber);

            // Token: 0x170001CE RID: 462
            // (get) Token: 0x060004CA RID: 1226 RVA: 0x00010233 File Offset: 0x0000E433
            public long ProcessorMask => Convert.ToInt64(Math.Pow(2.0, CPUNumber - 1));

            // Token: 0x170001CF RID: 463
            // (get) Token: 0x060004CB RID: 1227 RVA: 0x00010251 File Offset: 0x0000E451
            // (set) Token: 0x060004CC RID: 1228 RVA: 0x00010268 File Offset: 0x0000E468
            public bool IsSelected
            {
                get => (Configuration.Current.ProcessorAffinity & ProcessorMask) > 0L;
                set
                {
                    if (value != IsSelected)
                    {
                        if (!value && ProcessorMask == Configuration.Current.ProcessorAffinity)
                        {
                            return;
                        }
                        if (value)
                        {
                            Configuration.Current.ProcessorAffinity |= ProcessorMask;
                        }
                        else
                        {
                            Configuration.Current.ProcessorAffinity ^= ProcessorMask;
                        }
                        base.RaisePropertyChanged("IsSelected");
                    }
                }
            }
        }
    }
}
