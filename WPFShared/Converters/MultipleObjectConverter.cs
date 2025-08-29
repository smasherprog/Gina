using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace WPFShared.Converters
{
	// Token: 0x02000032 RID: 50
	public class MultipleObjectConverter : IMultiValueConverter
	{
		// Token: 0x060001B8 RID: 440 RVA: 0x000082E0 File Offset: 0x000064E0
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			List<object> list = new List<object>();
			foreach (object obj in values)
			{
				list.Add(obj);
			}
			return list;
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x0000830F File Offset: 0x0000650F
		public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
