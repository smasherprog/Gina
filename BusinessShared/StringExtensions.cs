using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BusinessShared
{
	// Token: 0x02000003 RID: 3
	public static class StringExtensions
	{
		// Token: 0x06000002 RID: 2 RVA: 0x00002077 File Offset: 0x00000277
		public static bool Contains(this string source, string toCheck, bool ignoreCase)
		{
			return source.IndexOf(toCheck, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal) >= 0;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002090 File Offset: 0x00000290
		public static string ReplaceWord(this string original, string pattern, string replacement)
		{
			int num2;
			int num = (num2 = 0);
			string text = original.ToUpper();
			string text2 = pattern.ToUpper();
			if (text.IndexOf(text2) == -1)
			{
				return original;
			}
			int num3 = original.Length / pattern.Length * (replacement.Length - pattern.Length);
			char[] array = new char[original.Length + Math.Max(0, num3)];
			int num4;
			while ((num4 = text.IndexOf(text2, num)) != -1)
			{
				for (int i = num; i < num4; i++)
				{
					array[num2++] = original[i];
				}
				if ((num4 == 0 || text[num4 - 1] < 'A' || text[num4 - 1] > 'Z') && (num4 + pattern.Length == original.Length || text[num4 + pattern.Length] < 'A' || text[num4 + pattern.Length] > 'Z'))
				{
					for (int j = 0; j < replacement.Length; j++)
					{
						array[num2++] = replacement[j];
					}
				}
				else
				{
					for (int k = 0; k < pattern.Length; k++)
					{
						array[num2++] = original[num4 + k];
					}
				}
				num = num4 + pattern.Length;
			}
			if (num == 0)
			{
				return original;
			}
			for (int l = num; l < original.Length; l++)
			{
				array[num2++] = original[l];
			}
			return new string(array, 0, num2);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002204 File Offset: 0x00000404
		public static string ReplaceInstance(this string original, string pattern, string replacement, int instance, bool ignoreCase = true)
		{
			int num = original.IndexOf(pattern, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
			if (num <= 0)
			{
				return original;
			}
			return original.Substring(0, num) + replacement + original.Substring(num + pattern.Length);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002243 File Offset: 0x00000443
		public static string Stuff(this string original, int startIndex, int length, string replacement)
		{
			return ((startIndex > 0) ? original.Substring(0, startIndex - 1) : "") + replacement + ((startIndex + length > original.Length) ? "" : original.Substring(startIndex + length));
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002378 File Offset: 0x00000578
		private static IEnumerable<string> GraphemeClusters(this string s)
		{
			TextElementEnumerator enumerator = StringInfo.GetTextElementEnumerator(s);
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				yield return (string)obj;
			}
			yield break;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002395 File Offset: 0x00000595
		public static string Reverse(this string original)
		{
			return string.Join("", original.GraphemeClusters().Reverse<string>().ToArray<string>());
		}
	}
}
