using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WPFShared.Converters
{
	// Token: 0x02000039 RID: 57
	public class TestConverter : IValueConverter
	{
		// Token: 0x060001D0 RID: 464 RVA: 0x0000868F File Offset: 0x0000688F
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Debugger.Break();
			return value;
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x00008697 File Offset: 0x00006897
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return ((bool)value) ? Visibility.Visible : Visibility.Collapsed;
		}
	}
}
