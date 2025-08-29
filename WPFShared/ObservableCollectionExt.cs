using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace WPFShared
{
	// Token: 0x0200004B RID: 75
	public class ObservableCollectionExt<T> : ObservableCollection<T>
	{
		// Token: 0x0600020E RID: 526 RVA: 0x000090F6 File Offset: 0x000072F6
		public ObservableCollectionExt()
		{
		}

		// Token: 0x0600020F RID: 527 RVA: 0x000090FE File Offset: 0x000072FE
		public ObservableCollectionExt(IEnumerable<T> collection)
			: base(collection)
		{
		}

		// Token: 0x06000210 RID: 528 RVA: 0x00009107 File Offset: 0x00007307
		public ObservableCollectionExt(List<T> list)
			: base(list)
		{
		}

		// Token: 0x06000211 RID: 529 RVA: 0x00009110 File Offset: 0x00007310
		public void RemoveRange(int index, int count)
		{
			base.CheckReentrancy();
			List<T> list = base.Items as List<T>;
			list.GetRange(index, count);
			list.RemoveRange(index, count);
			this.OnReset();
		}

		// Token: 0x06000212 RID: 530 RVA: 0x00009148 File Offset: 0x00007348
		public void InsertRange(int index, IEnumerable<T> collection)
		{
			base.CheckReentrancy();
			List<T> list = base.Items as List<T>;
			list.InsertRange(index, collection);
			this.OnReset();
		}

		// Token: 0x06000213 RID: 531 RVA: 0x00009175 File Offset: 0x00007375
		private void OnReset()
		{
			this.OnPropertyChanged("Count");
			this.OnPropertyChanged("Item[]");
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

		// Token: 0x06000214 RID: 532 RVA: 0x00009199 File Offset: 0x00007399
		private void OnPropertyChanged(string propertyName)
		{
			this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}
	}
}
