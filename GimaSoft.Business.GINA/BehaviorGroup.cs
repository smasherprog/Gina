using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Threading;
using System.Xml;
using WPFShared;

namespace GimaSoft.Business.GINA
{
    // Token: 0x02000003 RID: 3
    public class BehaviorGroup : GINABusinessObject
    {
        // Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000258
        static BehaviorGroup()
        {
            BindableObject.SetDependentProperties(typeof(BehaviorGroup));
        }

        // Token: 0x06000003 RID: 3 RVA: 0x00002069 File Offset: 0x00000269
        public static void RegisterDependentProperties(Type type)
        {
            BindableObject.RegisterDependentProperty(type, "Font", "FontName", null);
            BindableObject.RegisterDependentProperty(type, "Font", "FontSize", null);
        }

        // Token: 0x17000001 RID: 1
        // (get) Token: 0x06000004 RID: 4 RVA: 0x0000208D File Offset: 0x0000028D
        public static ObservableCollection<BehaviorGroup> All
        {
            get
            {
                if (BehaviorGroup._All == null)
                {
                    BehaviorGroup._All = new ObservableCollection<BehaviorGroup>();
                }
                return BehaviorGroup._All;
            }
        }

        // Token: 0x06000005 RID: 5 RVA: 0x000020A8 File Offset: 0x000002A8
        public static void Clear()
        {
            var list = BehaviorGroup._All.ToList<BehaviorGroup>();
            BehaviorGroup.All.Clear();
            foreach (var behaviorGroup in list)
            {
                behaviorGroup.Dispose();
            }
        }

        // Token: 0x17000002 RID: 2
        // (get) Token: 0x06000006 RID: 6 RVA: 0x0000213C File Offset: 0x0000033C
        public static BehaviorGroup DefaultTextGroup
        {
            get
            {
                var behaviorGroup = BehaviorGroup.All.SingleOrDefault((BehaviorGroup o) => o.BehaviorType == BehaviorTypes.Text && o.Name.ToLower() == "default");
                BehaviorGroup behaviorGroup2;
                if ((behaviorGroup2 = behaviorGroup) == null)
                {
                    behaviorGroup2 = BehaviorGroup.All.FirstOrDefault((BehaviorGroup o) => o.BehaviorType == BehaviorTypes.Text);
                }
                return behaviorGroup2;
            }
        }

        // Token: 0x17000003 RID: 3
        // (get) Token: 0x06000007 RID: 7 RVA: 0x000021CC File Offset: 0x000003CC
        public static BehaviorGroup DefaultTimerGroup
        {
            get
            {
                var behaviorGroup = BehaviorGroup.All.SingleOrDefault((BehaviorGroup o) => o.BehaviorType == BehaviorTypes.Timer && o.Name.ToLower() == "default");
                BehaviorGroup behaviorGroup2;
                if ((behaviorGroup2 = behaviorGroup) == null)
                {
                    behaviorGroup2 = BehaviorGroup.All.FirstOrDefault((BehaviorGroup o) => o.BehaviorType == BehaviorTypes.Timer);
                }
                return behaviorGroup2;
            }
        }

        // Token: 0x06000008 RID: 8 RVA: 0x00002244 File Offset: 0x00000444
        public static void Init()
        {
            if (!BehaviorGroup.All.Any((BehaviorGroup o) => o.BehaviorType == BehaviorTypes.Text))
            {
                _ = BehaviorGroup.Create(BehaviorTypes.Text, "Default", global::System.Drawing.FontFamily.Families.First<global::System.Drawing.FontFamily>().Name, 50, new SolidColorBrush(Colors.Yellow), TimerSortMethod.OrderTriggered);
            }
            if (!BehaviorGroup.All.Any((BehaviorGroup o) => o.BehaviorType == BehaviorTypes.Timer))
            {
                _ = BehaviorGroup.Create(BehaviorTypes.Timer, "Default", global::System.Drawing.FontFamily.Families.First<global::System.Drawing.FontFamily>().Name, 50, new SolidColorBrush(Colors.Yellow), TimerSortMethod.OrderTriggered);
            }
        }

        // Token: 0x06000009 RID: 9 RVA: 0x00002338 File Offset: 0x00000538
        public static string GetNextName(BehaviorTypes type)
        {
            var text = "Overlay #{0}";
            var num = 1;
            for (; ; )
            {
                var name = string.Format(text, num);
                if (!BehaviorGroup.All.Any((BehaviorGroup o) => o.BehaviorType == type && o.Name.ToLower() == name.ToLower()))
                {
                    break;
                }
                num++;
            }
            return string.Empty;
            //return CS$<> 8__locals2.name;
        }

        // Token: 0x0600000A RID: 10 RVA: 0x000023D4 File Offset: 0x000005D4
        public static BehaviorGroup GetGroupByName(BehaviorTypes type, string name, BehaviorGroup valueIfNotFound)
        {
            var behaviorGroup = BehaviorGroup.All.FirstOrDefault((BehaviorGroup o) => o.BehaviorType == type && o.Name.ToLower() == name.ToLower());
            if (behaviorGroup == null)
            {
                return valueIfNotFound;
            }
            return behaviorGroup;
        }

        // Token: 0x0600000B RID: 11 RVA: 0x00002468 File Offset: 0x00000668
        public BehaviorGroup()
        {
            TextFadeTime = 10;
            ShowTimerBar = true;
            EmptyBarColor = global::System.Windows.Media.Color.FromArgb(196, 0, 0, 0);
            Matches = new ObservableCollection<TriggerMatchedEventArgs>();
            _Timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1.0),
                IsEnabled = false
            };
            _Timer.Tick += delegate (object o, EventArgs e)
            {
                if (!Matches.Any<TriggerMatchedEventArgs>())
                {
                    return;
                }
                if (BehaviorType == BehaviorTypes.Text)
                {
                    DoTextTick();
                }
                else if (BehaviorType == BehaviorTypes.Timer)
                {
                    DoTimerTick();
                }
                _Timer.IsEnabled = Matches.Any<TriggerMatchedEventArgs>();
            };
        }

        // Token: 0x0600000C RID: 12 RVA: 0x000024F0 File Offset: 0x000006F0
        public static BehaviorGroup Create(BehaviorTypes type, string name, string fontName, int fontSize, SolidColorBrush fontColor, TimerSortMethod sortMethod)
        {
            var behaviorGroup = new BehaviorGroup
            {
                BehaviorType = type,
                Name = name,
                FontName = fontName,
                FontSize = fontSize,
                SortMethod = sortMethod
            };
            BehaviorGroup.All.Add(behaviorGroup);
            return behaviorGroup;
        }

        // Token: 0x0600000D RID: 13 RVA: 0x00002538 File Offset: 0x00000738
        public static BehaviorGroup Create(BehaviorTypes type)
        {
            var font = BehaviorGroup.GetFont(null, 50f);
            var behaviorGroup = new BehaviorGroup
            {
                BehaviorType = type,
                Name = BehaviorGroup.GetNextName(type),
                FontName = font.Name,
                FontSize = (type == BehaviorTypes.Text) ? 50 : 15,
                SortMethod = TimerSortMethod.OrderTriggered
            };
            BehaviorGroup.All.Add(behaviorGroup);
            return behaviorGroup;
        }

        // Token: 0x17000004 RID: 4
        // (get) Token: 0x0600000E RID: 14 RVA: 0x0000259B File Offset: 0x0000079B
        // (set) Token: 0x0600000F RID: 15 RVA: 0x000025A8 File Offset: 0x000007A8
        public BehaviorTypes BehaviorType
        {
            get => base.Get<BehaviorTypes>("BehaviorType");
            set
            {
                base.Set("BehaviorType", value);
                _Timer.Interval = (value == BehaviorTypes.Timer) ? TimeSpan.FromMilliseconds(50.0) : TimeSpan.FromSeconds(1.0);
            }
        }

        // Token: 0x17000005 RID: 5
        // (get) Token: 0x06000010 RID: 16 RVA: 0x000025E8 File Offset: 0x000007E8
        // (set) Token: 0x06000011 RID: 17 RVA: 0x000025F5 File Offset: 0x000007F5
        public string Name
        {
            get => base.Get<string>("Name"); set => base.Set("Name", value);
        }

        // Token: 0x17000006 RID: 6
        // (get) Token: 0x06000012 RID: 18 RVA: 0x00002603 File Offset: 0x00000803
        // (set) Token: 0x06000013 RID: 19 RVA: 0x00002610 File Offset: 0x00000810
        public string WindowLayout
        {
            get => base.Get<string>("WindowLayout"); set => base.Set("WindowLayout", value);
        }

        // Token: 0x17000007 RID: 7
        // (get) Token: 0x06000014 RID: 20 RVA: 0x0000261E File Offset: 0x0000081E
        // (set) Token: 0x06000015 RID: 21 RVA: 0x0000262B File Offset: 0x0000082B
        public string FontName
        {
            get => base.Get<string>("FontName");
            set
            {
                base.Set("FontName", value);
                _Font = null;
            }
        }

        // Token: 0x17000008 RID: 8
        // (get) Token: 0x06000016 RID: 22 RVA: 0x00002640 File Offset: 0x00000840
        // (set) Token: 0x06000017 RID: 23 RVA: 0x0000264D File Offset: 0x0000084D
        public int FontSize
        {
            get => base.Get<int>("FontSize");
            set
            {
                base.Set("FontSize", value);
                _Font = null;
            }
        }

        // Token: 0x17000009 RID: 9
        // (get) Token: 0x06000018 RID: 24 RVA: 0x00002667 File Offset: 0x00000867
        // (set) Token: 0x06000019 RID: 25 RVA: 0x00002674 File Offset: 0x00000874
        public bool GroupByCharacter
        {
            get => base.Get<bool>("GroupByCharacter");
            set
            {
                base.Set("GroupByCharacter", value);
                RefreshSort();
            }
        }

        // Token: 0x1700000A RID: 10
        // (get) Token: 0x0600001A RID: 26 RVA: 0x0000268D File Offset: 0x0000088D
        // (set) Token: 0x0600001B RID: 27 RVA: 0x0000269A File Offset: 0x0000089A
        public TimerSortMethod SortMethod
        {
            get => base.Get<TimerSortMethod>("SortMethod");
            set
            {
                base.Set("SortMethod", value);
                RefreshSort();
            }
        }

        // Token: 0x1700000B RID: 11
        // (get) Token: 0x0600001C RID: 28 RVA: 0x000026B3 File Offset: 0x000008B3
        // (set) Token: 0x0600001D RID: 29 RVA: 0x000026C0 File Offset: 0x000008C0
        public int TextFadeTime
        {
            get => base.Get<int>("TextFadeTime"); set => base.Set("TextFadeTime", value);
        }

        // Token: 0x1700000C RID: 12
        // (get) Token: 0x0600001E RID: 30 RVA: 0x000026D3 File Offset: 0x000008D3
        // (set) Token: 0x0600001F RID: 31 RVA: 0x000026E0 File Offset: 0x000008E0
        public global::System.Windows.Media.Color EmptyBarColor
        {
            get => base.Get<global::System.Windows.Media.Color>("EmptyBarColor"); set => base.Set("EmptyBarColor", value);
        }

        // Token: 0x1700000D RID: 13
        // (get) Token: 0x06000020 RID: 32 RVA: 0x000026F3 File Offset: 0x000008F3
        // (set) Token: 0x06000021 RID: 33 RVA: 0x00002700 File Offset: 0x00000900
        public bool ShowTimerBar
        {
            get => base.Get<bool>("ShowTimerBar"); set => base.Set("ShowTimerBar", value);
        }

        // Token: 0x1700000E RID: 14
        // (get) Token: 0x06000022 RID: 34 RVA: 0x00002713 File Offset: 0x00000913
        // (set) Token: 0x06000023 RID: 35 RVA: 0x00002720 File Offset: 0x00000920
        public bool StandardizeTimerBars
        {
            get => base.Get<bool>("StandardizeTimerBars"); set => base.Set("StandardizeTimerBars", value);
        }

        // Token: 0x1700000F RID: 15
        // (get) Token: 0x06000024 RID: 36 RVA: 0x00002733 File Offset: 0x00000933
        // (set) Token: 0x06000025 RID: 37 RVA: 0x00002740 File Offset: 0x00000940
        public global::System.Windows.Media.Color BackgroundColor
        {
            get => base.Get<global::System.Windows.Media.Color>("BackgroundColor"); set => base.Set("BackgroundColor", value);
        }

        // Token: 0x17000010 RID: 16
        // (get) Token: 0x06000026 RID: 38 RVA: 0x00002753 File Offset: 0x00000953
        // (set) Token: 0x06000027 RID: 39 RVA: 0x00002760 File Offset: 0x00000960
        public global::System.Windows.Media.Color BackgroundFadedColor
        {
            get => base.Get<global::System.Windows.Media.Color>("BackgroundFadedColor");
            set
            {
                if (value.A > 225)
                {
                    value.A = 225;
                    base.Set("BackgroundFadedColor", value);
                    base.RaisePropertyChanged("BackgroundFadedColor");
                    return;
                }
                base.Set("BackgroundFadedColor", value);
            }
        }

        // Token: 0x17000011 RID: 17
        // (get) Token: 0x06000028 RID: 40 RVA: 0x000027B5 File Offset: 0x000009B5
        // (set) Token: 0x06000029 RID: 41 RVA: 0x000027C2 File Offset: 0x000009C2
        public ObservableCollection<TriggerMatchedEventArgs> Matches
        {
            get => base.Get<ObservableCollection<TriggerMatchedEventArgs>>("Matches"); set => base.Set("Matches", value);
        }

        // Token: 0x17000012 RID: 18
        // (get) Token: 0x0600002A RID: 42 RVA: 0x000027D0 File Offset: 0x000009D0
        public Font Font
        {
            get
            {
                if (_Font == null)
                {
                    _Font = BehaviorGroup.GetFont(FontName, FontSize);
                }
                return _Font;
            }
        }

        // Token: 0x17000013 RID: 19
        // (get) Token: 0x0600002B RID: 43 RVA: 0x00002818 File Offset: 0x00000A18
        public int StandardizedTotalMilliseconds
        {
            get
            {
                var list = (from o in Matches
                            where o.TriggerFilter.Trigger.TimerType.CountsDown()
                            select o.TotalMilliseconds).ToList<int>();
                if (!list.Any<int>())
                {
                    return 1;
                }
                return list.Max();
            }
        }

        // Token: 0x14000001 RID: 1
        // (add) Token: 0x0600002C RID: 44 RVA: 0x00002888 File Offset: 0x00000A88
        // (remove) Token: 0x0600002D RID: 45 RVA: 0x000028C0 File Offset: 0x00000AC0
        public event BehaviorMatchesCollectionChangedHandler BehaviorMatchesCollectionChanged;

        // Token: 0x0600002E RID: 46 RVA: 0x000028F5 File Offset: 0x00000AF5
        protected void OnBehaviorMatchesCollectionChanged()
        {
            BehaviorMatchesCollectionChanged?.Invoke(this);
        }

        // Token: 0x0600002F RID: 47 RVA: 0x0000290C File Offset: 0x00000B0C
        private void RefreshSort()
        {
            var defaultView = CollectionViewSource.GetDefaultView(Matches);
            defaultView.SortDescriptions.Clear();
            if (GroupByCharacter)
            {
                defaultView.SortDescriptions.Add(new SortDescription("TriggerFilter.Character.CharacterIndex", ListSortDirection.Ascending));
            }
            defaultView.SortDescriptions.Add(new SortDescription((SortMethod == TimerSortMethod.TimeRemaining) ? "MillisecondsRemaining" : "MatchedTime", ListSortDirection.Ascending));
        }

        // Token: 0x06000030 RID: 48 RVA: 0x00002D84 File Offset: 0x00000F84
        private void ModifyMatches(TriggerMatchedEventArgs args, BehaviorGroup.CollectionOperation operation, int? index = null, GINACharacter character = null)
        {
            var application = Application.Current;
            if (application == null)
            {
                return;
            }
            _ = application.Dispatcher.Invoke(DispatcherPriority.Normal, new ThreadStart(delegate
            {
                lock (Matches)
                {
                    switch (operation)
                    {
                        case BehaviorGroup.CollectionOperation.Add:
                            break;
                        case BehaviorGroup.CollectionOperation.Remove:
                            if (args != null && Matches.Contains(args))
                            {
                                _ = Matches.Remove(args);
                                goto IL_02E3;
                            }
                            goto IL_02E3;
                        case BehaviorGroup.CollectionOperation.RestartTimer:
                            {
                                var triggerMatchedEventArgs = Matches.FirstOrDefault((TriggerMatchedEventArgs o) => o.TriggerFilter == args.TriggerFilter && (!args.TriggerFilter.Trigger.RestartBasedOnTimerName || o.DisplayText == args.DisplayText));
                                if (triggerMatchedEventArgs != null)
                                {
                                    Matches.Insert(Matches.IndexOf(triggerMatchedEventArgs), args);
                                    _ = Matches.Remove(triggerMatchedEventArgs);
                                    triggerMatchedEventArgs.Dispose(true);
                                    goto IL_02E3;
                                }
                                Matches.Add(args);
                                goto IL_02E3;
                            }
                        case BehaviorGroup.CollectionOperation.RepeatTimer:
                            {
                                var value = args.TargetDateTime.Value;
                                args.TargetDateTime = new DateTime?(value.AddMilliseconds(args.TimerSpanMilliseconds ?? args.TriggerFilter.Trigger.TimerMillisecondDuration));
                                args.TimerEndingDone = false;
                                args.MatchedTime = value;
                                if (args.EndingDateTime != null)
                                {
                                    args.EndingDateTime = new DateTime?(args.TargetDateTime.Value.AddSeconds(-1 * args.TriggerFilter.Trigger.TimerEndingTime));
                                    goto IL_02E3;
                                }
                                goto IL_02E3;
                            }
                        case BehaviorGroup.CollectionOperation.AddSingleton:
                            if (Matches.FirstOrDefault((TriggerMatchedEventArgs o) => o.TriggerFilter == args.TriggerFilter && o.DisplayText == args.DisplayText) == null)
                            {
                                Matches.Add(args);
                                goto IL_02E3;
                            }
                            args.Dispose(true);
                            goto IL_02E3;
                        case BehaviorGroup.CollectionOperation.Clear:
                            Matches.Clear();
                            goto IL_02E3;
                        case BehaviorGroup.CollectionOperation.ClearCharacter:
                            {
                                var list = Matches.Where((TriggerMatchedEventArgs o) => o.TriggerFilter.Character == character).ToList<TriggerMatchedEventArgs>();
                                using (var enumerator = list.GetEnumerator())
                                {
                                    while (enumerator.MoveNext())
                                    {
                                        var triggerMatchedEventArgs2 = enumerator.Current;
                                        _ = Matches.Remove(triggerMatchedEventArgs2);
                                        triggerMatchedEventArgs2.Dispose(true);
                                    }
                                    goto IL_02E3;
                                }
                                break;
                            }
                        default:
                            goto IL_02E3;
                    }
                    Matches.Add(args);
                IL_02E3:;
                }
                OnBehaviorMatchesCollectionChanged();
                RaisePropertyChanged("StandardizedTotalMilliseconds");
                _Timer.IsEnabled = Matches.Any<TriggerMatchedEventArgs>();
            }));
        }

        // Token: 0x06000031 RID: 49 RVA: 0x00002DD8 File Offset: 0x00000FD8
        public void AddMatch(TriggerMatchedEventArgs args, bool discardIfDuplicate = false)
        {
            if (BehaviorType == BehaviorTypes.Text && !Configuration.Current.EnableText)
            {
                return;
            }
            if (BehaviorType == BehaviorTypes.Timer && !Configuration.Current.EnableTimers)
            {
                return;
            }
            ModifyMatches(args, discardIfDuplicate ? BehaviorGroup.CollectionOperation.AddSingleton : BehaviorGroup.CollectionOperation.Add, null, null);
        }

        // Token: 0x06000032 RID: 50 RVA: 0x00002E2C File Offset: 0x0000102C
        public void RemoveMatch(TriggerMatchedEventArgs args, bool dispose = true)
        {
            if (args != null)
            {
                ModifyMatches(args, BehaviorGroup.CollectionOperation.Remove, null, null);
                if (dispose)
                {
                    args.Dispose(true);
                }
            }
        }

        // Token: 0x06000033 RID: 51 RVA: 0x00002E58 File Offset: 0x00001058
        public void RestartTimer(TriggerMatchedEventArgs args)
        {
            ModifyMatches(args, BehaviorGroup.CollectionOperation.RestartTimer, null, null);
        }

        // Token: 0x06000034 RID: 52 RVA: 0x00002E78 File Offset: 0x00001078
        public void RepeatTimer(TriggerMatchedEventArgs args)
        {
            ModifyMatches(args, BehaviorGroup.CollectionOperation.RepeatTimer, null, null);
        }

        // Token: 0x06000035 RID: 53 RVA: 0x00002E98 File Offset: 0x00001098
        public void ClearMatches(GINACharacter character)
        {
            ModifyMatches(null, BehaviorGroup.CollectionOperation.ClearCharacter, null, character);
        }

        // Token: 0x06000036 RID: 54 RVA: 0x00002EE8 File Offset: 0x000010E8
        private void DoTextTick()
        {
            var list = Matches.Where((TriggerMatchedEventArgs n) => (DateTime.Now - n.MatchedTime).TotalSeconds > TextFadeTime).ToList<TriggerMatchedEventArgs>();
            foreach (var triggerMatchedEventArgs in list)
            {
                RemoveMatch(triggerMatchedEventArgs, true);
            }
        }

        // Token: 0x06000037 RID: 55 RVA: 0x00002FB8 File Offset: 0x000011B8
        private void DoTimerTick()
        {
            lock (Matches)
            {
                foreach (var triggerMatchedEventArgs in Matches.ToList<TriggerMatchedEventArgs>())
                {
                    triggerMatchedEventArgs.DoTick();
                }
                var list = Matches.Where((TriggerMatchedEventArgs o) => o.TargetDateTime != null && DateTime.Now >= o.TargetDateTime.Value).ToList<TriggerMatchedEventArgs>();
                foreach (var triggerMatchedEventArgs2 in list.Where((TriggerMatchedEventArgs o) => o.TriggerFilter.Trigger.TimerType == TimerTypes.Timer))
                {
                    triggerMatchedEventArgs2.EndTimer();
                }
                foreach (var triggerMatchedEventArgs3 in list.Where((TriggerMatchedEventArgs o) => o.TriggerFilter.Trigger.TimerType == TimerTypes.RepeatingTimer))
                {
                    ModifyMatches(triggerMatchedEventArgs3, BehaviorGroup.CollectionOperation.RepeatTimer, null, null);
                }
            }
        }

        // Token: 0x06000038 RID: 56 RVA: 0x00003194 File Offset: 0x00001394
        private static Font GetFont(string fontName, float size)
        {
            if (string.IsNullOrWhiteSpace(fontName))
            {
                fontName = "Arial";
            }
            var fontFamily = global::System.Drawing.FontFamily.Families.FirstOrDefault((global::System.Drawing.FontFamily o) => o.Name == fontName);
            if (fontFamily == null && fontName != "Arial")
            {
                fontFamily = global::System.Drawing.FontFamily.Families.FirstOrDefault((global::System.Drawing.FontFamily o) => o.Name == "Arial");
            }
            if (fontFamily == null)
            {
                fontFamily = global::System.Drawing.FontFamily.Families.FirstOrDefault<global::System.Drawing.FontFamily>();
            }
            foreach (var fontStyle in new global::System.Drawing.FontStyle[]
            {
                global::System.Drawing.FontStyle.Regular,
                global::System.Drawing.FontStyle.Bold,
                global::System.Drawing.FontStyle.Italic
            })
            {
                if (fontFamily.IsStyleAvailable(fontStyle))
                {
                    return new Font(fontFamily, size, fontStyle);
                }
            }
            return new Font(global::System.Drawing.FontFamily.Families.First<global::System.Drawing.FontFamily>().Name, size);
        }

        // Token: 0x06000039 RID: 57 RVA: 0x00003284 File Offset: 0x00001484
        public static BehaviorGroup LoadFromXml(XmlElement element, bool loadToAll = true)
        {
            var behaviorGroup = new BehaviorGroup
            {
                BehaviorType = element.GetElementValue("BehaviorType", BehaviorTypes.Text),
                Name = element.GetElementValue("Name", null),
                FontName = element.GetElementValue("FontName", "Arial"),
                FontSize = element.GetElementValue("FontSize", 50),
                GroupByCharacter = element.GetElementValue("GroupByCharacter", true),
                SortMethod = element.GetElementValue("SortMethod", TimerSortMethod.OrderTriggered),
                TextFadeTime = element.GetElementValue("TextFadeTime", 10),
                ShowTimerBar = element.GetElementValue("ShowTimerBar", true),
                EmptyBarColor = default(global::System.Windows.Media.Color).SetFromHtml(element.GetElementValue("EmptyBarColor", "#D0000000"), "White"),
                StandardizeTimerBars = element.GetElementValue("StandardizeTimerBars", false),
                BackgroundColor = default(global::System.Windows.Media.Color).SetFromHtml(element.GetElementValue("BackgroundColor", "#00000000"), "White"),
                BackgroundFadedColor = default(global::System.Windows.Media.Color).SetFromHtml(element.GetElementValue("BackgroundFadedColor", "#00000000"), "White")
            };
            var xmlElement = element.GetElementsByTagName("WindowLayout").Cast<XmlElement>().FirstOrDefault<XmlElement>();
            behaviorGroup.WindowLayout = xmlElement?.InnerXml;
            if (loadToAll)
            {
                BehaviorGroup.All.Add(behaviorGroup);
            }
            return behaviorGroup;
        }

        // Token: 0x0600003A RID: 58 RVA: 0x000033F8 File Offset: 0x000015F8
        public void SaveToXml(XmlElement element)
        {
            var ownerDocument = element.OwnerDocument;
            var xmlElement = ownerDocument.CreateElement("Behavior");
            _ = element.AppendChild(xmlElement);
            _ = xmlElement.AppendChild(ownerDocument.NewElement("BehaviorType", BehaviorType));
            _ = xmlElement.AppendChild(ownerDocument.NewElement("Name", Name ?? ""));
            _ = xmlElement.AppendChild(ownerDocument.NewElement("FontName", FontName));
            _ = xmlElement.AppendChild(ownerDocument.NewElement("FontSize", FontSize));
            _ = xmlElement.AppendChild(ownerDocument.NewElement("GroupByCharacter", GroupByCharacter));
            _ = xmlElement.AppendChild(ownerDocument.NewElement("SortMethod", SortMethod));
            _ = xmlElement.AppendChild(ownerDocument.NewElement("TextFadeTime", TextFadeTime));
            _ = xmlElement.AppendChild(ownerDocument.NewElement("ShowTimerBar", ShowTimerBar));
            _ = xmlElement.AppendChild(ownerDocument.NewElement("EmptyBarColor", EmptyBarColor.ToHexString(true)));
            _ = xmlElement.AppendChild(ownerDocument.NewElement("StandardizeTimerBars", StandardizeTimerBars));
            _ = xmlElement.AppendChild(ownerDocument.NewElement("BackgroundColor", BackgroundColor.ToHexString(true)));
            _ = xmlElement.AppendChild(ownerDocument.NewElement("BackgroundFadedColor", BackgroundFadedColor.ToHexString(true)));
            if (WindowLayout != null)
            {
                var xmlDocumentFragment = element.OwnerDocument.CreateDocumentFragment();
                xmlDocumentFragment.InnerXml = WindowLayout;
                var xmlElement2 = ownerDocument.CreateElement("WindowLayout");
                _ = xmlElement2.AppendChild(xmlDocumentFragment);
                _ = xmlElement.AppendChild(xmlElement2);
            }
        }

        // Token: 0x04000001 RID: 1
        private static ObservableCollection<BehaviorGroup> _All;

        // Token: 0x04000002 RID: 2
        private readonly DispatcherTimer _Timer;

        // Token: 0x04000003 RID: 3
        private Font _Font;

        // Token: 0x02000004 RID: 4
        private enum CollectionOperation
        {
            // Token: 0x04000012 RID: 18
            Add,
            // Token: 0x04000013 RID: 19
            Remove,
            // Token: 0x04000014 RID: 20
            RestartTimer,
            // Token: 0x04000015 RID: 21
            RepeatTimer,
            // Token: 0x04000016 RID: 22
            AddSingleton,
            // Token: 0x04000017 RID: 23
            Clear,
            // Token: 0x04000018 RID: 24
            ClearCharacter
        }
    }
}
