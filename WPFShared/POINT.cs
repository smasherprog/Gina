using System;

namespace WPFShared
{
	// Token: 0x02000042 RID: 66
	[Serializable]
	public struct POINT
	{
		// Token: 0x060001E3 RID: 483 RVA: 0x00008ABD File Offset: 0x00006CBD
		public POINT(int x, int y)
		{
			this.X = x;
			this.Y = y;
		}

		// Token: 0x04000093 RID: 147
		public int X;

		// Token: 0x04000094 RID: 148
		public int Y;
	}
}
