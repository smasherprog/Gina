using System;

namespace GimaSoft.Communication.GINA
{
	// Token: 0x0200000E RID: 14
	public class ErrorReportUploadedEventArgs : EventArgs
	{
		// Token: 0x06000034 RID: 52 RVA: 0x0000218E File Offset: 0x0000038E
		public ErrorReportUploadedEventArgs(bool successful)
		{
			successful = this.Successful;
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000035 RID: 53 RVA: 0x0000219E File Offset: 0x0000039E
		// (set) Token: 0x06000036 RID: 54 RVA: 0x000021A6 File Offset: 0x000003A6
		public bool Successful { get; set; }
	}
}
