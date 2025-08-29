using System;
using System.Collections.Generic;

namespace BusinessShared
{
	// Token: 0x02000002 RID: 2
	public static class DictionaryExtensions
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static V Value<T, V>(this Dictionary<T, V> dic, T key)
		{
			if (!dic.ContainsKey(key))
			{
				return default(V);
			}
			return dic[key];
		}
	}
}
