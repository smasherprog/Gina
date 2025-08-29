using System.Collections.Generic;
using System.Linq;

namespace WPFShared
{
	// Token: 0x0200004A RID: 74
	public static class ITreeItemExtensions
	{
		// Token: 0x0600020D RID: 525 RVA: 0x000090A8 File Offset: 0x000072A8
		public static List<ITreeItem> GetChildren(this ITreeItem item)
		{
			if (item.TreeChildren == null)
			{
				return null;
			}
			if (item.TreeChildSorter != null)
			{
				List<ITreeItem> list = ((IEnumerable<ITreeItem>)item.TreeChildren).ToList<ITreeItem>();
				list.Sort(item.TreeChildSorter);
				return list;
			}
			return ((IEnumerable<ITreeItem>)item.TreeChildren).ToList<ITreeItem>();
		}
	}
}
