using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using BusinessShared;
using WPFShared;

namespace GimaSoft.Business.GINA
{
	// Token: 0x02000034 RID: 52
	public class MatchHelper : BindableObject
	{
		// Token: 0x06000243 RID: 579 RVA: 0x0000ADCC File Offset: 0x00008FCC
		public static bool RegexRequired(string str)
		{
			return !string.IsNullOrWhiteSpace(str) && (Regex.IsMatch(str, "\\{s\\d*\\}", RegexOptions.IgnoreCase) || Regex.IsMatch(str, "\\{n\\d*[\\>\\=\\<]*\\d*\\}", RegexOptions.IgnoreCase) || str.ToLower().Contains("{ts}"));
		}

		// Token: 0x06000244 RID: 580 RVA: 0x0000AE08 File Offset: 0x00009008
		public static bool RegexValid(string str)
		{
			bool flag = false;
			try
			{
				new Regex(str, RegexOptions.IgnoreCase);
				flag = true;
			}
			catch
			{
			}
			return flag;
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0000AE38 File Offset: 0x00009038
		public MatchHelper(GINACharacter character, string pattern, bool useRegex, bool useFastCheck)
		{
			this.Character = character;
			this.GenerateFilter(pattern, useRegex, useFastCheck);
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000246 RID: 582 RVA: 0x0000AE5C File Offset: 0x0000905C
		// (set) Token: 0x06000247 RID: 583 RVA: 0x0000AE64 File Offset: 0x00009064
		private string SearchText { get; set; }

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000248 RID: 584 RVA: 0x0000AE6D File Offset: 0x0000906D
		// (set) Token: 0x06000249 RID: 585 RVA: 0x0000AE75 File Offset: 0x00009075
		private Regex RegExp { get; set; }

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x0600024A RID: 586 RVA: 0x0000AE7E File Offset: 0x0000907E
		// (set) Token: 0x0600024B RID: 587 RVA: 0x0000AE86 File Offset: 0x00009086
		public GINACharacter Character { get; set; }

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x0600024C RID: 588 RVA: 0x0000AE8F File Offset: 0x0000908F
		// (set) Token: 0x0600024D RID: 589 RVA: 0x0000AE97 File Offset: 0x00009097
		private bool HasTimerSpan { get; set; }

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x0600024E RID: 590 RVA: 0x0000AEA0 File Offset: 0x000090A0
		// (set) Token: 0x0600024F RID: 591 RVA: 0x0000AEA8 File Offset: 0x000090A8
		public string RegExpPreview
		{
			get
			{
				return this._RegExpPreview;
			}
			set
			{
				this._RegExpPreview = value;
				base.RaisePropertyChanged("RegExpPreview");
			}
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0000AF44 File Offset: 0x00009144
		public void GenerateFilter(string pattern, bool useRegex, bool useFastCheck)
		{
			lock (this)
			{
				if (string.IsNullOrWhiteSpace(pattern))
				{
					this.SearchText = null;
					this.RegExp = null;
					this.RegExpPreview = null;
				}
				else
				{
					this.SearchText = pattern.ToLower().Replace("{c}", (this.Character != null) ? this.Character.Name.ToLower() : "");
					this.RegExpPreview = null;
					if (useRegex && this.SearchText.Contains("{ts}"))
					{
						this.HasTimerSpan = true;
						this.SearchText = this.SearchText.ReplaceInstance("{ts}", "((((?<GTS_D>\\d+):){0,1}((?<GTS_H>\\d+):)){0,1}((?<GTS_M>\\d+):)){0,1}(?<GTS_S>\\d+){1}(\\.(?<GTS_MS>\\d+)){0,1}", 1, true);
					}
					this.STokens = new List<KeyValuePair<string, string>>();
					foreach (Match match in MatchHelper.STokenRegex.Matches(this.SearchText).Cast<Match>())
					{
						string value = match.Groups["stoken"].Value;
						this.STokens.Add(new KeyValuePair<string, string>(value, string.Format("\\{{{0}\\}}", value)));
						this.SearchText = this.SearchText.Replace("{" + value + "}", string.Format("(?<{0}>.+)", value));
					}
					this.NTokens = new List<MatchHelper.NTokenData>();
					foreach (Match match2 in from Match o in MatchHelper.NTokenRegex.Matches(this.SearchText)
						orderby o.Index descending
						select o)
					{
						MatchHelper.NTokenData ntokenData = new MatchHelper.NTokenData(match2.Groups["ntoken"].Value, match2.Groups["ops"].Value, match2.Groups["val"].Value);
						this.NTokens.Add(ntokenData);
						this.SearchText = this.SearchText.Remove(match2.Index, match2.Length).Insert(match2.Index, string.Format("(?<{0}>\\d+)", ntokenData.NToken));
					}
					this.RegExp = ((useRegex || this.STokens.Any<KeyValuePair<string, string>>() || this.NTokens.Any<MatchHelper.NTokenData>()) ? new Regex(this.SearchText, RegexOptions.IgnoreCase | RegexOptions.Compiled) : null);
					if (this.RegExp != null)
					{
						this.RegExpGroupNames = (from o in this.RegExp.GetGroupNames()
							where o != "0"
							select new KeyValuePair<string, string>(o, string.Format("\\$\\{{{0}\\}}", o.ToLower()))).ToList<KeyValuePair<string, string>>();
						this.RegExpGroupNumbers = (from o in this.RegExp.GetGroupNumbers()
							select new KeyValuePair<int, string[]>(o, new string[]
							{
								string.Format("\\$\\{{{0}\\}}", o),
								string.Format("\\${0}", o)
							})).ToList<KeyValuePair<int, string[]>>();
						if (useFastCheck)
						{
							string text = this.SearchText.ToLower();
							text = Regex.Replace(text, "\\\\[^\\\\]", "-");
							text = Regex.Replace(text, "\\\\\\\\", "-");
							string text2;
							do
							{
								text2 = text;
								text = Regex.Replace(text, "\\([^\\(\\)]+\\)", "-");
								text = Regex.Replace(text, "\\[[^\\[\\]]+\\]", "-");
							}
							while (text2 != text);
							IOrderedEnumerable<string> orderedEnumerable = from Match o in Regex.Matches(text, "[\\w,:'\\s]+")
								select o.Value.ToLower() into o
								orderby o.Length descending
								select o;
							string text3 = orderedEnumerable.FirstOrDefault<string>();
							if (text3 != null && text3.Length >= 8)
							{
								this.RegExpPreview = text3.Substring(0, 8);
							}
						}
					}
				}
			}
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0000B3B4 File Offset: 0x000095B4
		public bool IsMatch(string str)
		{
			if (string.IsNullOrWhiteSpace(this.SearchText))
			{
				return false;
			}
			bool flag = false;
			str = str.ToLower();
			if (this.RegExp == null)
			{
				flag = str.Contains(this.SearchText);
			}
			else if (this.RegExpPreview == null || str.Contains(this.RegExpPreview))
			{
				Match match = this.RegExp.Match(str);
				flag = match.Success;
				if (match.Success)
				{
					foreach (MatchHelper.NTokenData ntokenData in this.NTokens)
					{
						flag &= ntokenData.IsMatch(match.Groups[ntokenData.NToken].Value);
					}
				}
			}
			return flag;
		}

		// Token: 0x06000252 RID: 594 RVA: 0x0000B488 File Offset: 0x00009688
		public string ResolveText(string srcLine, string destLine, bool isVoice = false)
		{
			destLine = Regex.Replace(destLine ?? "", "\\{C\\}", (this.Character != null) ? this.Character.Name : "", RegexOptions.IgnoreCase);
			destLine = Regex.Replace(destLine ?? "", "\\{L\\}", srcLine ?? "", RegexOptions.IgnoreCase);
			if (this.RegExp != null)
			{
				Match match = this.RegExp.Match(srcLine);
				if (match.Success && destLine.Contains("{"))
				{
					foreach (KeyValuePair<string, string> keyValuePair in this.RegExpGroupNames)
					{
						destLine = Regex.Replace(destLine, keyValuePair.Value, match.Groups[keyValuePair.Key].Value, RegexOptions.IgnoreCase);
					}
					if (MatchHelper.NumberedGroupRegex.IsMatch(destLine))
					{
						foreach (KeyValuePair<int, string[]> keyValuePair2 in this.RegExpGroupNumbers)
						{
							foreach (string text in keyValuePair2.Value)
							{
								destLine = Regex.Replace(destLine, text, match.Groups[keyValuePair2.Key].Value, RegexOptions.IgnoreCase);
							}
						}
					}
					foreach (KeyValuePair<string, string> keyValuePair3 in this.STokens)
					{
						destLine = Regex.Replace(destLine, keyValuePair3.Value, match.Groups[keyValuePair3.Key].Value, RegexOptions.IgnoreCase);
					}
					foreach (MatchHelper.NTokenData ntokenData in this.NTokens)
					{
						destLine = Regex.Replace(destLine, ntokenData.ReplacementToken, match.Groups[ntokenData.NToken].Value, RegexOptions.IgnoreCase);
					}
				}
			}
			if (isVoice)
			{
				foreach (PhoneticTransform phoneticTransform in Configuration.Current.PhoneticDictionary)
				{
					destLine = destLine.ReplaceWord(phoneticTransform.FastActualWord, phoneticTransform.FastPhoneticWord);
				}
			}
			return destLine;
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0000B730 File Offset: 0x00009930
		public long? ResolveTimerSpanToMilliseconds(string srcLine)
		{
			if (!this.HasTimerSpan)
			{
				return null;
			}
			Match match = this.RegExp.Match(srcLine);
			if (!match.Success)
			{
				return null;
			}
			long? num;
			try
			{
				num = new long?(86400000L * (match.Groups["GTS_D"].Success ? long.Parse(match.Groups["GTS_D"].Value) : 0L) + 3600000L * (match.Groups["GTS_H"].Success ? long.Parse(match.Groups["GTS_H"].Value) : 0L) + 60000L * (match.Groups["GTS_M"].Success ? long.Parse(match.Groups["GTS_M"].Value) : 0L) + 1000L * long.Parse(match.Groups["GTS_S"].Value) + (match.Groups["GTS_MS"].Success ? long.Parse(match.Groups["GTS_MS"].Value) : 0L));
			}
			catch
			{
				try
				{
					num = new long?((long)TimeSpan.Parse(match.Groups["GINATimerSpan"].Value).TotalMilliseconds);
				}
				catch
				{
					num = null;
				}
			}
			return num;
		}

		// Token: 0x040000FE RID: 254
		private static Regex STokenRegex = new Regex("\\{(?<stoken>s\\d*)\\}", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		// Token: 0x040000FF RID: 255
		private static Regex NTokenRegex = new Regex("\\{(?<ntoken>n\\d*)(?<ops>[\\>\\=\\<]*)(?<val>\\d*)\\}", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		// Token: 0x04000100 RID: 256
		private static Regex NumberedGroupRegex = new Regex("\\$\\{{0,1}\\d", RegexOptions.Compiled);

		// Token: 0x04000101 RID: 257
		private List<KeyValuePair<string, string>> RegExpGroupNames;

		// Token: 0x04000102 RID: 258
		private List<KeyValuePair<int, string[]>> RegExpGroupNumbers;

		// Token: 0x04000103 RID: 259
		private List<KeyValuePair<string, string>> STokens;

		// Token: 0x04000104 RID: 260
		private List<MatchHelper.NTokenData> NTokens = new List<MatchHelper.NTokenData>();

		// Token: 0x04000105 RID: 261
		private string _RegExpPreview;

		// Token: 0x02000035 RID: 53
		private class NTokenData
		{
			// Token: 0x0600025B RID: 603 RVA: 0x0000B928 File Offset: 0x00009B28
			public NTokenData(string ntoken, string operands, string known)
			{
				this.NToken = ntoken;
				this.ReplacementToken = string.Format("\\{{{0}\\}}", ntoken);
				this.Operand = OperandsExtensions.FromString(operands);
				int num = 0;
				this.Known = (int.TryParse(known, out num) ? new int?(num) : null);
			}

			// Token: 0x0600025C RID: 604 RVA: 0x0000B982 File Offset: 0x00009B82
			public bool IsMatch(string value)
			{
				return this.Known == null || this.Operand.IsMatch(value, this.Known.Value);
			}

			// Token: 0x04000110 RID: 272
			public string NToken;

			// Token: 0x04000111 RID: 273
			public string ReplacementToken;

			// Token: 0x04000112 RID: 274
			public Operands Operand;

			// Token: 0x04000113 RID: 275
			public int? Known;
		}
	}
}
