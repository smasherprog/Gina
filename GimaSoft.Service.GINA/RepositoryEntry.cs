using System;
using System.Runtime.Serialization;

namespace GimaSoft.Service.GINA
{
	// Token: 0x02000005 RID: 5
	[DataContract]
	public class RepositoryEntry
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000021 RID: 33 RVA: 0x0000210A File Offset: 0x0000030A
		// (set) Token: 0x06000022 RID: 34 RVA: 0x00002112 File Offset: 0x00000312
		[DataMember]
		public Guid RepositoryId { get; set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000023 RID: 35 RVA: 0x0000211B File Offset: 0x0000031B
		// (set) Token: 0x06000024 RID: 36 RVA: 0x00002123 File Offset: 0x00000323
		[DataMember]
		public string Credits { get; set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000025 RID: 37 RVA: 0x0000212C File Offset: 0x0000032C
		// (set) Token: 0x06000026 RID: 38 RVA: 0x00002134 File Offset: 0x00000334
		[DataMember]
		public string Category { get; set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000027 RID: 39 RVA: 0x0000213D File Offset: 0x0000033D
		// (set) Token: 0x06000028 RID: 40 RVA: 0x00002145 File Offset: 0x00000345
		[DataMember]
		public string SubCategory { get; set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000029 RID: 41 RVA: 0x0000214E File Offset: 0x0000034E
		// (set) Token: 0x0600002A RID: 42 RVA: 0x00002156 File Offset: 0x00000356
		[DataMember]
		public string Name { get; set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600002B RID: 43 RVA: 0x0000215F File Offset: 0x0000035F
		// (set) Token: 0x0600002C RID: 44 RVA: 0x00002167 File Offset: 0x00000367
		[DataMember]
		public string Comments { get; set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600002D RID: 45 RVA: 0x00002170 File Offset: 0x00000370
		// (set) Token: 0x0600002E RID: 46 RVA: 0x00002178 File Offset: 0x00000378
		[DataMember]
		public bool IsTested { get; set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600002F RID: 47 RVA: 0x00002181 File Offset: 0x00000381
		// (set) Token: 0x06000030 RID: 48 RVA: 0x00002189 File Offset: 0x00000389
		[Obsolete]
		[DataMember]
		public DateTime Created { get; set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000031 RID: 49 RVA: 0x00002192 File Offset: 0x00000392
		// (set) Token: 0x06000032 RID: 50 RVA: 0x0000219A File Offset: 0x0000039A
		[Obsolete]
		[DataMember]
		public DateTime Modified { get; set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000033 RID: 51 RVA: 0x000021A3 File Offset: 0x000003A3
		// (set) Token: 0x06000034 RID: 52 RVA: 0x000021AB File Offset: 0x000003AB
		[DataMember]
		public DateTimeOffset CreatedGMT { get; set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000035 RID: 53 RVA: 0x000021B4 File Offset: 0x000003B4
		// (set) Token: 0x06000036 RID: 54 RVA: 0x000021BC File Offset: 0x000003BC
		[DataMember]
		public DateTimeOffset ModifiedGMT { get; set; }
	}
}
