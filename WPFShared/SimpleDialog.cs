using System.Windows;

namespace WPFShared
{
	// Token: 0x02000018 RID: 24
	public class SimpleDialog : DialogOverlay
	{
		// Token: 0x060000AA RID: 170 RVA: 0x000042AC File Offset: 0x000024AC
		static SimpleDialog()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(SimpleDialog), new FrameworkPropertyMetadata(typeof(SimpleDialog)));
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000AB RID: 171 RVA: 0x000043D8 File Offset: 0x000025D8
		// (set) Token: 0x060000AC RID: 172 RVA: 0x000043EA File Offset: 0x000025EA
		public string Message
		{
			get
			{
				return (string)base.GetValue(SimpleDialog.MessageProperty);
			}
			set
			{
				base.SetValue(SimpleDialog.MessageProperty, value);
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000AD RID: 173 RVA: 0x000043F8 File Offset: 0x000025F8
		// (set) Token: 0x060000AE RID: 174 RVA: 0x0000440A File Offset: 0x0000260A
		public bool ShowOKButton
		{
			get
			{
				return (bool)base.GetValue(SimpleDialog.ShowOKButtonProperty);
			}
			set
			{
				base.SetValue(SimpleDialog.ShowOKButtonProperty, value);
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000AF RID: 175 RVA: 0x0000441D File Offset: 0x0000261D
		// (set) Token: 0x060000B0 RID: 176 RVA: 0x0000442F File Offset: 0x0000262F
		public bool ShowCancelButton
		{
			get
			{
				return (bool)base.GetValue(SimpleDialog.ShowCancelButtonProperty);
			}
			set
			{
				base.SetValue(SimpleDialog.ShowCancelButtonProperty, value);
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x00004442 File Offset: 0x00002642
		// (set) Token: 0x060000B2 RID: 178 RVA: 0x00004454 File Offset: 0x00002654
		public string OKButtonText
		{
			get
			{
				return (string)base.GetValue(SimpleDialog.OKButtonTextProperty);
			}
			set
			{
				base.SetValue(SimpleDialog.OKButtonTextProperty, value);
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x00004462 File Offset: 0x00002662
		// (set) Token: 0x060000B4 RID: 180 RVA: 0x00004474 File Offset: 0x00002674
		public string CancelButtonText
		{
			get
			{
				return (string)base.GetValue(SimpleDialog.CancelButtonTextProperty);
			}
			set
			{
				base.SetValue(SimpleDialog.CancelButtonTextProperty, value);
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x00004482 File Offset: 0x00002682
		// (set) Token: 0x060000B6 RID: 182 RVA: 0x00004494 File Offset: 0x00002694
		public GenericCommand OKButtonCommand
		{
			get
			{
				return (GenericCommand)base.GetValue(SimpleDialog.OKButtonCommandProperty);
			}
			set
			{
				base.SetValue(SimpleDialog.OKButtonCommandProperty, value);
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000B7 RID: 183 RVA: 0x000044A2 File Offset: 0x000026A2
		// (set) Token: 0x060000B8 RID: 184 RVA: 0x000044B4 File Offset: 0x000026B4
		public GenericCommand CancelButtonCommand
		{
			get
			{
				return (GenericCommand)base.GetValue(SimpleDialog.CancelButtonCommandProperty);
			}
			set
			{
				base.SetValue(SimpleDialog.CancelButtonCommandProperty, value);
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x000044E0 File Offset: 0x000026E0
		public GenericCommand InternalOKCommand
		{
			get
			{
				return new GenericCommand
				{
					Execute = delegate(object p)
					{
						if (this.OKButtonCommand != null)
						{
							this.OKButtonCommand.Execute(null);
						}
					}
				};
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000BA RID: 186 RVA: 0x00004524 File Offset: 0x00002724
		public GenericCommand InternalCancelCommand
		{
			get
			{
				return new GenericCommand
				{
					Execute = delegate(object p)
					{
						if (this.CancelButtonCommand != null)
						{
							this.CancelButtonCommand.Execute(null);
						}
					}
				};
			}
		}

		// Token: 0x0400003E RID: 62
		public static readonly DependencyProperty MessageProperty = DependencyProperty.Register("Message", typeof(string), typeof(SimpleDialog), null);

		// Token: 0x0400003F RID: 63
		public static readonly DependencyProperty ShowOKButtonProperty = DependencyProperty.Register("ShowOKButton", typeof(bool), typeof(SimpleDialog), null);

		// Token: 0x04000040 RID: 64
		public static readonly DependencyProperty ShowCancelButtonProperty = DependencyProperty.Register("ShowCancelButton", typeof(bool), typeof(SimpleDialog), null);

		// Token: 0x04000041 RID: 65
		public static readonly DependencyProperty OKButtonTextProperty = DependencyProperty.Register("OKButtonText", typeof(string), typeof(SimpleDialog), null);

		// Token: 0x04000042 RID: 66
		public static readonly DependencyProperty CancelButtonTextProperty = DependencyProperty.Register("CancelButtonText", typeof(string), typeof(SimpleDialog), null);

		// Token: 0x04000043 RID: 67
		public static readonly DependencyProperty OKButtonCommandProperty = DependencyProperty.Register("OKButtonCommand", typeof(GenericCommand), typeof(SimpleDialog), null);

		// Token: 0x04000044 RID: 68
		public static readonly DependencyProperty CancelButtonCommandProperty = DependencyProperty.Register("CancelButtonCommand", typeof(GenericCommand), typeof(SimpleDialog), null);
	}
}
