using System.Drawing;

namespace WPFShared
{
	// Token: 0x0200003B RID: 59
	public static class ColorExtensions
	{
		// Token: 0x060001D6 RID: 470 RVA: 0x00008714 File Offset: 0x00006914
		public static string ToHexString(this global::System.Windows.Media.Color color, bool includePound = true)
		{
			return string.Format("{0}{1:X2}{2:X2}{3:X2}{4:X2}", new object[]
			{
				includePound ? "#" : "",
				color.A,
				color.R,
				color.G,
				color.B
			});
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x00008784 File Offset: 0x00006984
		public static global::System.Windows.Media.Color SetFromHtml(this global::System.Windows.Media.Color color, string html, string defaultColor = "White")
		{
			global::System.Drawing.Color color2 = ColorTranslator.FromHtml(string.IsNullOrWhiteSpace(html) ? defaultColor : html);
			color.A = color2.A;
			color.B = color2.B;
			color.G = color2.G;
			color.R = color2.R;
			return color;
		}
	}
}
