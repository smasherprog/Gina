using System;

namespace WPFShared
{
	// Token: 0x02000041 RID: 65
	[Serializable]
	public struct RECT
	{
		// Token: 0x060001E2 RID: 482 RVA: 0x00008A9E File Offset: 0x00006C9E
		public RECT(int left, int top, int right, int bottom)
		{
			this.Left = left;
			this.Top = top;
			this.Right = right;
			this.Bottom = bottom;
		}

		// Token: 0x0400008F RID: 143
		public int Left;

		// Token: 0x04000090 RID: 144
		public int Top;

		// Token: 0x04000091 RID: 145
		public int Right;

		// Token: 0x04000092 RID: 146
		public int Bottom;
	}
}
