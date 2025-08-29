using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WPFShared.Converters
{
	// Token: 0x0200003A RID: 58
	public class VisiblityToBooleanConverter : IValueConverter
	{
		// Token: 0x060001D3 RID: 467 RVA: 0x000086B4 File Offset: 0x000068B4
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = true;
			if (parameter is string && parameter.ToString().ToLower() == "false")
			{
				flag = false;
			}
			return ((Visibility)value == Visibility.Visible) ? flag : (!flag);
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x000086F8 File Offset: 0x000068F8
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return ((bool)value) ? Visibility.Visible : Visibility.Collapsed;
		}
	}
}
