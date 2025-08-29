using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPFShared
{
	// Token: 0x02000015 RID: 21
	public class DialogOverlay : ContentControl
	{
		// Token: 0x0600008B RID: 139 RVA: 0x00003D9C File Offset: 0x00001F9C
		static DialogOverlay()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DialogOverlay), new FrameworkPropertyMetadata(typeof(DialogOverlay)));
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00003FB1 File Offset: 0x000021B1
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property == UIElement.VisibilityProperty && this.VisibilityChangedCommand != null)
			{
				this.VisibilityChangedCommand.Execute(base.Visibility);
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600008D RID: 141 RVA: 0x00003FEB File Offset: 0x000021EB
		// (set) Token: 0x0600008E RID: 142 RVA: 0x00003FFD File Offset: 0x000021FD
		public HorizontalAlignment DialogHorizontalAlignment
		{
			get
			{
				return (HorizontalAlignment)base.GetValue(DialogOverlay.DialogHorizontalAlignmentProperty);
			}
			set
			{
				base.SetValue(DialogOverlay.DialogHorizontalAlignmentProperty, value);
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600008F RID: 143 RVA: 0x00004010 File Offset: 0x00002210
		// (set) Token: 0x06000090 RID: 144 RVA: 0x00004022 File Offset: 0x00002222
		public VerticalAlignment DialogVerticalAlignment
		{
			get
			{
				return (VerticalAlignment)base.GetValue(DialogOverlay.DialogVerticalAlignmentProperty);
			}
			set
			{
				base.SetValue(DialogOverlay.DialogVerticalAlignmentProperty, value);
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000091 RID: 145 RVA: 0x00004035 File Offset: 0x00002235
		// (set) Token: 0x06000092 RID: 146 RVA: 0x00004047 File Offset: 0x00002247
		public Thickness DialogMargin
		{
			get
			{
				return (Thickness)base.GetValue(DialogOverlay.DialogMarginProperty);
			}
			set
			{
				base.SetValue(DialogOverlay.DialogMarginProperty, value);
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000093 RID: 147 RVA: 0x0000405A File Offset: 0x0000225A
		// (set) Token: 0x06000094 RID: 148 RVA: 0x0000406C File Offset: 0x0000226C
		public bool ShowDialogAlignButtons
		{
			get
			{
				return (bool)base.GetValue(DialogOverlay.ShowDialogAlignButtonsProperty);
			}
			set
			{
				base.SetValue(DialogOverlay.ShowDialogAlignButtonsProperty, value);
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000095 RID: 149 RVA: 0x0000407F File Offset: 0x0000227F
		// (set) Token: 0x06000096 RID: 150 RVA: 0x00004091 File Offset: 0x00002291
		public double ContentWidth
		{
			get
			{
				return (double)base.GetValue(DialogOverlay.ContentWidthProperty);
			}
			set
			{
				base.SetValue(DialogOverlay.ContentWidthProperty, value);
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000097 RID: 151 RVA: 0x000040A4 File Offset: 0x000022A4
		// (set) Token: 0x06000098 RID: 152 RVA: 0x000040B6 File Offset: 0x000022B6
		public double ContentHeight
		{
			get
			{
				return (double)base.GetValue(DialogOverlay.ContentHeightProperty);
			}
			set
			{
				base.SetValue(DialogOverlay.ContentHeightProperty, value);
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000099 RID: 153 RVA: 0x000040C9 File Offset: 0x000022C9
		// (set) Token: 0x0600009A RID: 154 RVA: 0x000040DB File Offset: 0x000022DB
		public string Title
		{
			get
			{
				return (string)base.GetValue(DialogOverlay.TitleProperty);
			}
			set
			{
				base.SetValue(DialogOverlay.TitleProperty, value);
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600009B RID: 155 RVA: 0x000040E9 File Offset: 0x000022E9
		// (set) Token: 0x0600009C RID: 156 RVA: 0x000040FB File Offset: 0x000022FB
		public Brush BorderColor
		{
			get
			{
				return (Brush)base.GetValue(DialogOverlay.BorderColorProperty);
			}
			set
			{
				base.SetValue(DialogOverlay.BorderColorProperty, value);
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600009D RID: 157 RVA: 0x00004109 File Offset: 0x00002309
		// (set) Token: 0x0600009E RID: 158 RVA: 0x0000411B File Offset: 0x0000231B
		public GenericCommand VisibilityChangedCommand
		{
			get
			{
				return (GenericCommand)base.GetValue(DialogOverlay.VisibilityChangedCommandProperty);
			}
			set
			{
				base.SetValue(DialogOverlay.VisibilityChangedCommandProperty, value);
			}
		}

		// Token: 0x0600009F RID: 159 RVA: 0x0000412C File Offset: 0x0000232C
		private static void VisibilityChangedCommand_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DialogOverlay dialogOverlay = d as DialogOverlay;
			if (dialogOverlay.VisibilityChangedCommand != null)
			{
				dialogOverlay.VisibilityChangedCommand.Execute(dialogOverlay.Visibility);
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x000041BA File Offset: 0x000023BA
		public GenericCommand MoveDialogCommand
		{
			get
			{
				return new GenericCommand(delegate(object p)
				{
					string text;
					if ((text = p as string) != null)
					{
						if (text == "left")
						{
							this.DialogHorizontalAlignment = HorizontalAlignment.Left;
							return;
						}
						if (text == "right")
						{
							this.DialogHorizontalAlignment = HorizontalAlignment.Right;
							return;
						}
						if (!(text == "center"))
						{
							return;
						}
						this.DialogHorizontalAlignment = HorizontalAlignment.Center;
					}
				});
			}
		}

		// Token: 0x04000033 RID: 51
		public static readonly DependencyProperty DialogHorizontalAlignmentProperty = DependencyProperty.Register("DialogHorizontalAlignment", typeof(HorizontalAlignment), typeof(DialogOverlay), new FrameworkPropertyMetadata(HorizontalAlignment.Center, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null));

		// Token: 0x04000034 RID: 52
		public static readonly DependencyProperty DialogVerticalAlignmentProperty = DependencyProperty.Register("DialogVerticalAlignment", typeof(VerticalAlignment), typeof(DialogOverlay), new FrameworkPropertyMetadata(VerticalAlignment.Center, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null));

		// Token: 0x04000035 RID: 53
		public static readonly DependencyProperty DialogMarginProperty = DependencyProperty.Register("DialogMargin", typeof(Thickness), typeof(DialogOverlay), new FrameworkPropertyMetadata(new Thickness(double.NaN), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null));

		// Token: 0x04000036 RID: 54
		public static readonly DependencyProperty ShowDialogAlignButtonsProperty = DependencyProperty.Register("ShowDialogAlignButtons", typeof(bool), typeof(DialogOverlay), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null));

		// Token: 0x04000037 RID: 55
		public static readonly DependencyProperty ContentWidthProperty = DependencyProperty.Register("ContentWidth", typeof(double), typeof(DialogOverlay), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null));

		// Token: 0x04000038 RID: 56
		public static readonly DependencyProperty ContentHeightProperty = DependencyProperty.Register("ContentHeight", typeof(double), typeof(DialogOverlay), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null));

		// Token: 0x04000039 RID: 57
		public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(DialogOverlay), null);

		// Token: 0x0400003A RID: 58
		public static readonly DependencyProperty BorderColorProperty = DependencyProperty.Register("BorderColor", typeof(Brush), typeof(DialogOverlay), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Navy), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null));

		// Token: 0x0400003B RID: 59
		public static readonly DependencyProperty VisibilityChangedCommandProperty = DependencyProperty.Register("VisibilityChangedCommand", typeof(GenericCommand), typeof(DialogOverlay), new PropertyMetadata(new PropertyChangedCallback(DialogOverlay.VisibilityChangedCommand_Changed)));
	}
}
