using System;

namespace GimaSoft.Communication.GINA
{
	// Token: 0x02000006 RID: 6
	public class ConnectionErrorEventArgs : EventArgs
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000017 RID: 23 RVA: 0x000020F0 File Offset: 0x000002F0
		// (set) Token: 0x06000018 RID: 24 RVA: 0x000020F8 File Offset: 0x000002F8
		public string ErrorMessage { get; set; }

		// Token: 0x06000019 RID: 25 RVA: 0x00002101 File Offset: 0x00000301
		public ConnectionErrorEventArgs(string errorMessage)
		{
			this.ErrorMessage = errorMessage ?? "Unknown";
		}
	}
}
