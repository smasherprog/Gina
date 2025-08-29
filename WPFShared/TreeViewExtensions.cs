using System.Windows.Controls;

namespace WPFShared
{
	// Token: 0x0200003F RID: 63
	public static class TreeViewExtensions
	{
		// Token: 0x060001DD RID: 477 RVA: 0x000089F4 File Offset: 0x00006BF4
		public static void ClearTreeViewSelection(this TreeView tv)
		{
			if (tv != null)
			{
				TreeViewExtensions.ClearTreeViewItemsControlSelection(tv.Items, tv.ItemContainerGenerator);
			}
		}

		// Token: 0x060001DE RID: 478 RVA: 0x00008A0C File Offset: 0x00006C0C
		private static void ClearTreeViewItemsControlSelection(ItemCollection ic, ItemContainerGenerator icg)
		{
			if (ic != null && icg != null)
			{
				for (int i = 0; i < ic.Count; i++)
				{
					TreeViewItem treeViewItem = icg.ContainerFromIndex(i) as TreeViewItem;
					if (treeViewItem != null)
					{
						TreeViewExtensions.ClearTreeViewItemsControlSelection(treeViewItem.Items, treeViewItem.ItemContainerGenerator);
						treeViewItem.IsSelected = false;
					}
				}
			}
		}
	}
}
