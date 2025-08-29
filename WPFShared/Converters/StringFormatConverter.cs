using System;
using System.Globalization;
using System.Windows.Data;

namespace WPFShared.Converters
{
	// Token: 0x02000037 RID: 55
	public class StringFormatConverter : IValueConverter
	{
		// Token: 0x060001C9 RID: 457 RVA: 0x0000855F File Offset: 0x0000675F
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{
				return null;
			}
			if (parameter != null && parameter is string)
			{
				return string.Format((string)parameter, value);
			}
			return value.ToString();
		}

		// Token: 0x060001CA RID: 458 RVA: 0x00008584 File Offset: 0x00006784
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
