using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GimaSoft.Business.GINA;
using WPFShared;

namespace GimaSoft.GINA
{
	// Token: 0x0200001E RID: 30
	public class TextWindowViewModel : GINAViewModel
	{
		// Token: 0x06000359 RID: 857 RVA: 0x0000B9E8 File Offset: 0x00009BE8
		public TextWindowViewModel()
		{
		}

		// Token: 0x0600035A RID: 858 RVA: 0x0000BA28 File Offset: 0x00009C28
		public TextWindowViewModel(BehaviorGroup behavior)
		{
			this.Behavior = behavior;
			this.Behavior.BehaviorMatchesCollectionChanged += delegate(object o)
			{
				if (this.Behavior.Matches.Any<TriggerMatchedEventArgs>())
				{
					this.Window.BringToTop();
				}
				base.RaisePropertyChanged("BackgroundFill");
				base.RaisePropertyChanged("ShowFullBackground");
			};
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x0600035B RID: 859 RVA: 0x0000BA60 File Offset: 0x00009C60
		public bool ShowFullBackground
		{
			get
			{
				return this.Behavior.Matches.Any<TriggerMatchedEventArgs>();
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x0600035C RID: 860 RVA: 0x0000BA72 File Offset: 0x00009C72
		public double BackgroundFill
		{
			get
			{
				if (this.Behavior.Matches.Count <= 0)
				{
					return 0.0;
				}
				return 1.0;
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x0600035D RID: 861 RVA: 0x0000BA9A File Offset: 0x00009C9A
		// (set) Token: 0x0600035E RID: 862 RVA: 0x0000BAA7 File Offset: 0x00009CA7
		public BehaviorGroup Behavior
		{
			get
			{
				return base.Get<BehaviorGroup>("Behavior");
			}
			set
			{
				base.Set("Behavior", value);
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x0600035F RID: 863 RVA: 0x0000BAB5 File Offset: 0x00009CB5
		// (set) Token: 0x06000360 RID: 864 RVA: 0x0000BAC2 File Offset: 0x00009CC2
		public bool ShowOpaqueWindow
		{
			get
			{
				return base.Get<bool>("ShowOpaqueWindow");
			}
			set
			{
				base.Set("ShowOpaqueWindow", value);
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000361 RID: 865 RVA: 0x0000BAD5 File Offset: 0x00009CD5
		// (set) Token: 0x06000362 RID: 866 RVA: 0x0000BB1C File Offset: 0x00009D1C
		public string BehaviorName
		{
			get
			{
				return this.Behavior.Name;
			}
			set
			{
				if (!BehaviorGroup.All.Any((BehaviorGroup o) => o.Name.ToLower() == value.ToLower() && o != this.Behavior))
				{
					this.Behavior.Name = value;
				}
				base.RaisePropertyChanged("BehaviorName");
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000363 RID: 867 RVA: 0x0000BB71 File Offset: 0x00009D71
		// (set) Token: 0x06000364 RID: 868 RVA: 0x0000BB79 File Offset: 0x00009D79
		public TextWindow Window { get; set; }

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000365 RID: 869 RVA: 0x0000BB90 File Offset: 0x00009D90
		public IEnumerable<string> FontNames
		{
			get
			{
				return from o in FontFamily.Families
					select o.Name into o
					orderby o
					select o;
			}
		}

		// Token: 0x06000366 RID: 870 RVA: 0x0000BBE6 File Offset: 0x00009DE6
		internal void SaveWindow()
		{
			this.Behavior.WindowLayout = this.Window.GetPlacement();
			Configuration.SaveConfiguration(false);
			this.ShowOpaqueWindow = false;
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000367 RID: 871 RVA: 0x0000BC13 File Offset: 0x00009E13
		public GenericCommand SaveWindowCommand
		{
			get
			{
				return new GenericCommand(delegate(object p)
				{
					this.SaveWindow();
				});
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000368 RID: 872 RVA: 0x0000BC37 File Offset: 0x00009E37
		public GenericCommand ToggleEditCommand
		{
			get
			{
				return new GenericCommand(delegate(object p)
				{
					this.ShowOpaqueWindow = !this.ShowOpaqueWindow;
				});
			}
		}
	}
}
