using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WPFShared.Converters
{
	// Token: 0x02000038 RID: 56
	public class StringNullOrEmptyToVisibilityConverter : IValueConverter
	{
		// Token: 0x060001CC RID: 460 RVA: 0x00008594 File Offset: 0x00006794
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

		// Token: 0x060001CD RID: 461 RVA: 0x000085DC File Offset: 0x000067DC
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
			if (!(value is string))
			{
				return visibility;
			}
			return string.IsNullOrWhiteSpace(value as string) ? visibility : visibility2;
		}

		// Token: 0x060001CE RID: 462 RVA: 0x00008680 File Offset: 0x00006880
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
