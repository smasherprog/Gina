using System.Collections.Generic;
using System.Collections.Specialized;

namespace WPFShared
{
	// Token: 0x02000049 RID: 73
	public interface ITreeItem
	{
		// Token: 0x1700005A RID: 90
		// (get) Token: 0x0600020A RID: 522
		bool HasTreeChildren { get; }

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x0600020B RID: 523
		INotifyCollectionChanged TreeChildren { get; }

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600020C RID: 524
		IComparer<ITreeItem> TreeChildSorter { get; }
	}
}
