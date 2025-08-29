using System.Windows;

namespace WPFShared
{
	// Token: 0x02000016 RID: 22
	public class DialogEditor : DialogOverlay
	{
		// Token: 0x060000A3 RID: 163 RVA: 0x000041D5 File Offset: 0x000023D5
		static DialogEditor()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DialogEditor), new FrameworkPropertyMetadata(typeof(DialogEditor)));
		}
	}
}
