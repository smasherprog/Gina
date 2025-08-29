using System;
using System.Globalization;
using System.Windows.Data;

namespace WPFShared.Converters
{
	// Token: 0x02000033 RID: 51
	public class NullToBooleanConverter : IValueConverter
	{
		// Token: 0x060001BB RID: 443 RVA: 0x00008320 File Offset: 0x00006520
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = false;
			if (parameter is string && parameter.ToString().ToLower() == "true")
			{
				flag = true;
			}
			return (value == null) ? flag : (!flag);
		}

		// Token: 0x060001BC RID: 444 RVA: 0x0000835F File Offset: 0x0000655F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
