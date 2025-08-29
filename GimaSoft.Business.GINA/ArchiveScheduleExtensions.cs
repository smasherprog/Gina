using System;
using System.Collections.Generic;
using System.Linq;

namespace GimaSoft.Business.GINA
{
	// Token: 0x0200000B RID: 11
	public static class ArchiveScheduleExtensions
	{
		// Token: 0x060000EC RID: 236 RVA: 0x000053B4 File Offset: 0x000035B4
		public static DateTime GetNextDate(this ArchiveSchedules schedule, DateTime baseDate)
		{
			List<DateTime> list = (from o in Enumerable.Range(0, 370)
				select baseDate.AddDays((double)o) into o
				where o > baseDate
				orderby o
				select o).ToList<DateTime>();
			switch (schedule)
			{
			case ArchiveSchedules.Daily:
				return list.First<DateTime>();
			case ArchiveSchedules.EveryMonday:
				return list.First((DateTime o) => o.DayOfWeek == DayOfWeek.Monday);
			case ArchiveSchedules.EveryTuesday:
				return list.First((DateTime o) => o.DayOfWeek == DayOfWeek.Tuesday);
			case ArchiveSchedules.EveryWednesday:
				return list.First((DateTime o) => o.DayOfWeek == DayOfWeek.Wednesday);
			case ArchiveSchedules.EveryThursday:
				return list.First((DateTime o) => o.DayOfWeek == DayOfWeek.Thursday);
			case ArchiveSchedules.EveryFriday:
				return list.First((DateTime o) => o.DayOfWeek == DayOfWeek.Friday);
			case ArchiveSchedules.EverySaturday:
				return list.First((DateTime o) => o.DayOfWeek == DayOfWeek.Saturday);
			case ArchiveSchedules.EverySunday:
				return list.First((DateTime o) => o.DayOfWeek == DayOfWeek.Sunday);
			case ArchiveSchedules.Monthly:
				return list.First((DateTime o) => o.Day == 1);
			case ArchiveSchedules.Quarterly:
				return list.First((DateTime o) => o.Month % 3 == 1 && o.Day == 1);
			case ArchiveSchedules.Annually:
				return list.First((DateTime o) => o.DayOfYear == 1);
			default:
				return baseDate.AddDays(1.0);
			}
		}
	}
}
