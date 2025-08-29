using System;
using GimaSoft.Service.GINA;

namespace GimaSoft.Communication.GINA
{
	// Token: 0x02000002 RID: 2
	public class ChunkFailedEventArgs : EventArgs
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public ChunkFailedEventArgs(Guid sessionId, ShareErrors error, int totalSize)
		{
			this.SessionId = sessionId;
			this.Error = error;
			this.TotalSize = totalSize;
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000002 RID: 2 RVA: 0x0000206D File Offset: 0x0000026D
		// (set) Token: 0x06000003 RID: 3 RVA: 0x00002075 File Offset: 0x00000275
		public Guid SessionId { get; set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000004 RID: 4 RVA: 0x0000207E File Offset: 0x0000027E
		// (set) Token: 0x06000005 RID: 5 RVA: 0x00002086 File Offset: 0x00000286
		public int TotalSize { get; set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000006 RID: 6 RVA: 0x0000208F File Offset: 0x0000028F
		// (set) Token: 0x06000007 RID: 7 RVA: 0x00002097 File Offset: 0x00000297
		public ShareErrors Error { get; set; }
	}
}
