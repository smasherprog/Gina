using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WPFShared.Converters
{
	// Token: 0x02000030 RID: 48
	public class InRangeToVisibilityConverter : IValueConverter
	{
		// Token: 0x060001B1 RID: 433 RVA: 0x00008160 File Offset: 0x00006360
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			object obj;
			try
			{
				double num = global::System.Convert.ToDouble(value);
				string[] array = (parameter as string).Split(new char[] { '|' });
				double num2 = double.Parse(array[0]);
				double num3 = double.Parse(array[1]);
				Visibility visibility = Visibility.Visible;
				if (array.Length == 3 && array[2].ToLower() == "collapsed")
				{
					visibility = Visibility.Collapsed;
				}
				Visibility visibility2 = ((visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible);
				obj = ((num >= num2 && num <= num3) ? visibility : visibility2);
			}
			catch
			{
				obj = false;
			}
			return obj;
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x00008200 File Offset: 0x00006400
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
