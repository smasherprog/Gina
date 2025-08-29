using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WPFShared.Converters
{
	// Token: 0x02000034 RID: 52
	public class NullToVisibilityConverter : IValueConverter
	{
		// Token: 0x060001BE RID: 446 RVA: 0x00008370 File Offset: 0x00006570
		private Visibility StringToVisibility(string value, Visibility defaultValue)
		{
			string text;
			if ((text = value.ToLower()) != null)
			{
				if (text == "visible")
				{
					return Visibility.Visible;
				}
				if (text == "hidden")
				{
					return Visibility.Hidden;
				}
				if (text == "collapsed")
				{
					return Visibility.Collapsed;
				}
			}
			return defaultValue;
		}

		// Token: 0x060001BF RID: 447 RVA: 0x000083B8 File Offset: 0x000065B8
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Visibility visibility = Visibility.Collapsed;
			Visibility visibility2 = Visibility.Visible;
			if (parameter is string && parameter.ToString().ToLower() == "visible")
			{
				visibility = Visibility.Visible;
				visibility2 = Visibility.Collapsed;
			}
			else if (parameter is string && parameter.ToString().Contains("|"))
			{
				string[] array = parameter.ToString().Split(new char[] { '|' });
				visibility = this.StringToVisibility(array[0], visibility);
				visibility2 = this.StringToVisibility(array[1], visibility2);
			}
			return (value == null) ? visibility : visibility2;
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00008443 File Offset: 0x00006643
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
