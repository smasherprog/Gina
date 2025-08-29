using System;
using System.Globalization;
using System.Windows.Data;

namespace WPFShared.Converters
{
	// Token: 0x02000025 RID: 37
	public class BooleanNegateConverter : IValueConverter
	{
		// Token: 0x06000186 RID: 390 RVA: 0x00007745 File Offset: 0x00005945
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool)
			{
				return !(bool)value;
			}
			return Binding.DoNothing;
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00007763 File Offset: 0x00005963
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool)
			{
				return !(bool)value;
			}
			return Binding.DoNothing;
		}
	}
}
