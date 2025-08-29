using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace WPFShared.Converters
{
	// Token: 0x02000031 RID: 49
	public class MultipleBooleanToVisibilityConverter : IMultiValueConverter
	{
		// Token: 0x060001B4 RID: 436 RVA: 0x0000821C File Offset: 0x0000641C
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			Visibility visibility = Visibility.Visible;
			if (values.Length >= 1 && values[0] is string && ((string)values[0]).ToLower() == "collapsed")
			{
				visibility = Visibility.Collapsed;
			}
			Visibility visibility2 = ((visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible);
			bool flag = true;
			foreach (object obj in values.Where((object o) => o is bool))
			{
				flag &= (bool)obj;
			}
			return flag ? visibility : visibility2;
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x000082D0 File Offset: 0x000064D0
		public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
