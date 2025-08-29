using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace WPFShared.Converters
{
	// Token: 0x02000029 RID: 41
	public class EmptyCollectionToBooleanConverter : IValueConverter
	{
		// Token: 0x06000196 RID: 406 RVA: 0x00007C38 File Offset: 0x00005E38
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = parameter == null || !(parameter is string) || !(((string)parameter).ToLower() == "false");
			IEnumerable<object> enumerable = value as IEnumerable<object>;
			if (enumerable == null)
			{
				return !flag;
			}
			return enumerable.Any<object>() ? flag : (!flag);
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00007C94 File Offset: 0x00005E94
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
