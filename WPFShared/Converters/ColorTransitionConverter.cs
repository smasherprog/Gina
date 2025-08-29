using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace WPFShared.Converters
{
	// Token: 0x02000028 RID: 40
	public class ColorTransitionConverter : IMultiValueConverter
	{
		// Token: 0x06000193 RID: 403 RVA: 0x00007B18 File Offset: 0x00005D18
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			double? num = values[0] as double?;
			double num2 = ((num != null) ? num.GetValueOrDefault() : 0.0);
			Color color = (values[1] as Color?) ?? Colors.Transparent;
			Color color2 = (values[2] as Color?) ?? Colors.Transparent;
			return Color.FromArgb(global::System.Convert.ToByte((double)color.A + (double)(color2.A - color.A) * num2), global::System.Convert.ToByte((double)color.R + (double)(color2.R - color.R) * num2), global::System.Convert.ToByte((double)color.G + (double)(color2.G - color.G) * num2), global::System.Convert.ToByte((double)color.B + (double)(color2.B - color.B) * num2));
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00007C29 File Offset: 0x00005E29
		public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
