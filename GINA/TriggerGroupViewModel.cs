using GimaSoft.Business.GINA;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using WPFShared;

namespace GimaSoft.GINA
{
    // Token: 0x02000023 RID: 35
    public class TriggerGroupViewModel : TriggerAndGroupViewModel
    {
        // Token: 0x0600038D RID: 909 RVA: 0x0000C258 File Offset: 0x0000A458
        public TriggerGroupViewModel(MainWindowViewModel mainVM, TriggerGroup triggerGroup, bool groupsOnly = false)
        {
            PropertyChangedEventHandler propertyChangedEventHandler = null;

            MainVM = mainVM;
            TriggerGroup = triggerGroup;
            GroupsOnly = groupsOnly;
            ChildGroups = triggerGroup.Groups.Select((TriggerGroup o) => new TriggerGroupViewModel(mainVM, o, GroupsOnly)).ToObservableCollection<TriggerGroupViewModel>();
            CollectionViewSource.GetDefaultView(ChildGroups).SortDescriptions.Add(new SortDescription("TriggerGroup.Name", ListSortDirection.Ascending));
            if (groupsOnly)
            {
                ChildTriggers = new ObservableCollection<TriggerViewModel>();
            }
            else
            {
                foreach (var triggerGroupViewModel in ChildGroups)
                {
                    BindableObject bindableObject = triggerGroupViewModel;
                    if (propertyChangedEventHandler == null)
                    {
                        propertyChangedEventHandler = delegate (object oi, PropertyChangedEventArgs ei)
                        {
                            if (ei.PropertyName == "IsActive")
                            {
                                base.RaisePropertyChanged("IsActive");
                            }
                        };
                    }
                    bindableObject.PropertyChanged += propertyChangedEventHandler;
                }
                ChildTriggers = triggerGroup.Triggers.Select((Trigger o) => new TriggerViewModel(this, o)).ToObservableCollection<TriggerViewModel>();
            }
            Children = new ObservableCollectionExt<TriggerAndGroupViewModel>();
            MergeChildren();
            if (MainVM != null)
            {
                MainVM.PropertyChanged += delegate (object o, PropertyChangedEventArgs e)
                {
                    if (e.PropertyName == "SelectedCharacters")
                    {
                        base.RaisePropertyChanged("IsActive");
                    }
                };
            }
            triggerGroup.Groups.CollectionChanged += Groups_CollectionChanged;
            if (!groupsOnly)
            {
                triggerGroup.Triggers.CollectionChanged += Triggers_CollectionChanged;
            }
        }

        // Token: 0x0600038E RID: 910 RVA: 0x0000C414 File Offset: 0x0000A614
        private void Groups_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var newStartingIndex = e.NewStartingIndex;
                foreach (var obj in e.NewItems)
                {
                    var triggerGroupViewModel = new TriggerGroupViewModel(MainVM, (TriggerGroup)obj, GroupsOnly);
                    triggerGroupViewModel.PropertyChanged += delegate (object oi, PropertyChangedEventArgs ei)
                    {
                        if (ei.PropertyName == "IsActive")
                        {
                            base.RaisePropertyChanged("IsActive");
                        }
                    };
                    ChildGroups.Insert(newStartingIndex++, triggerGroupViewModel);
                }
            }
            if (e.Action == NotifyCollectionChangedAction.Remove || e.Action == NotifyCollectionChangedAction.Reset)
            {
                List<TriggerGroupViewModel> list;
                if (e.Action != NotifyCollectionChangedAction.Remove)
                {
                    list = ChildGroups.ToList<TriggerGroupViewModel>();
                }
                else
                {
                    list = ChildGroups.Join(e.OldItems.Cast<TriggerGroup>(), (TriggerGroupViewModel o) => o.TriggerGroup, (TriggerGroup n) => n, (TriggerGroupViewModel n, TriggerGroup o) => n).ToList<TriggerGroupViewModel>();
                }
                var list2 = list;
                foreach (var triggerGroupViewModel2 in list2)
                {
                    _ = ChildGroups.Remove(triggerGroupViewModel2);
                }
            }
            MergeChildren();
        }

        // Token: 0x0600038F RID: 911 RVA: 0x0000C5C0 File Offset: 0x0000A7C0
        private void Triggers_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var newStartingIndex = e.NewStartingIndex;
                foreach (var obj in e.NewItems)
                {
                    ChildTriggers.Insert(newStartingIndex++, new TriggerViewModel(this, (Trigger)obj));
                }
            }
            if (e.Action == NotifyCollectionChangedAction.Remove || e.Action == NotifyCollectionChangedAction.Reset)
            {
                List<TriggerViewModel> list;
                if (e.Action != NotifyCollectionChangedAction.Remove)
                {
                    list = ChildTriggers.ToList<TriggerViewModel>();
                }
                else
                {
                    list = ChildTriggers.Join(e.OldItems.Cast<Trigger>(), (TriggerViewModel o) => o.Trigger, (Trigger n) => n, (TriggerViewModel n, Trigger o) => n).ToList<TriggerViewModel>();
                }
                var list2 = list;
                foreach (var triggerViewModel in list2)
                {
                    _ = ChildTriggers.Remove(triggerViewModel);
                }
            }
            MergeChildren();
        }

        // Token: 0x06000390 RID: 912 RVA: 0x0000C72C File Offset: 0x0000A92C
        private void RefreshChildSort()
        {
            CollectionViewSource.GetDefaultView(Children).Refresh();
        }

        // Token: 0x06000391 RID: 913 RVA: 0x0000C7E4 File Offset: 0x0000A9E4
        private void MergeChildren()
        {
            if (Children == null)
            {
                Children = new ObservableCollectionExt<TriggerAndGroupViewModel>();
            }
            var list = Children.Where((TriggerAndGroupViewModel o) => o is TriggerGroupViewModel && !ChildGroups.Contains((TriggerGroupViewModel)o)).Union(Children.Where((TriggerAndGroupViewModel o) => o is TriggerViewModel && !ChildTriggers.Contains((TriggerViewModel)o))).ToList<TriggerAndGroupViewModel>();
            foreach (var triggerAndGroupViewModel in list)
            {
                _ = Children.Remove(triggerAndGroupViewModel);
            }
            int j;
            for (j = 0; j < ChildGroups.Count; j++)
            {
                var triggerAndGroupViewModel2 = Children.Where((TriggerAndGroupViewModel o) => o is TriggerGroupViewModel && (TriggerGroupViewModel)o == ChildGroups[j]).SingleOrDefault<TriggerAndGroupViewModel>();
                if (triggerAndGroupViewModel2 == null)
                {
                    Children.Insert(j, ChildGroups[j]);
                }
                else
                {
                    var num = Children.IndexOf(triggerAndGroupViewModel2);
                    if (num != j)
                    {
                        Children.Move(num, j);
                    }
                }
            }
            var count = ChildGroups.Count;
            int i;
            for (i = 0; i < ChildTriggers.Count; i++)
            {
                var triggerAndGroupViewModel3 = Children.Where((TriggerAndGroupViewModel o) => o is TriggerViewModel && (TriggerViewModel)o == ChildTriggers[i]).SingleOrDefault<TriggerAndGroupViewModel>();
                if (triggerAndGroupViewModel3 == null)
                {
                    Children.Insert(i + count, ChildTriggers[i]);
                }
                else
                {
                    var num2 = Children.IndexOf(triggerAndGroupViewModel3);
                    if (num2 != i + count)
                    {
                        Children.Move(num2, i + count);
                    }
                }
            }
            base.RaisePropertyChanged("HasTreeChildren");
        }

        // Token: 0x17000154 RID: 340
        // (get) Token: 0x06000392 RID: 914 RVA: 0x0000CA28 File Offset: 0x0000AC28
        // (set) Token: 0x06000393 RID: 915 RVA: 0x0000CA35 File Offset: 0x0000AC35
        public MainWindowViewModel MainVM
        {
            get => base.Get<MainWindowViewModel>("MainVM"); set => base.Set("MainVM", value);
        }

        // Token: 0x17000155 RID: 341
        // (get) Token: 0x06000394 RID: 916 RVA: 0x0000CA43 File Offset: 0x0000AC43
        // (set) Token: 0x06000395 RID: 917 RVA: 0x0000CA50 File Offset: 0x0000AC50
        public TriggerGroup TriggerGroup
        {
            get => base.Get<TriggerGroup>("TriggerGroup"); set => base.Set("TriggerGroup", value);
        }

        // Token: 0x17000156 RID: 342
        // (get) Token: 0x06000396 RID: 918 RVA: 0x0000CA5E File Offset: 0x0000AC5E
        // (set) Token: 0x06000397 RID: 919 RVA: 0x0000CA6B File Offset: 0x0000AC6B
        public ObservableCollection<TriggerGroupViewModel> ChildGroups
        {
            get => base.Get<ObservableCollection<TriggerGroupViewModel>>("ChildGroups"); set => base.Set("ChildGroups", value);
        }

        // Token: 0x17000157 RID: 343
        // (get) Token: 0x06000398 RID: 920 RVA: 0x0000CA79 File Offset: 0x0000AC79
        // (set) Token: 0x06000399 RID: 921 RVA: 0x0000CA86 File Offset: 0x0000AC86
        public ObservableCollection<TriggerViewModel> ChildTriggers
        {
            get => base.Get<ObservableCollection<TriggerViewModel>>("ChildTriggers"); set => base.Set("ChildTriggers", value);
        }

        // Token: 0x17000158 RID: 344
        // (get) Token: 0x0600039A RID: 922 RVA: 0x0000CA94 File Offset: 0x0000AC94
        // (set) Token: 0x0600039B RID: 923 RVA: 0x0000CAA1 File Offset: 0x0000ACA1
        public ObservableCollectionExt<TriggerAndGroupViewModel> Children
        {
            get => base.Get<ObservableCollectionExt<TriggerAndGroupViewModel>>("Children"); set => base.Set("Children", value);
        }

        // Token: 0x17000159 RID: 345
        // (get) Token: 0x0600039C RID: 924 RVA: 0x0000CAAF File Offset: 0x0000ACAF
        public override INotifyCollectionChanged TreeChildren => Children;

        // Token: 0x1700015A RID: 346
        // (get) Token: 0x0600039D RID: 925 RVA: 0x0000CAB7 File Offset: 0x0000ACB7
        public override bool HasTreeChildren => Children != null && Children.Any<TriggerAndGroupViewModel>();

        // Token: 0x1700015B RID: 347
        // (get) Token: 0x0600039E RID: 926 RVA: 0x0000CACE File Offset: 0x0000ACCE
        public override string DisplayName => TriggerGroup.Name;

        // Token: 0x1700015C RID: 348
        // (get) Token: 0x0600039F RID: 927 RVA: 0x0000CAE4 File Offset: 0x0000ACE4
        public IEnumerable<TriggerGroupViewModel> DescendantTree => new List<TriggerGroupViewModel> { this }.Union(ChildGroups.SelectMany((TriggerGroupViewModel o) => o.DescendantTree).ToList<TriggerGroupViewModel>());

        // Token: 0x1700015D RID: 349
        // (get) Token: 0x060003A0 RID: 928 RVA: 0x0000CC04 File Offset: 0x0000AE04
        // (set) Token: 0x060003A1 RID: 929 RVA: 0x0000CCEC File Offset: 0x0000AEEC
        public bool? IsActive
        {
            get
            {
                if (MainVM == null)
                {
                    return new bool?(false);
                }
                var descendantTree = DescendantTree;
                var num = (from o in MainVM.SelectedCharacters.SelectMany((GINACharacter o) => o.TriggerGroups)
                           join n in descendantTree on o equals n.TriggerGroup
                           select new
                           {
                               Group = o
                           }).Count();
                if (num == 0)
                {
                    return new bool?(false);
                }
                if (num == MainVM.SelectedCharacters.Count<GINACharacter>() * descendantTree.Count<TriggerGroupViewModel>())
                {
                    return new bool?(true);
                }
                return null;
            }
            set
            {
                if (value == null || MainVM == null)
                {
                    return;
                }
                if (value.Value)
                {
                    foreach (var ginacharacter in MainVM.SelectedCharacters)
                    {
                        ginacharacter.AddTriggerGroup(TriggerGroup);
                    }
                    using (var enumerator2 = ChildGroups.GetEnumerator())
                    {
                        while (enumerator2.MoveNext())
                        {
                            var triggerGroupViewModel = enumerator2.Current;
                            triggerGroupViewModel.IsActive = new bool?(true);
                        }
                        goto IL_0110;
                    }
                }
                foreach (var ginacharacter2 in MainVM.SelectedCharacters)
                {
                    ginacharacter2.RemoveTriggerGroup(TriggerGroup);
                }
                foreach (var triggerGroupViewModel2 in ChildGroups)
                {
                    triggerGroupViewModel2.IsActive = new bool?(false);
                }
            IL_0110:
                base.RaisePropertyChanged("IsActive");
            }
        }

        // Token: 0x1700015E RID: 350
        // (get) Token: 0x060003A2 RID: 930 RVA: 0x0000CE48 File Offset: 0x0000B048
        public override bool IsTriggerGroupView => true;

        // Token: 0x1700015F RID: 351
        // (get) Token: 0x060003A3 RID: 931 RVA: 0x0000CE4B File Offset: 0x0000B04B
        // (set) Token: 0x060003A4 RID: 932 RVA: 0x0000CE53 File Offset: 0x0000B053
        private bool GroupsOnly { get; set; }

        // Token: 0x060003A5 RID: 933 RVA: 0x0000CE5C File Offset: 0x0000B05C
        public override void Dispose(bool disposing)
        {
            if (!_Disposed)
            {
                _Disposed = true;
                if (disposing)
                {
                    foreach (var triggerViewModel in ChildTriggers)
                    {
                        triggerViewModel.Dispose();
                    }
                    foreach (var triggerGroupViewModel in ChildGroups)
                    {
                        triggerGroupViewModel.Dispose();
                    }
                }
            }
        }
    }
}
