using System;
using GimaSoft.Service.GINA;

namespace GimaSoft.Communication.GINA
{
	// Token: 0x02000008 RID: 8
	public class ConnectionEstablishedEventArgs : EventArgs
	{
		// Token: 0x0600001E RID: 30 RVA: 0x00002119 File Offset: 0x00000319
		public ConnectionEstablishedEventArgs(ConnectionInfo connectionInfo)
		{
			this.ConnectionInfo = connectionInfo;
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600001F RID: 31 RVA: 0x00002128 File Offset: 0x00000328
		// (set) Token: 0x06000020 RID: 32 RVA: 0x00002130 File Offset: 0x00000330
		public Guid SessionId { get; set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000021 RID: 33 RVA: 0x00002139 File Offset: 0x00000339
		// (set) Token: 0x06000022 RID: 34 RVA: 0x00002141 File Offset: 0x00000341
		public ConnectionInfo ConnectionInfo { get; set; }
	}
}
