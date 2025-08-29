using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using WPFShared;

namespace GimaSoft.Business.GINA
{
    // Token: 0x0200003F RID: 63
    public class TriggerFilter : BindableObject
    {
        // Token: 0x0600036C RID: 876 RVA: 0x000105DC File Offset: 0x0000E7DC
        static TriggerFilter()
        {
            BindableObject.SetDependentProperties(typeof(TriggerFilter));
        }

        // Token: 0x0600036D RID: 877 RVA: 0x000105F0 File Offset: 0x0000E7F0
        public TriggerFilter()
        {
        }

        // Token: 0x0600036E RID: 878 RVA: 0x0001071C File Offset: 0x0000E91C
        public TriggerFilter(GINACharacter character, Trigger trigger)
        {
            Matcher = new MatchHelper(character, trigger.TriggerText, trigger.EnableRegex, trigger.UseFastCheck);
            Character = character;
            Trigger = trigger;
            _LastGroup[BehaviorTypes.Text] = GetEffectiveOverlay(BehaviorTypes.Text);
            _LastGroup[BehaviorTypes.Timer] = GetEffectiveOverlay(BehaviorTypes.Timer);
            foreach (var characterCategory in Character.Categories)
            {
                characterCategory.PropertyChanged += CharacterCategory_PropertyChanged;
            }
            Character.Categories.CollectionChanged += delegate (object o, NotifyCollectionChangedEventArgs e)
            {
                if (e.NewItems != null)
                {
                    foreach (var obj in e.NewItems)
                    {
                        ((CharacterCategory)obj).PropertyChanged += CharacterCategory_PropertyChanged;
                    }
                }
            };
            Trigger.PropertyChanged += delegate (object o, PropertyChangedEventArgs e)
            {
                if (e.PropertyName == "Category")
                {
                    Trigger.Category.PropertyChanged += TriggerCategory_PropertyChanged;
                    AdjustOverlay(BehaviorTypes.Text);
                    base.RaisePropertyChanged("TextOverlay");
                    base.RaisePropertyChanged("TextStyle");
                    AdjustOverlay(BehaviorTypes.Timer);
                    base.RaisePropertyChanged("TimerOverlay");
                    base.RaisePropertyChanged("TimerStyle");
                }
            };
            Trigger.Category.PropertyChanged += TriggerCategory_PropertyChanged;
        }

        // Token: 0x1700013E RID: 318
        // (get) Token: 0x0600036F RID: 879 RVA: 0x00010868 File Offset: 0x0000EA68
        // (set) Token: 0x06000370 RID: 880 RVA: 0x00010875 File Offset: 0x0000EA75
        public GINACharacter Character
        {
            get => base.Get<GINACharacter>("Character");
            set
            {
                if (value == Character)
                {
                    return;
                }
                base.Set("Character", value);
                if (value != null)
                {
                    value.PropertyChanged += RegenerateFilter;
                }
                RegenerateFilter(null, null);
            }
        }

        // Token: 0x06000371 RID: 881 RVA: 0x000108AA File Offset: 0x0000EAAA
        private void RegenerateFilter(object sender, PropertyChangedEventArgs e)
        {
            if (Trigger != null)
            {
                Matcher.GenerateFilter(Trigger.TriggerText, Trigger.EnableRegex, Trigger.UseFastCheck);
            }
        }

        // Token: 0x1700013F RID: 319
        // (get) Token: 0x06000372 RID: 882 RVA: 0x000108E0 File Offset: 0x0000EAE0
        // (set) Token: 0x06000373 RID: 883 RVA: 0x000108ED File Offset: 0x0000EAED
        public Trigger Trigger
        {
            get => base.Get<Trigger>("Trigger");
            set
            {
                if (value == Trigger)
                {
                    return;
                }
                base.Set("Trigger", value);
                if (value != null)
                {
                    value.PropertyChanged += RegenerateFilter;
                }
                RegenerateFilter(null, null);
            }
        }

        // Token: 0x17000140 RID: 320
        // (get) Token: 0x06000374 RID: 884 RVA: 0x00010922 File Offset: 0x0000EB22
        // (set) Token: 0x06000375 RID: 885 RVA: 0x0001092A File Offset: 0x0000EB2A
        public int Matches { get; set; }

        // Token: 0x17000141 RID: 321
        // (get) Token: 0x06000376 RID: 886 RVA: 0x00010933 File Offset: 0x0000EB33
        public BehaviorStyle TextStyle => GetEffectiveStyle(BehaviorTypes.Text);

        // Token: 0x17000142 RID: 322
        // (get) Token: 0x06000377 RID: 887 RVA: 0x0001093C File Offset: 0x0000EB3C
        public BehaviorGroup TextOverlay => GetEffectiveOverlay(BehaviorTypes.Text);

        // Token: 0x17000143 RID: 323
        // (get) Token: 0x06000378 RID: 888 RVA: 0x00010945 File Offset: 0x0000EB45
        public BehaviorStyle TimerStyle => GetEffectiveStyle(BehaviorTypes.Timer);

        // Token: 0x17000144 RID: 324
        // (get) Token: 0x06000379 RID: 889 RVA: 0x0001094E File Offset: 0x0000EB4E
        public BehaviorGroup TimerOverlay => GetEffectiveOverlay(BehaviorTypes.Timer);

        // Token: 0x0600037A RID: 890 RVA: 0x00010958 File Offset: 0x0000EB58
        private void CharacterCategory_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (_ = e.PropertyName)
            {
                case "TextOverlay":
                    AdjustOverlay(BehaviorTypes.Text);
                    base.RaisePropertyChanged("TextOverlay");
                    return;
                case "TextStyleSource":
                case "TextStyle":
                    base.RaisePropertyChanged("TextStyle");
                    return;
                case "TimerOverlay":
                    AdjustOverlay(BehaviorTypes.Timer);
                    base.RaisePropertyChanged("TimerOverlay");
                    return;
                case "TimerStyleSource":
                case "TimerStyle":
                    base.RaisePropertyChanged("TimerStyle");
                    break;
            }
        }

        // Token: 0x0600037B RID: 891 RVA: 0x00010A40 File Offset: 0x0000EC40
        private void TriggerCategory_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (_ = e.PropertyName)
            {
                case "TextOverlay":
                    AdjustOverlay(BehaviorTypes.Text);
                    base.RaisePropertyChanged("TextOverlay");
                    return;
                case "TextStyleSource":
                case "TextStyle":
                    base.RaisePropertyChanged("TextStyle");
                    return;
                case "TimerOverlay":
                    AdjustOverlay(BehaviorTypes.Timer);
                    base.RaisePropertyChanged("TimerOverlay");
                    return;
                case "TimerStyleSource":
                case "TimerStyle":
                    base.RaisePropertyChanged("TimerStyle");
                    break;

                    return;
            }
        }

        // Token: 0x0600037C RID: 892 RVA: 0x00010B3C File Offset: 0x0000ED3C
        private BehaviorStyle GetEffectiveStyle(BehaviorTypes type)
        {
            if (Trigger == null || Trigger.Category == null)
            {
                return new BehaviorStyle();
            }
            var flag = type == BehaviorTypes.Text;
            var characterCategory = Character?.Categories.SingleOrDefault((CharacterCategory o) => o.Category == Trigger.Category);
            return GetInheritanceObject<BehaviorStyle>(flag ? Trigger.Category.TextStyleSource : Trigger.Category.TimerStyleSource, (characterCategory == null) ? InheritanceSources.FromCategory : (flag ? characterCategory.TextStyleSource : characterCategory.TimerStyleSource), (Character == null) ? null : (flag ? Character.TextStyle : Character.TimerStyle), flag ? Trigger.Category.TextStyle : Trigger.Category.TimerStyle, (characterCategory == null) ? null : (flag ? characterCategory.TextStyle : characterCategory.TimerStyle));
        }

        // Token: 0x0600037D RID: 893 RVA: 0x00010C4C File Offset: 0x0000EE4C
        private BehaviorGroup GetEffectiveOverlay(BehaviorTypes type)
        {
            if (Trigger == null || Trigger.Category == null)
            {
                return new BehaviorGroup();
            }
            var flag = type == BehaviorTypes.Text;
            var characterCategory = Character?.Categories.SingleOrDefault((CharacterCategory o) => o.Category == Trigger.Category);
            return GetInheritanceObject<BehaviorGroup>(InheritanceSources.FromCategory, (characterCategory == null) ? InheritanceSources.FromCategory : (flag ? characterCategory.TextStyleSource : characterCategory.TimerOverlaySource), null, flag ? Trigger.Category.TextOverlay : Trigger.Category.TimerOverlay, (characterCategory == null) ? null : (flag ? characterCategory.TextOverlay : characterCategory.TimerOverlay));
        }

        // Token: 0x0600037E RID: 894 RVA: 0x00010D08 File Offset: 0x0000EF08
        private void AdjustOverlay(BehaviorTypes type)
        {
            var effectiveOverlay = GetEffectiveOverlay(type);
            var behaviorGroup = _LastGroup[type];
            _LastGroup[type] = effectiveOverlay;
            if (behaviorGroup != effectiveOverlay)
            {
                var list = new List<TriggerMatchedEventArgs>();
                if (behaviorGroup != null)
                {
                    list.AddRange(behaviorGroup.Matches.Where((TriggerMatchedEventArgs o) => o.TriggerFilter == this).ToList<TriggerMatchedEventArgs>());
                    foreach (var triggerMatchedEventArgs in list)
                    {
                        behaviorGroup.RemoveMatch(triggerMatchedEventArgs, false);
                    }
                }
                if (effectiveOverlay != null)
                {
                    foreach (var triggerMatchedEventArgs2 in list)
                    {
                        effectiveOverlay.AddMatch(triggerMatchedEventArgs2, false);
                    }
                }
            }
        }

        // Token: 0x0600037F RID: 895 RVA: 0x00010DFC File Offset: 0x0000EFFC
        private T GetInheritanceObject<T>(InheritanceSources categorySource, InheritanceSources characterCategorySource, T characterObject, T categoryObject, T characterCategoryObject)
        {
            if (characterCategorySource == InheritanceSources.None && characterCategoryObject != null)
            {
                return characterCategoryObject;
            }
            if ((characterCategorySource == InheritanceSources.FromCharacter || categorySource == InheritanceSources.FromCharacter) && characterObject != null)
            {
                return characterObject;
            }
            return categoryObject;
        }

        // Token: 0x06000380 RID: 896 RVA: 0x00010E24 File Offset: 0x0000F024
        internal void ResetMatchCount()
        {
            lock (_MatchesLockObject)
            {
                Matches = 0;
            }
        }

        // Token: 0x06000381 RID: 897 RVA: 0x00010E68 File Offset: 0x0000F068
        public bool IsMatch(string str)
        {
            var flag = Matcher.IsMatch(str);
            if (flag)
            {
                lock (_MatchesLockObject)
                {
                    if (LastMatched != null && Trigger.UseCounterResetTimer && (DateTime.Now - LastMatched.Value).TotalSeconds > Trigger.CounterResetDuration)
                    {
                        Matches = 1;
                    }
                    else
                    {
                        Matches++;
                    }
                    LastMatched = new DateTime?(DateTime.Now);
                }
            }
            return flag;
        }

        // Token: 0x06000382 RID: 898 RVA: 0x00010F24 File Offset: 0x0000F124
        public string ResolveText(string srcLine, string destLine, bool isVoice = false)
        {
            return Matcher.ResolveText(srcLine, destLine, isVoice);
        }

        // Token: 0x06000383 RID: 899 RVA: 0x00010F34 File Offset: 0x0000F134
        public long? ResolveTimerSpan(string srcLine)
        {
            return Matcher.ResolveTimerSpanToMilliseconds(srcLine);
        }

        // Token: 0x06000384 RID: 900 RVA: 0x00010F44 File Offset: 0x0000F144
        public override void Dispose(bool disposing)
        {
            if (!_Disposed)
            {
                _Disposed = true;
                if (disposing)
                {
                    if (Trigger != null)
                    {
                        Trigger.PropertyChanged -= RegenerateFilter;
                    }
                    if (Character != null)
                    {
                        Character.PropertyChanged -= RegenerateFilter;
                    }
                }
            }
        }

        // Token: 0x0400014F RID: 335
        private readonly object _MatchesLockObject = new object();

        // Token: 0x04000150 RID: 336
        private DateTime? LastMatched = null;

        // Token: 0x04000151 RID: 337
        private readonly MatchHelper Matcher;

        // Token: 0x04000152 RID: 338
        private readonly Dictionary<BehaviorTypes, BehaviorGroup> _LastGroup = new Dictionary<BehaviorTypes, BehaviorGroup>
        {
            {
                BehaviorTypes.Text,
                null
            },
            {
                BehaviorTypes.Timer,
                null
            }
        };
    }
}
