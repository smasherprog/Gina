using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WPFShared
{
	// Token: 0x0200003D RID: 61
	public static class IEnumerableExtensions
	{
		// Token: 0x060001DA RID: 474 RVA: 0x00008894 File Offset: 0x00006A94
		public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> coll)
		{
			ObservableCollection<T> observableCollection = new ObservableCollection<T>();
			foreach (T t in coll)
			{
				observableCollection.Add(t);
			}
			return observableCollection;
		}
	}
}
