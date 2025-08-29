using System.Windows;
using System.Windows.Controls;

namespace WPFShared
{
	// Token: 0x02000007 RID: 7
	public class TreeViewItemDropBehavior
	{
		// Token: 0x06000027 RID: 39 RVA: 0x000026E5 File Offset: 0x000008E5
		public static bool GetIsDropTarget(TreeViewItem treeViewItem)
		{
			return (bool)treeViewItem.GetValue(TreeViewItemDropBehavior.IsDropTargetProperty);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000026F7 File Offset: 0x000008F7
		public static void SetIsDropTarget(TreeViewItem treeViewItem, bool value)
		{
			treeViewItem.SetValue(TreeViewItemDropBehavior.IsDropTargetProperty, value);
		}

		// Token: 0x06000029 RID: 41 RVA: 0x0000270C File Offset: 0x0000090C
		private static void OnIsDropTargetChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			TreeViewItem treeViewItem = sender as TreeViewItem;
			TreeViewItemDropBehavior treeViewItemDropBehavior = new TreeViewItemDropBehavior();
			treeViewItemDropBehavior.RegisterStuff(treeViewItem, (bool)e.NewValue);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002739 File Offset: 0x00000939
		public void RegisterStuff(TreeViewItem item, bool newValue)
		{
			this._Item = item;
			item.AllowDrop = true;
			if (item == null)
			{
				return;
			}
			if (!newValue)
			{
				return;
			}
			if (newValue)
			{
				item.DragOver += this.item_DragOver;
			}
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002768 File Offset: 0x00000968
		private void item_DragOver(object sender, DragEventArgs e)
		{
			object data = ((DataObject)e.Data).GetData("DragSource");
			if (this._Item.DataContext == data)
			{
				e.Effects = DragDropEffects.None;
			}
			else
			{
				e.Effects = DragDropEffects.Copy;
			}
			e.Handled = true;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000027B0 File Offset: 0x000009B0
		private static void item_DragEnter(object sender, DragEventArgs e)
		{
		}

		// Token: 0x0400000F RID: 15
		public static readonly DependencyProperty IsDropTargetProperty = DependencyProperty.RegisterAttached("IsDropTarget", typeof(bool), typeof(TreeViewItemDropBehavior), new UIPropertyMetadata(false, new PropertyChangedCallback(TreeViewItemDropBehavior.OnIsDropTargetChanged)));

		// Token: 0x04000010 RID: 16
		private TreeViewItem _Item;
	}
}
