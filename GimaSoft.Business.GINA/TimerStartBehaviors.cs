using System;
using System.ComponentModel;

namespace GimaSoft.Business.GINA
{
	// Token: 0x02000014 RID: 20
	public enum TimerStartBehaviors
	{
		// Token: 0x04000066 RID: 102
		[Description("Start a new timer")]
		StartNewTimer,
		// Token: 0x04000067 RID: 103
		[Description("Restart current timer")]
		RestartTimer,
		// Token: 0x04000068 RID: 104
		[Description("Do nothing")]
		IgnoreIfRunning
	}
}
