using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace WPFShared
{
	// Token: 0x02000017 RID: 23
	public class SetFocus : FrameworkElement
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x00004202 File Offset: 0x00002402
		// (set) Token: 0x060000A6 RID: 166 RVA: 0x00004214 File Offset: 0x00002414
		[Category("Common")]
		public Control TargetControl
		{
			get
			{
				return (Control)base.GetValue(SetFocus.TargetControlProperty);
			}
			set
			{
				base.SetValue(SetFocus.TargetControlProperty, value);
			}
		}

		// Token: 0x0400003C RID: 60
		public static readonly DependencyProperty TargetControlProperty = DependencyProperty.Register("TargetControl", typeof(Control), typeof(SetFocus), new PropertyMetadata(delegate(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			SetFocus setFocus = o as SetFocus;
			if (setFocus != null && setFocus.TargetControl != null)
			{
				setFocus.TargetControl.Focus();
			}
		}));
	}
}
