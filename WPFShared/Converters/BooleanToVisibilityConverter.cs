using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace WPFShared.Converters
{
	// Token: 0x02000026 RID: 38
	public class BooleanToVisibilityConverter : IValueConverter
	{
		// Token: 0x06000189 RID: 393 RVA: 0x00007798 File Offset: 0x00005998
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Visibility visibility = Visibility.Visible;
			Visibility visibility2 = Visibility.Collapsed;
			if (parameter != null && parameter is string)
			{
				List<string> list = (from o in (parameter as string).Split(new char[] { '|' })
					select o.Trim().ToLower()).ToList<string>();
				switch (list.Count)
				{
				case 1:
					if (list[0] == "collapsed")
					{
						visibility = Visibility.Collapsed;
						visibility2 = Visibility.Visible;
					}
					break;
				case 2:
					visibility = (Visibility)Enum.Parse(typeof(Visibility), list[0], true);
					visibility2 = (Visibility)Enum.Parse(typeof(Visibility), list[1], true);
					break;
				}
			}
			return ((bool)value) ? visibility : visibility2;
		}

		// Token: 0x0600018A RID: 394 RVA: 0x0000787C File Offset: 0x00005A7C
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = parameter == null || !(parameter is string) || !(((string)parameter).ToLower() == "collapsed");
			return ((Visibility)value == Visibility.Visible) ? flag : (!flag);
		}
	}
}
