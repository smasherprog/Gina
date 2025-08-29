using System;
using System.ComponentModel;

namespace GimaSoft.Business.GINA
{
	// Token: 0x02000015 RID: 21
	public enum TimerTypes
	{
		// Token: 0x0400006A RID: 106
		[Description("No Timer")]
		NoTimer,
		// Token: 0x0400006B RID: 107
		[Description("Timer (Count Down)")]
		Timer,
		// Token: 0x0400006C RID: 108
		[Description("Stopwatch (Count Up)")]
		Stopwatch,
		// Token: 0x0400006D RID: 109
		[Description("Repeating Timer")]
		RepeatingTimer
	}
}
