using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace WPFShared.Converters
{
	// Token: 0x0200002A RID: 42
	public class EmptyCollectionToVisibilityConverter : IValueConverter
	{
		// Token: 0x06000199 RID: 409 RVA: 0x00007CA4 File Offset: 0x00005EA4
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Visibility visibility = ((parameter != null && parameter is string && ((string)parameter).ToLower() == "collapsed") ? Visibility.Collapsed : Visibility.Visible);
			Visibility visibility2 = ((visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible);
			IEnumerable<object> enumerable = value as IEnumerable<object>;
			if (enumerable == null)
			{
				return visibility2;
			}
			return enumerable.Any<object>() ? visibility : visibility2;
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00007D02 File Offset: 0x00005F02
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
