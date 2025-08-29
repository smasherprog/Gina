using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace WPFShared
{
	// Token: 0x0200003C RID: 60
	public static class DependencyObjectExtensions
	{
		// Token: 0x060001D8 RID: 472 RVA: 0x000087DC File Offset: 0x000069DC
		public static T VisualUpwardSearch<T>(this DependencyObject source)
		{
			while (source != null && source.GetType() != typeof(T))
			{
				if (source is Visual || source is Visual3D)
				{
					source = VisualTreeHelper.GetParent(source);
				}
				else
				{
					source = LogicalTreeHelper.GetParent(source);
				}
			}
			if (!(source is T))
			{
				return default(T);
			}
			return (T)((object)source);
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x00008840 File Offset: 0x00006A40
		public static T FindVisualChild<T>(this DependencyObject obj) where T : DependencyObject
		{
			for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
			{
				DependencyObject child = VisualTreeHelper.GetChild(obj, i);
				if (child != null && child is T)
				{
					return (T)((object)child);
				}
				T t = child.FindVisualChild<T>();
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}
	}
}
