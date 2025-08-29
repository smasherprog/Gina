using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using GimaSoft.Business.GINA;

namespace GimaSoft.GINA
{
	// Token: 0x02000010 RID: 16
	public class CharacterAndCategoryConverter : IMultiValueConverter
	{
		// Token: 0x06000272 RID: 626 RVA: 0x00008968 File Offset: 0x00006B68
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (values.Length != 2)
			{
				return Binding.DoNothing;
			}
			GINACharacter ginacharacter = values[0] as GINACharacter;
			TriggerCategory category = values[1] as TriggerCategory;
			if (ginacharacter == null || category == null)
			{
				return Binding.DoNothing;
			}
			return ginacharacter.Categories.SingleOrDefault((CharacterCategory o) => o.Category == category);
		}

		// Token: 0x06000273 RID: 627 RVA: 0x000089C6 File Offset: 0x00006BC6
		public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
