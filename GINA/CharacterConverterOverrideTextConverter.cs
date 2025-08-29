using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using GimaSoft.Business.GINA;

namespace GimaSoft.GINA
{
	// Token: 0x0200000F RID: 15
	public class CharacterConverterOverrideTextConverter : IMultiValueConverter
	{
		// Token: 0x0600026F RID: 623 RVA: 0x00008864 File Offset: 0x00006A64
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (values.Length < 2)
			{
				return Binding.DoNothing;
			}
			GINACharacter ginacharacter = values[0] as GINACharacter;
			TriggerCategory category = values[1] as TriggerCategory;
			if (ginacharacter == null || category == null)
			{
				return Binding.DoNothing;
			}
			CharacterCategory characterCategory = ginacharacter.Categories.SingleOrDefault((CharacterCategory o) => o.Category == category);
			if (characterCategory == null)
			{
				return Binding.DoNothing;
			}
			List<string> list = new List<string>();
			if (characterCategory.TextOverlaySource != InheritanceSources.FromCategory)
			{
				list.Add("text overlay");
			}
			if (characterCategory.TextStyleSource != InheritanceSources.FromCategory)
			{
				list.Add("text colors");
			}
			if (characterCategory.TimerOverlaySource != InheritanceSources.FromCategory)
			{
				list.Add("timer overlay");
			}
			if (characterCategory.TimerStyleSource != InheritanceSources.FromCategory)
			{
				list.Add("timer colors");
			}
			if (!list.Any<string>())
			{
				return null;
			}
			return "Overriding: " + string.Join(", ", list);
		}

		// Token: 0x06000270 RID: 624 RVA: 0x00008941 File Offset: 0x00006B41
		public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
