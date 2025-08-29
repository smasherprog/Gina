﻿using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Xml;
using WPFShared;

namespace GimaSoft.Business.GINA
{
    // Token: 0x0200003E RID: 62
    public class TriggerCategory : BindableObject
    {
        // Token: 0x17000134 RID: 308
        // (get) Token: 0x0600034B RID: 843 RVA: 0x0000FE04 File Offset: 0x0000E004
        public static ObservableCollection<TriggerCategory> All
        {
            get
            {
                if (TriggerCategory._All == null)
                {
                    TriggerCategory._All = new ObservableCollection<TriggerCategory>();
                    TriggerCategory._All.CollectionChanged += delegate (object o, NotifyCollectionChangedEventArgs e)
                    {
                        TriggerCategory.EnsureDefaultExists();
                        if (e.Action == NotifyCollectionChangedAction.Remove)
                        {
                            using (var enumerator = e.OldItems.Cast<TriggerCategory>().GetEnumerator())
                            {
                                while (enumerator.MoveNext())
                                {
                                    var category = enumerator.Current;
                                    foreach (var trigger in TriggerGroup.RootGroup.DescendantTree.SelectMany((TriggerGroup m) => m.Triggers.Where((Trigger n) => n.Category == category)).ToList<Trigger>())
                                    {
                                        trigger.Category = TriggerCategory.DefaultCategory;
                                    }
                                    foreach (var ginacharacter in GINACharacter.All)
                                    {
                                        foreach (var characterCategory in ginacharacter.Categories.Where((CharacterCategory n) => n.Category == category).ToList<CharacterCategory>())
                                        {
                                            _ = ginacharacter.Categories.Remove(characterCategory);
                                        }
                                    }
                                }
                                goto IL_01F5;
                            }
                        }
                        if (e.Action == NotifyCollectionChangedAction.Add)
                        {
                            foreach (var triggerCategory in e.NewItems.Cast<TriggerCategory>())
                            {
                                triggerCategory.PropertyChanged += delegate (object m, PropertyChangedEventArgs n)
                                {
                                    if (n.PropertyName == "Name")
                                    {
                                        TriggerCategory.RefreshView();
                                    }
                                };
                                foreach (var ginacharacter2 in GINACharacter.All)
                                {
                                    ginacharacter2.Categories.Add(new CharacterCategory
                                    {
                                        Category = triggerCategory
                                    });
                                }
                            }
                        }
                    IL_01F5:
                        TriggerCategory.EnsureDefaultExists();
                    };
                    var defaultView = CollectionViewSource.GetDefaultView(TriggerCategory._All);
                    defaultView.SortDescriptions.Clear();
                    defaultView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                }
                return TriggerCategory._All;
            }
        }

        // Token: 0x0600034C RID: 844 RVA: 0x0000FE84 File Offset: 0x0000E084
        private static void EnsureDefaultExists()
        {
            if (TriggerCategory.All.Any<TriggerCategory>())
            {
                if (!TriggerCategory.All.Any((TriggerCategory n) => n.IsDefault))
                {
                    TriggerCategory.All.First<TriggerCategory>().IsDefault = true;
                }
            }
        }

        // Token: 0x0600034D RID: 845 RVA: 0x0000FED8 File Offset: 0x0000E0D8
        public static void Clear()
        {
            var list = TriggerCategory.All.ToList<TriggerCategory>();
            TriggerCategory.All.Clear();
            foreach (var triggerCategory in list)
            {
                triggerCategory.Dispose();
            }
        }

        // Token: 0x0600034E RID: 846 RVA: 0x0000FF3C File Offset: 0x0000E13C
        public static void RefreshView()
        {
            CollectionViewSource.GetDefaultView(TriggerCategory.All).Refresh();
        }

        // Token: 0x17000135 RID: 309
        // (get) Token: 0x0600034F RID: 847 RVA: 0x0000FF55 File Offset: 0x0000E155
        public static TriggerCategory DefaultCategory => TriggerCategory.All.SingleOrDefault((TriggerCategory o) => o.IsDefault);

        // Token: 0x06000350 RID: 848 RVA: 0x0000FF7E File Offset: 0x0000E17E
        public static void Init()
        {
            if (!TriggerCategory.All.Any<TriggerCategory>())
            {
                _ = TriggerCategory.Create("Default", null, null, true);
            }
        }

        // Token: 0x06000351 RID: 849 RVA: 0x0000FFC8 File Offset: 0x0000E1C8
        public static TriggerCategory Find(string name, bool createIfMissing = false)
        {
            var triggerCategory = TriggerCategory.All.SingleOrDefault((TriggerCategory o) => o.Name.ToLower() == (name ?? "").ToLower());
            if (triggerCategory == null)
            {
                triggerCategory = TriggerCategory.Create(name, null, null, false);
            }
            return triggerCategory;
        }

        // Token: 0x06000352 RID: 850 RVA: 0x00010034 File Offset: 0x0000E234
        public static string GetNextName(string baseName = "New Category")
        {
            var newName = baseName;
            var num = 1;
            while (TriggerCategory.All.Any((TriggerCategory o) => o.Name.ToLower() == newName.ToLower()))
            {
                newName = string.Format(baseName + " #{0}", num++);
            }
            return newName;
        }

        // Token: 0x06000353 RID: 851 RVA: 0x000100FC File Offset: 0x0000E2FC
        public static TriggerCategory Create(string name = null, string textBehaviorName = null, string timerBehaviorName = null, bool isDefault = false)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                name = TriggerCategory.GetNextName("New Category");
            }
            var triggerCategory = new TriggerCategory
            {
                Name = name,
                IsDefault = isDefault
            };
            var behaviorGroup = BehaviorGroup.All.SingleOrDefault((BehaviorGroup o) => o.BehaviorType == BehaviorTypes.Text && o.Name.ToLower() == (textBehaviorName ?? "").ToLower());
            triggerCategory.TextOverlay = behaviorGroup ?? BehaviorGroup.DefaultTextGroup;
            var behaviorGroup2 = BehaviorGroup.All.SingleOrDefault((BehaviorGroup o) => o.BehaviorType == BehaviorTypes.Timer && o.Name.ToLower() == (timerBehaviorName ?? "").ToLower());
            triggerCategory.TimerOverlay = behaviorGroup2 ?? BehaviorGroup.DefaultTimerGroup;
            TriggerCategory.All.Add(triggerCategory);
            return triggerCategory;
        }

        // Token: 0x06000354 RID: 852 RVA: 0x000101A8 File Offset: 0x0000E3A8
        public TriggerCategory()
        {
            TextOverlay = BehaviorGroup.DefaultTextGroup;
            TextStyle = new BehaviorStyle();
            TextStyleSource = InheritanceSources.None;
            TimerOverlay = BehaviorGroup.DefaultTimerGroup;
            TimerStyle = new BehaviorStyle();
            TimerStyleSource = InheritanceSources.None;
        }

        // Token: 0x17000136 RID: 310
        // (get) Token: 0x06000355 RID: 853 RVA: 0x000101F5 File Offset: 0x0000E3F5
        // (set) Token: 0x06000356 RID: 854 RVA: 0x00010228 File Offset: 0x0000E428
        public string Name
        {
            get => base.Get<string>("Name");
            set
            {
                var triggerCategory = TriggerCategory.All.SingleOrDefault((TriggerCategory o) => o.Name.ToLower() == value.ToLower());
                if (triggerCategory != null && triggerCategory != this)
                {
                    base.RaisePropertyChanged("Name");
                    return;
                }
                if (string.IsNullOrWhiteSpace(value) && !string.IsNullOrWhiteSpace(Name))
                {
                    base.RaisePropertyChanged("Name");
                    return;
                }
                base.Set("Name", value);
            }
        }

        // Token: 0x17000137 RID: 311
        // (get) Token: 0x06000357 RID: 855 RVA: 0x000102A3 File Offset: 0x0000E4A3
        // (set) Token: 0x06000358 RID: 856 RVA: 0x000102B0 File Offset: 0x0000E4B0
        public BehaviorGroup TextOverlay
        {
            get => base.Get<BehaviorGroup>("TextOverlay"); set => base.Set("TextOverlay", value);
        }

        // Token: 0x17000138 RID: 312
        // (get) Token: 0x06000359 RID: 857 RVA: 0x000102BE File Offset: 0x0000E4BE
        // (set) Token: 0x0600035A RID: 858 RVA: 0x000102CB File Offset: 0x0000E4CB
        public BehaviorStyle TextStyle
        {
            get => base.Get<BehaviorStyle>("TextStyle"); set => base.Set("TextStyle", value);
        }

        // Token: 0x17000139 RID: 313
        // (get) Token: 0x0600035B RID: 859 RVA: 0x000102D9 File Offset: 0x0000E4D9
        // (set) Token: 0x0600035C RID: 860 RVA: 0x000102E6 File Offset: 0x0000E4E6
        public InheritanceSources TextStyleSource
        {
            get => base.Get<InheritanceSources>("TextStyleSource"); set => base.Set("TextStyleSource", value);
        }

        // Token: 0x1700013A RID: 314
        // (get) Token: 0x0600035D RID: 861 RVA: 0x000102F9 File Offset: 0x0000E4F9
        // (set) Token: 0x0600035E RID: 862 RVA: 0x00010306 File Offset: 0x0000E506
        public BehaviorGroup TimerOverlay
        {
            get => base.Get<BehaviorGroup>("TimerOverlay"); set => base.Set("TimerOverlay", value);
        }

        // Token: 0x1700013B RID: 315
        // (get) Token: 0x0600035F RID: 863 RVA: 0x00010314 File Offset: 0x0000E514
        // (set) Token: 0x06000360 RID: 864 RVA: 0x00010321 File Offset: 0x0000E521
        public BehaviorStyle TimerStyle
        {
            get => base.Get<BehaviorStyle>("TimerStyle"); set => base.Set("TimerStyle", value);
        }

        // Token: 0x1700013C RID: 316
        // (get) Token: 0x06000361 RID: 865 RVA: 0x0001032F File Offset: 0x0000E52F
        // (set) Token: 0x06000362 RID: 866 RVA: 0x0001033C File Offset: 0x0000E53C
        public InheritanceSources TimerStyleSource
        {
            get => base.Get<InheritanceSources>("TimerStyleSource"); set => base.Set("TimerStyleSource", value);
        }

        // Token: 0x1700013D RID: 317
        // (get) Token: 0x06000363 RID: 867 RVA: 0x0001034F File Offset: 0x0000E54F
        // (set) Token: 0x06000364 RID: 868 RVA: 0x0001036C File Offset: 0x0000E56C
        public bool IsDefault
        {
            get => base.Get<bool>("IsDefault");
            set
            {
                base.Set("IsDefault", value);
                if (value)
                {
                    var list = TriggerCategory.All.Where((TriggerCategory o) => o != this && o.IsDefault).ToList<TriggerCategory>();
                    foreach (var triggerCategory in list)
                    {
                        triggerCategory.IsDefault = false;
                    }
                }
            }
        }

        // Token: 0x06000365 RID: 869 RVA: 0x000103F4 File Offset: 0x0000E5F4
        public static TriggerCategory LoadFromXml(XmlElement element)
        {
            var triggerCategory = TriggerCategory.Create(element.GetElementValue<string>("Name", null), element.GetElementValue("TextOverlay", element.GetElementValue<string>("TextBehavior", null)), element.GetElementValue("TimerOverlay", element.GetElementValue<string>("TimerBehavior", null)), false);
            triggerCategory.TextStyleSource = element.GetElementValue("TextStyleSource", InheritanceSources.FromCharacter);
            triggerCategory.TextStyle.LoadFromXml(element.SelectSingleNode("TextStyle[1]") as XmlElement);
            triggerCategory.TimerStyleSource = element.GetElementValue("TimerStyleSource", InheritanceSources.FromCharacter);
            triggerCategory.TimerStyle.LoadFromXml(element.SelectSingleNode("TimerStyle[1]") as XmlElement);
            return triggerCategory;
        }

        // Token: 0x06000366 RID: 870 RVA: 0x000104A0 File Offset: 0x0000E6A0
        public void SaveToXml(XmlElement element)
        {
            var ownerDocument = element.OwnerDocument;
            var xmlElement = ownerDocument.CreateElement("Category");
            _ = element.AppendChild(xmlElement);
            _ = xmlElement.AppendChild(ownerDocument.NewElement("Name", Name ?? ""));
            _ = xmlElement.AppendChild(ownerDocument.NewElement("TextOverlay", (TextOverlay != null) ? TextOverlay.Name : BehaviorGroup.DefaultTextGroup.Name));
            _ = xmlElement.AppendChild(ownerDocument.NewElement("TextStyleSource", TextStyleSource));
            _ = xmlElement.AppendChild(TextStyle.SaveToXml(ownerDocument.CreateElement("TextStyle")));
            _ = xmlElement.AppendChild(ownerDocument.NewElement("TimerOverlay", (TimerOverlay != null) ? TimerOverlay.Name : BehaviorGroup.DefaultTimerGroup.Name));
            _ = xmlElement.AppendChild(ownerDocument.NewElement("TimerStyleSource", TimerStyleSource));
            _ = xmlElement.AppendChild(TimerStyle.SaveToXml(ownerDocument.CreateElement("TimerStyle")));
            _ = xmlElement.AppendChild(ownerDocument.NewElement("IsDefault", IsDefault));
        }

        // Token: 0x0400014A RID: 330
        private static ObservableCollection<TriggerCategory> _All;
    }
}
