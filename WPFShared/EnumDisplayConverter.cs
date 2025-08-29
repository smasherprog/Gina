using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;

namespace WPFShared
{
	// Token: 0x0200002B RID: 43
	public class EnumDisplayConverter : IValueConverter
	{
		// Token: 0x0600019C RID: 412 RVA: 0x00007D11 File Offset: 0x00005F11
		public EnumDisplayConverter()
		{
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00007D19 File Offset: 0x00005F19
		public EnumDisplayConverter(Type type)
		{
			this.Type = type;
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x0600019E RID: 414 RVA: 0x00007D28 File Offset: 0x00005F28
		// (set) Token: 0x0600019F RID: 415 RVA: 0x00007D30 File Offset: 0x00005F30
		public Type Type
		{
			get
			{
				return this.type;
			}
			set
			{
				if (!value.IsEnum)
				{
					throw new ArgumentException("parameter is not an Enumermated type", "value");
				}
				this.type = value;
				this.FillDisplayNames();
			}
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00007D58 File Offset: 0x00005F58
		private void FillDisplayNames()
		{
			Type type = typeof(Dictionary<, >).GetGenericTypeDefinition().MakeGenericType(new Type[]
			{
				this.type,
				typeof(string)
			});
			this.displayValues = (IDictionary)Activator.CreateInstance(type);
			this.reverseValues = (IDictionary)Activator.CreateInstance(typeof(Dictionary<, >).GetGenericTypeDefinition().MakeGenericType(new Type[]
			{
				typeof(string),
				this.type
			}));
			FieldInfo[] fields = this.type.GetFields(BindingFlags.Static | BindingFlags.Public);
			foreach (FieldInfo fieldInfo in fields)
			{
				DescriptionAttribute[] array2 = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
				string displayStringValue = this.GetDisplayStringValue(array2);
				object value = fieldInfo.GetValue(null);
				if (displayStringValue != null)
				{
					this.displayValues.Add(value, displayStringValue);
					this.reverseValues.Add(displayStringValue, value);
				}
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060001A1 RID: 417 RVA: 0x00007E68 File Offset: 0x00006068
		public ReadOnlyCollection<string> DisplayNames
		{
			get
			{
				if (this.displayValues == null)
				{
					this.FillDisplayNames();
				}
				return new List<string>((IEnumerable<string>)this.displayValues.Values).AsReadOnly();
			}
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00007E94 File Offset: 0x00006094
		private string GetDisplayStringValue(DescriptionAttribute[] a)
		{
			if (a == null || a.Length == 0)
			{
				return null;
			}
			DescriptionAttribute descriptionAttribute = a[0];
			return descriptionAttribute.Description;
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00007EB5 File Offset: 0x000060B5
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return this.displayValues[value];
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00007EC3 File Offset: 0x000060C3
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return this.reverseValues[value];
		}

		// Token: 0x0400008A RID: 138
		private Type type;

		// Token: 0x0400008B RID: 139
		private IDictionary displayValues;

		// Token: 0x0400008C RID: 140
		private IDictionary reverseValues;
	}
}
