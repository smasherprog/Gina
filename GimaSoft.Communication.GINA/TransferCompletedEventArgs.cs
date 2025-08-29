using System;

namespace GimaSoft.Communication.GINA
{
	// Token: 0x02000014 RID: 20
	public class TransferCompletedEventArgs : EventArgs
	{
		// Token: 0x06000045 RID: 69 RVA: 0x000021BF File Offset: 0x000003BF
		public TransferCompletedEventArgs(Guid sessionId)
		{
			this.SessionId = sessionId;
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000046 RID: 70 RVA: 0x000021CE File Offset: 0x000003CE
		// (set) Token: 0x06000047 RID: 71 RVA: 0x000021D6 File Offset: 0x000003D6
		public Guid SessionId { get; set; }
	}
}
