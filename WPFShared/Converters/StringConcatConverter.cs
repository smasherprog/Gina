using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace WPFShared.Converters
{
	// Token: 0x02000036 RID: 54
	public class StringConcatConverter : IMultiValueConverter
	{
		// Token: 0x060001C5 RID: 453 RVA: 0x000084C0 File Offset: 0x000066C0
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			List<object> list = values.Where((object o) => o is string).ToList<object>();
			if (!list.Any<object>())
			{
				return Binding.DoNothing;
			}
			string text = "";
			foreach (object obj in list)
			{
				text += obj;
			}
			return text;
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00008550 File Offset: 0x00006750
		public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
