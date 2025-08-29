using System;

namespace GimaSoft.Business.GINA
{
	// Token: 0x02000016 RID: 22
	public static class TimerTypesExtensions
	{
		// Token: 0x060000F8 RID: 248 RVA: 0x000055DF File Offset: 0x000037DF
		public static bool CountsDown(this TimerTypes timerType)
		{
			return timerType == TimerTypes.Timer || timerType == TimerTypes.RepeatingTimer;
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x000055EB File Offset: 0x000037EB
		public static bool CountsUp(this TimerTypes timerType)
		{
			return timerType == TimerTypes.Stopwatch;
		}
	}
}
