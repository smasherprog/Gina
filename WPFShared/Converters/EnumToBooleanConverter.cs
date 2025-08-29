using System;
using System.Globalization;
using System.Windows.Data;

namespace WPFShared.Converters
{
	// Token: 0x0200002C RID: 44
	public class EnumToBooleanConverter : IValueConverter
	{
		// Token: 0x060001A5 RID: 421 RVA: 0x00007ED1 File Offset: 0x000060D1
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value.Equals(parameter);
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00007EDF File Offset: 0x000060DF
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!value.Equals(true))
			{
				return Binding.DoNothing;
			}
			return parameter;
		}
	}
}
