using System;
using System.ComponentModel;

namespace GimaSoft.Business.GINA
{
	// Token: 0x0200000A RID: 10
	public enum ArchiveSchedules
	{
		// Token: 0x04000030 RID: 48
		[Description("Daily")]
		Daily,
		// Token: 0x04000031 RID: 49
		[Description("Weekly (Monday)")]
		EveryMonday,
		// Token: 0x04000032 RID: 50
		[Description("Weekly (Tuesday)")]
		EveryTuesday,
		// Token: 0x04000033 RID: 51
		[Description("Weekly (Wednesday)")]
		EveryWednesday,
		// Token: 0x04000034 RID: 52
		[Description("Weekly (Thursday)")]
		EveryThursday,
		// Token: 0x04000035 RID: 53
		[Description("Weekly (Friday)")]
		EveryFriday,
		// Token: 0x04000036 RID: 54
		[Description("Weekly (Saturday)")]
		EverySaturday,
		// Token: 0x04000037 RID: 55
		[Description("Weekly (Sunday)")]
		EverySunday,
		// Token: 0x04000038 RID: 56
		[Description("Monthly")]
		Monthly,
		// Token: 0x04000039 RID: 57
		[Description("Quarterly")]
		Quarterly,
		// Token: 0x0400003A RID: 58
		[Description("Annually")]
		Annually
	}
}
