using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WPFShared.Converters
{
	// Token: 0x0200002D RID: 45
	public class EnumToVisibilityConverter : IValueConverter
	{
		// Token: 0x060001A8 RID: 424 RVA: 0x00007EFE File Offset: 0x000060FE
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value.Equals(parameter) ? Visibility.Visible : Visibility.Collapsed;
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00007F12 File Offset: 0x00006112
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
