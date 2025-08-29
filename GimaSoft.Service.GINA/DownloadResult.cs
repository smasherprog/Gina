using System;
using System.Runtime.Serialization;

namespace GimaSoft.Service.GINA
{
	// Token: 0x02000003 RID: 3
	[DataContract]
	public class DownloadResult
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000A RID: 10 RVA: 0x0000209C File Offset: 0x0000029C
		// (set) Token: 0x0600000B RID: 11 RVA: 0x000020A4 File Offset: 0x000002A4
		[DataMember]
		public bool Success { get; set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000C RID: 12 RVA: 0x000020AD File Offset: 0x000002AD
		// (set) Token: 0x0600000D RID: 13 RVA: 0x000020B5 File Offset: 0x000002B5
		[DataMember]
		public ShareErrors Error { get; set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600000E RID: 14 RVA: 0x000020BE File Offset: 0x000002BE
		// (set) Token: 0x0600000F RID: 15 RVA: 0x000020C6 File Offset: 0x000002C6
		[DataMember]
		public int? ChunkNumber { get; set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000010 RID: 16 RVA: 0x000020CF File Offset: 0x000002CF
		// (set) Token: 0x06000011 RID: 17 RVA: 0x000020D7 File Offset: 0x000002D7
		[DataMember]
		public int? TotalSize { get; set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000012 RID: 18 RVA: 0x000020E0 File Offset: 0x000002E0
		// (set) Token: 0x06000013 RID: 19 RVA: 0x000020E8 File Offset: 0x000002E8
		[DataMember]
		public byte[] ChunkData { get; set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000014 RID: 20 RVA: 0x000020F1 File Offset: 0x000002F1
		// (set) Token: 0x06000015 RID: 21 RVA: 0x000020F9 File Offset: 0x000002F9
		[DataMember]
		public Guid SessionId { get; set; }
	}
}
