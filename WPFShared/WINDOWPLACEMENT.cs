using System;

namespace WPFShared
{
	// Token: 0x02000043 RID: 67
	[Serializable]
	public struct WINDOWPLACEMENT
	{
		// Token: 0x04000095 RID: 149
		public int length;

		// Token: 0x04000096 RID: 150
		public int flags;

		// Token: 0x04000097 RID: 151
		public int showCmd;

		// Token: 0x04000098 RID: 152
		public POINT minPosition;

		// Token: 0x04000099 RID: 153
		public POINT maxPosition;

		// Token: 0x0400009A RID: 154
		public RECT normalPosition;
	}
}
