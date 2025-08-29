using System;
using System.Globalization;
using System.Windows.Data;

namespace WPFShared.Converters
{
	// Token: 0x02000035 RID: 53
	public class SecondsRemainingToStringConverter : IValueConverter
	{
		// Token: 0x060001C2 RID: 450 RVA: 0x00008454 File Offset: 0x00006654
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null || !(value is int))
			{
				return null;
			}
			int num = (int)value;
			return string.Format("{0:00}:{1:00}:{2:00}", num / 3600, num % 3600 / 60, num % 60);
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x000084A3 File Offset: 0x000066A3
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
