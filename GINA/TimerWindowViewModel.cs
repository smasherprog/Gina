using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using GimaSoft.Business.GINA;
using WPFShared;

namespace GimaSoft.GINA
{
	// Token: 0x0200001A RID: 26
	public class TimerWindowViewModel : GINAViewModel
	{
		// Token: 0x0600030B RID: 779 RVA: 0x0000A79C File Offset: 0x0000899C
		public TimerWindowViewModel()
		{
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0000A848 File Offset: 0x00008A48
		public TimerWindowViewModel(BehaviorGroup behavior)
		{
			this.Behavior = behavior;
			this.Behavior.BehaviorMatchesCollectionChanged += delegate(object o)
			{
				if (this.Behavior.Matches.Any<TriggerMatchedEventArgs>())
				{
					this.Window.BringToTop();
					this._Timer.IsEnabled = true;
				}
				else
				{
					this._Timer.IsEnabled = false;
				}
				base.RaisePropertyChanged("BackgroundFill");
				base.RaisePropertyChanged("ShowFullBackground");
			};
			this.Behavior.PropertyChanged += delegate(object o, PropertyChangedEventArgs e)
			{
				string propertyName;
				if ((propertyName = e.PropertyName) != null)
				{
					if (!(propertyName == "FontName") && !(propertyName == "FontSize"))
					{
						return;
					}
					this.RecalculateTimerLabelWidth();
				}
			};
			this.RecalculateTimerLabelWidth();
			this._Timer = new DispatcherTimer
			{
				Interval = TimeSpan.FromSeconds(5.0),
				IsEnabled = true
			};
			this._Timer.Tick += delegate(object o, EventArgs e)
			{
				this.Window.BringToTop();
			};
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x0600030D RID: 781 RVA: 0x0000A8EA File Offset: 0x00008AEA
		public bool ShowFullBackground
		{
			get
			{
				return this.Behavior.Matches.Any<TriggerMatchedEventArgs>();
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x0600030E RID: 782 RVA: 0x0000A8FC File Offset: 0x00008AFC
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

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x0600030F RID: 783 RVA: 0x0000A924 File Offset: 0x00008B24
		// (set) Token: 0x06000310 RID: 784 RVA: 0x0000A931 File Offset: 0x00008B31
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

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000311 RID: 785 RVA: 0x0000A93F File Offset: 0x00008B3F
		// (set) Token: 0x06000312 RID: 786 RVA: 0x0000A94C File Offset: 0x00008B4C
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

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000313 RID: 787 RVA: 0x0000A95F File Offset: 0x00008B5F
		// (set) Token: 0x06000314 RID: 788 RVA: 0x0000A9A8 File Offset: 0x00008BA8
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

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000315 RID: 789 RVA: 0x0000A9FD File Offset: 0x00008BFD
		// (set) Token: 0x06000316 RID: 790 RVA: 0x0000AA05 File Offset: 0x00008C05
		public TimerWindow Window { get; set; }

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000317 RID: 791 RVA: 0x0000AA1C File Offset: 0x00008C1C
		public IEnumerable<string> FontNames
		{
			get
			{
				return from o in global::System.Drawing.FontFamily.Families
					select o.Name into o
					orderby o
					select o;
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000318 RID: 792 RVA: 0x0000AA72 File Offset: 0x00008C72
		// (set) Token: 0x06000319 RID: 793 RVA: 0x0000AA7F File Offset: 0x00008C7F
		public int TimerLabelWidth
		{
			get
			{
				return base.Get<int>("TimerLabelWidth");
			}
			set
			{
				base.Set("TimerLabelWidth", value);
			}
		}

		// Token: 0x0600031A RID: 794 RVA: 0x0000AA94 File Offset: 0x00008C94
		private void RecalculateTimerLabelWidth()
		{
			FormattedText formattedText = new FormattedText("000:00:00", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(this.Behavior.FontName), (double)this.Behavior.FontSize, global::System.Windows.Media.Brushes.Black);
			this.TimerLabelWidth = (int)Math.Ceiling(formattedText.Width);
		}

		// Token: 0x0600031B RID: 795 RVA: 0x0000AAE5 File Offset: 0x00008CE5
		internal void SaveWindow()
		{
			this.Behavior.WindowLayout = this.Window.GetPlacement();
			Configuration.SaveConfiguration(false);
			this.ShowOpaqueWindow = false;
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x0600031C RID: 796 RVA: 0x0000AB12 File Offset: 0x00008D12
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

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x0600031D RID: 797 RVA: 0x0000AB36 File Offset: 0x00008D36
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

		// Token: 0x04000096 RID: 150
		private DispatcherTimer _Timer;
	}
}
