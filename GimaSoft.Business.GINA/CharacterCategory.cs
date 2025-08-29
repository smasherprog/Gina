using System;
using WPFShared;

namespace GimaSoft.Business.GINA
{
	// Token: 0x02000006 RID: 6
	public class CharacterCategory : BindableObject
	{
		// Token: 0x06000054 RID: 84 RVA: 0x000037A4 File Offset: 0x000019A4
		public CharacterCategory()
		{
			this.TextOverlaySource = InheritanceSources.FromCategory;
			this.TextOverlay = BehaviorGroup.DefaultTextGroup;
			this.TextStyleSource = InheritanceSources.FromCategory;
			this.TextStyle = new BehaviorStyle();
			this.TimerOverlaySource = InheritanceSources.FromCategory;
			this.TimerOverlay = BehaviorGroup.DefaultTimerGroup;
			this.TimerStyleSource = InheritanceSources.FromCategory;
			this.TimerStyle = new BehaviorStyle();
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000055 RID: 85 RVA: 0x000037FF File Offset: 0x000019FF
		// (set) Token: 0x06000056 RID: 86 RVA: 0x00003807 File Offset: 0x00001A07
		public TriggerCategory Category { get; set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000057 RID: 87 RVA: 0x00003810 File Offset: 0x00001A10
		// (set) Token: 0x06000058 RID: 88 RVA: 0x0000381D File Offset: 0x00001A1D
		public InheritanceSources TextOverlaySource
		{
			get
			{
				return base.Get<InheritanceSources>("TextOverlaySource");
			}
			set
			{
				base.Set("TextOverlaySource", value);
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000059 RID: 89 RVA: 0x00003830 File Offset: 0x00001A30
		// (set) Token: 0x0600005A RID: 90 RVA: 0x0000383D File Offset: 0x00001A3D
		public BehaviorGroup TextOverlay
		{
			get
			{
				return base.Get<BehaviorGroup>("TextOverlay");
			}
			set
			{
				base.Set("TextOverlay", value);
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600005B RID: 91 RVA: 0x0000384B File Offset: 0x00001A4B
		// (set) Token: 0x0600005C RID: 92 RVA: 0x00003858 File Offset: 0x00001A58
		public InheritanceSources TextStyleSource
		{
			get
			{
				return base.Get<InheritanceSources>("TextStyleSource");
			}
			set
			{
				base.Set("TextStyleSource", value);
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600005D RID: 93 RVA: 0x0000386B File Offset: 0x00001A6B
		// (set) Token: 0x0600005E RID: 94 RVA: 0x00003878 File Offset: 0x00001A78
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

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600005F RID: 95 RVA: 0x00003886 File Offset: 0x00001A86
		// (set) Token: 0x06000060 RID: 96 RVA: 0x00003893 File Offset: 0x00001A93
		public InheritanceSources TimerOverlaySource
		{
			get
			{
				return base.Get<InheritanceSources>("TimerOverlaySource");
			}
			set
			{
				base.Set("TimerOverlaySource", value);
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000061 RID: 97 RVA: 0x000038A6 File Offset: 0x00001AA6
		// (set) Token: 0x06000062 RID: 98 RVA: 0x000038B3 File Offset: 0x00001AB3
		public BehaviorGroup TimerOverlay
		{
			get
			{
				return base.Get<BehaviorGroup>("TimerOverlay");
			}
			set
			{
				base.Set("TimerOverlay", value);
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000063 RID: 99 RVA: 0x000038C1 File Offset: 0x00001AC1
		// (set) Token: 0x06000064 RID: 100 RVA: 0x000038CE File Offset: 0x00001ACE
		public InheritanceSources TimerStyleSource
		{
			get
			{
				return base.Get<InheritanceSources>("TimerStyleSource");
			}
			set
			{
				base.Set("TimerStyleSource", value);
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000065 RID: 101 RVA: 0x000038E1 File Offset: 0x00001AE1
		// (set) Token: 0x06000066 RID: 102 RVA: 0x000038EE File Offset: 0x00001AEE
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
	}
}
