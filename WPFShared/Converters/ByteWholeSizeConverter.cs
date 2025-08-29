using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace WPFShared.Converters
{
	// Token: 0x02000027 RID: 39
	public class ByteWholeSizeConverter : IValueConverter
	{
		// Token: 0x0600018D RID: 397 RVA: 0x000078CC File Offset: 0x00005ACC
		private long DoConversion(decimal baseValue, string fromType, string toType)
		{
			string[] array = new string[] { fromType, toType };
			decimal[] array2 = new decimal[] { 1m, 1m };
			for (int i = 0; i < 2; i++)
			{
				string text;
				if ((text = array[i]) != null)
				{
					if (!(text == "b"))
					{
						if (!(text == "kb"))
						{
							if (!(text == "mb"))
							{
								if (!(text == "gb"))
								{
									if (text == "tb")
									{
										array2[i] = 1099511627776m;
									}
								}
								else
								{
									array2[i] = 1073741824m;
								}
							}
							else
							{
								array2[i] = 1048576m;
							}
						}
						else
						{
							array2[i] = 1024m;
						}
					}
					else
					{
						array2[i] = 1m;
					}
				}
			}
			return global::System.Convert.ToInt64(baseValue * array2[0] / array2[1]);
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00007A28 File Offset: 0x00005C28
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{
				return Binding.DoNothing;
			}
			decimal num = global::System.Convert.ToDecimal(value);
			string[] array = (from o in (parameter as string).Split(new char[] { ',' })
				select o.ToLower()).ToArray<string>();
			return this.DoConversion(num, array[0], array[1]);
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00007AA0 File Offset: 0x00005CA0
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{
				return Binding.DoNothing;
			}
			decimal num = global::System.Convert.ToDecimal(value);
			string[] array = (from o in (parameter as string).Split(new char[] { ',' })
				select o.ToLower()).ToArray<string>();
			return this.DoConversion(num, array[1], array[0]);
		}
	}
}
