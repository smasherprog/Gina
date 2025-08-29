using System;

namespace GimaSoft.Communication.GINA
{
	// Token: 0x02000004 RID: 4
	public class ChunkTransferredEventArgs
	{
		// Token: 0x0600000C RID: 12 RVA: 0x000020A0 File Offset: 0x000002A0
		public ChunkTransferredEventArgs(Guid sessionId, int totalTransferred, int totalSize)
		{
			this.SessionId = sessionId;
			this.TotalTransferred = totalTransferred;
			this.TotalSize = totalSize;
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000D RID: 13 RVA: 0x000020BD File Offset: 0x000002BD
		// (set) Token: 0x0600000E RID: 14 RVA: 0x000020C5 File Offset: 0x000002C5
		public Guid SessionId { get; set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000F RID: 15 RVA: 0x000020CE File Offset: 0x000002CE
		// (set) Token: 0x06000010 RID: 16 RVA: 0x000020D6 File Offset: 0x000002D6
		public int TotalTransferred { get; set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000011 RID: 17 RVA: 0x000020DF File Offset: 0x000002DF
		// (set) Token: 0x06000012 RID: 18 RVA: 0x000020E7 File Offset: 0x000002E7
		public int TotalSize { get; set; }
	}
}
