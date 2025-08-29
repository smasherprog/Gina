using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using WPFShared;

namespace GimaSoft.GINA
{
	// Token: 0x02000020 RID: 32
	public class TriggerAndGroupViewModel : GINAViewModel, ITreeItem
	{
		// Token: 0x1700014D RID: 333
		// (get) Token: 0x06000381 RID: 897 RVA: 0x0000C107 File Offset: 0x0000A307
		public virtual bool HasTreeChildren
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x06000382 RID: 898 RVA: 0x0000C10A File Offset: 0x0000A30A
		public virtual INotifyCollectionChanged TreeChildren
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06000383 RID: 899 RVA: 0x0000C10D File Offset: 0x0000A30D
		public virtual IComparer<ITreeItem> TreeChildSorter
		{
			get
			{
				return this._Sorter;
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000384 RID: 900 RVA: 0x0000C115 File Offset: 0x0000A315
		public virtual string DisplayName
		{
			get
			{
				return "";
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06000385 RID: 901 RVA: 0x0000C11C File Offset: 0x0000A31C
		public virtual bool IsTriggerView
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06000386 RID: 902 RVA: 0x0000C11F File Offset: 0x0000A31F
		public virtual bool IsTriggerGroupView
		{
			get
			{
				return false;
			}
		}

		// Token: 0x040000C7 RID: 199
		private TriggerAndGroupViewModel.ChildSorter _Sorter = new TriggerAndGroupViewModel.ChildSorter();

		// Token: 0x02000021 RID: 33
		public class ChildSorter : IComparer<ITreeItem>
		{
			// Token: 0x06000388 RID: 904 RVA: 0x0000C138 File Offset: 0x0000A338
			public int Compare(ITreeItem x, ITreeItem y)
			{
				if (x is TriggerGroupViewModel && y is TriggerViewModel)
				{
					return -1;
				}
				if (x is TriggerGroupViewModel && y is TriggerGroupViewModel)
				{
					return (x as TriggerGroupViewModel).TriggerGroup.Name.CompareTo((y as TriggerGroupViewModel).TriggerGroup.Name);
				}
				if (x is TriggerViewModel && y is TriggerViewModel)
				{
					return (x as TriggerViewModel).Trigger.Name.CompareTo((y as TriggerViewModel).Trigger.Name);
				}
				return 0;
			}
		}
	}
}
