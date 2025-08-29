using System;
using System.Globalization;
using System.Windows.Data;

namespace WPFShared.Converters
{
	// Token: 0x0200002F RID: 47
	public class InRangeToBooleanConverter : IValueConverter
	{
		// Token: 0x060001AE RID: 430 RVA: 0x000080D8 File Offset: 0x000062D8
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			object obj;
			try
			{
				double num = global::System.Convert.ToDouble(value);
				string[] array = (parameter as string).Split(new char[] { '|' });
				double num2 = double.Parse(array[0]);
				double num3 = double.Parse(array[1]);
				obj = num >= num2 && num <= num3;
			}
			catch
			{
				obj = false;
			}
			return obj;
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00008150 File Offset: 0x00006350
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
