using System;
using System.Windows;

namespace WPFShared
{
	// Token: 0x02000057 RID: 87
	public class TimeSpanPicker : BindableControl
	{
		// Token: 0x06000237 RID: 567 RVA: 0x00009730 File Offset: 0x00007930
		static TimeSpanPicker()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(TimeSpanPicker), new FrameworkPropertyMetadata(typeof(TimeSpanPicker)));
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000238 RID: 568 RVA: 0x00009808 File Offset: 0x00007A08
		// (set) Token: 0x06000239 RID: 569 RVA: 0x00009815 File Offset: 0x00007A15
		public int Hours
		{
			get
			{
				return base.Get<int>("Hours");
			}
			set
			{
				base.Set("Hours", value);
				this.RecalculateValue();
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600023A RID: 570 RVA: 0x0000982E File Offset: 0x00007A2E
		// (set) Token: 0x0600023B RID: 571 RVA: 0x0000983B File Offset: 0x00007A3B
		public int Minutes
		{
			get
			{
				return base.Get<int>("Minutes");
			}
			set
			{
				base.Set("Minutes", value);
				this.RecalculateValue();
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600023C RID: 572 RVA: 0x00009854 File Offset: 0x00007A54
		// (set) Token: 0x0600023D RID: 573 RVA: 0x00009861 File Offset: 0x00007A61
		public int Seconds
		{
			get
			{
				return base.Get<int>("Seconds");
			}
			set
			{
				base.Set("Seconds", value);
				this.RecalculateValue();
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600023E RID: 574 RVA: 0x0000987A File Offset: 0x00007A7A
		// (set) Token: 0x0600023F RID: 575 RVA: 0x00009887 File Offset: 0x00007A87
		public int Milliseconds
		{
			get
			{
				return base.Get<int>("Milliseconds");
			}
			set
			{
				base.Set("Milliseconds", value);
				this.RecalculateValue();
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000240 RID: 576 RVA: 0x000098A0 File Offset: 0x00007AA0
		// (set) Token: 0x06000241 RID: 577 RVA: 0x000098AD File Offset: 0x00007AAD
		public int MaxHours
		{
			get
			{
				return base.Get<int>("MaxHours");
			}
			set
			{
				base.Set("MaxHours", value);
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000242 RID: 578 RVA: 0x000098C0 File Offset: 0x00007AC0
		// (set) Token: 0x06000243 RID: 579 RVA: 0x000098CD File Offset: 0x00007ACD
		public int MaxMinutes
		{
			get
			{
				return base.Get<int>("MaxMinutes");
			}
			set
			{
				base.Set("MaxMinutes", value);
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000244 RID: 580 RVA: 0x000098E0 File Offset: 0x00007AE0
		// (set) Token: 0x06000245 RID: 581 RVA: 0x000098ED File Offset: 0x00007AED
		public int MaxSeconds
		{
			get
			{
				return base.Get<int>("MaxSeconds");
			}
			set
			{
				base.Set("MaxSeconds", value);
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000246 RID: 582 RVA: 0x00009900 File Offset: 0x00007B00
		// (set) Token: 0x06000247 RID: 583 RVA: 0x00009912 File Offset: 0x00007B12
		public int MaxTimeSpanInSeconds
		{
			get
			{
				return (int)base.GetValue(TimeSpanPicker.MaxTimeSpanInSecondsProperty);
			}
			set
			{
				base.SetValue(TimeSpanPicker.MaxTimeSpanInSecondsProperty, value);
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000248 RID: 584 RVA: 0x00009925 File Offset: 0x00007B25
		// (set) Token: 0x06000249 RID: 585 RVA: 0x00009937 File Offset: 0x00007B37
		public string Granularity
		{
			get
			{
				return (string)base.GetValue(TimeSpanPicker.GranularityProperty);
			}
			set
			{
				base.SetValue(TimeSpanPicker.GranularityProperty, value);
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x0600024A RID: 586 RVA: 0x00009945 File Offset: 0x00007B45
		// (set) Token: 0x0600024B RID: 587 RVA: 0x00009957 File Offset: 0x00007B57
		public long Value
		{
			get
			{
				return (long)base.GetValue(TimeSpanPicker.ValueProperty);
			}
			set
			{
				base.SetValue(TimeSpanPicker.ValueProperty, value);
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x0600024C RID: 588 RVA: 0x0000996A File Offset: 0x00007B6A
		// (set) Token: 0x0600024D RID: 589 RVA: 0x00009977 File Offset: 0x00007B77
		public int SelectedSection
		{
			get
			{
				return base.Get<int>("SelectedSection");
			}
			set
			{
				base.Set("SelectedSection", value);
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x0600024E RID: 590 RVA: 0x0000998C File Offset: 0x00007B8C
		public int MinimumInterval
		{
			get
			{
				string text;
				switch (text = this.Granularity.ToLower())
				{
				case "ms":
				case "millisecond":
					return 0;
				case "m":
				case "minute":
					return 2;
				case "h":
				case "hour":
					return 3;
				}
				return 1;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600024F RID: 591 RVA: 0x00009A44 File Offset: 0x00007C44
		private long IntervalDivisor
		{
			get
			{
				switch (this.MinimumInterval)
				{
				case 0:
					return 1L;
				case 1:
					return 1000L;
				case 2:
					return 60000L;
				default:
					return 360000L;
				}
			}
		}

		// Token: 0x06000250 RID: 592 RVA: 0x00009A84 File Offset: 0x00007C84
		private static void MaxTimeSpanInSecondsProperty_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TimeSpanPicker timeSpanPicker = d as TimeSpanPicker;
			timeSpanPicker.MaxHours = timeSpanPicker.MaxTimeSpanInSeconds / 3600;
			timeSpanPicker.MaxMinutes = ((timeSpanPicker.MaxTimeSpanInSeconds < 3600) ? (timeSpanPicker.MaxTimeSpanInSeconds / 3600) : 59);
			timeSpanPicker.MaxSeconds = ((timeSpanPicker.MaxTimeSpanInSeconds < 60) ? timeSpanPicker.MaxTimeSpanInSeconds : 59);
		}

		// Token: 0x06000251 RID: 593 RVA: 0x00009AE8 File Offset: 0x00007CE8
		private static void GranularityProperty_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TimeSpanPicker timeSpanPicker = d as TimeSpanPicker;
			timeSpanPicker.SelectedSection = ((timeSpanPicker.MinimumInterval == 0) ? 1 : timeSpanPicker.MinimumInterval);
		}

		// Token: 0x06000252 RID: 594 RVA: 0x00009B14 File Offset: 0x00007D14
		private static void Value_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TimeSpanPicker timeSpanPicker = d as TimeSpanPicker;
			timeSpanPicker.RefreshValue();
		}

		// Token: 0x06000253 RID: 595 RVA: 0x00009B30 File Offset: 0x00007D30
		public void RefreshValue()
		{
			this._UpdatingValue = true;
			this.Hours = Convert.ToInt32(this.Value * this.IntervalDivisor / 3600000L);
			this.Minutes = Convert.ToInt32(this.Value * this.IntervalDivisor % 3600000L / 60000L);
			this.Seconds = Convert.ToInt32(this.Value * this.IntervalDivisor % 60000L / 1000L);
			this.Milliseconds = Convert.ToInt32(this.Value * this.IntervalDivisor % 1000L);
			this._UpdatingValue = false;
		}

		// Token: 0x06000254 RID: 596 RVA: 0x00009BD8 File Offset: 0x00007DD8
		private void RecalculateValue()
		{
			if (this._UpdatingValue)
			{
				return;
			}
			if (this.MinimumInterval == 0 && this.Milliseconds % 50 != 0)
			{
				this.Milliseconds = this.Milliseconds / 50 * 50;
			}
			this.Value = (long)(this.Hours * 3600000 + this.Minutes * 60000 + this.Seconds * 1000 + this.Milliseconds) / this.IntervalDivisor;
		}

		// Token: 0x06000255 RID: 597 RVA: 0x00009C50 File Offset: 0x00007E50
		private void ModifyTime(long milliseconds)
		{
			long num = this.Value * this.IntervalDivisor + milliseconds;
			num = ((num < 0L) ? (num + (long)(this.MaxTimeSpanInSeconds * 1000)) : num);
			num = ((num > (long)(this.MaxTimeSpanInSeconds * 1000)) ? (num % (long)(this.MaxTimeSpanInSeconds * 1000)) : num);
			this.Value = num / this.IntervalDivisor;
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000256 RID: 598 RVA: 0x00009CE3 File Offset: 0x00007EE3
		public GenericCommand SetPositionCommand
		{
			get
			{
				if (this._SetPositionCommand == null)
				{
					this._SetPositionCommand = new GenericCommand(delegate(object p)
					{
						int num;
						if (p is string && int.TryParse((string)p, out num))
						{
							this.SelectedSection = num;
						}
					});
				}
				return this._SetPositionCommand;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000257 RID: 599 RVA: 0x00009D81 File Offset: 0x00007F81
		public GenericCommand IncreaseValueCommand
		{
			get
			{
				if (this._IncreaseValueCommand == null)
				{
					this._IncreaseValueCommand = new GenericCommand(delegate(object p)
					{
						if (this.SelectedSection < this.MinimumInterval)
						{
							this.SelectedSection = this.MinimumInterval;
						}
						switch (this.SelectedSection)
						{
						case 0:
							this.ModifyTime(50L);
							return;
						case 1:
							this.ModifyTime(1000L);
							return;
						case 2:
							this.ModifyTime(60000L);
							return;
						case 3:
							this.ModifyTime(3600000L);
							return;
						default:
							return;
						}
					});
				}
				return this._IncreaseValueCommand;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000258 RID: 600 RVA: 0x00009E1D File Offset: 0x0000801D
		public GenericCommand DecreaseValueCommand
		{
			get
			{
				if (this._DecreaseValueCommand == null)
				{
					this._DecreaseValueCommand = new GenericCommand(delegate(object p)
					{
						if (this.SelectedSection < this.MinimumInterval)
						{
							this.SelectedSection = this.MinimumInterval;
						}
						switch (this.SelectedSection)
						{
						case 0:
							this.ModifyTime(-50L);
							return;
						case 1:
							this.ModifyTime(-1000L);
							return;
						case 2:
							this.ModifyTime(-60000L);
							return;
						case 3:
							this.ModifyTime(-3600000L);
							return;
						default:
							return;
						}
					});
				}
				return this._DecreaseValueCommand;
			}
		}

		// Token: 0x0400011B RID: 283
		public static readonly DependencyProperty MaxTimeSpanInSecondsProperty = DependencyProperty.Register("MaxTimeSpanInSeconds", typeof(int), typeof(TimeSpanPicker), new PropertyMetadata(new PropertyChangedCallback(TimeSpanPicker.MaxTimeSpanInSecondsProperty_Changed)));

		// Token: 0x0400011C RID: 284
		public static readonly DependencyProperty GranularityProperty = DependencyProperty.Register("Granularity", typeof(string), typeof(TimeSpanPicker), new PropertyMetadata(new PropertyChangedCallback(TimeSpanPicker.GranularityProperty_Changed)));

		// Token: 0x0400011D RID: 285
		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(long), typeof(TimeSpanPicker), new FrameworkPropertyMetadata(0L, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(TimeSpanPicker.Value_Changed)));

		// Token: 0x0400011E RID: 286
		private bool _UpdatingValue;

		// Token: 0x0400011F RID: 287
		private GenericCommand _SetPositionCommand;

		// Token: 0x04000120 RID: 288
		private GenericCommand _IncreaseValueCommand;

		// Token: 0x04000121 RID: 289
		private GenericCommand _DecreaseValueCommand;
	}
}
