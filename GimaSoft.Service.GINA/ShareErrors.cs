using System;
using System.ComponentModel;

namespace GimaSoft.Service.GINA
{
	// Token: 0x02000007 RID: 7
	public enum ShareErrors
	{
		// Token: 0x0400001A RID: 26
		[Description("No error")]
		None,
		// Token: 0x0400001B RID: 27
		[Description("Unknown")]
		Unknown,
		// Token: 0x0400001C RID: 28
		[Description("Package too large")]
		TooLarge,
		// Token: 0x0400001D RID: 29
		[Description("Invalid package")]
		InvalidPackage,
		// Token: 0x0400001E RID: 30
		[Description("Invalid session")]
		InvalidSession,
		// Token: 0x0400001F RID: 31
		[Description("Error reading file")]
		FileRead,
		// Token: 0x04000020 RID: 32
		[Description("Error writing file")]
		FileWrite,
		// Token: 0x04000021 RID: 33
		[Description("Error reading repository")]
		RepositoryRead,
		// Token: 0x04000022 RID: 34
		[Description("Invalid login credentials")]
		InvalidLogin
	}
}
