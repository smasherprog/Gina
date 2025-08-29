using System;

namespace GimaSoft.Communication.GINA
{
	// Token: 0x0200000C RID: 12
	public class ConnectionVersionFailedEventArgs : EventArgs
	{
		// Token: 0x0600002C RID: 44 RVA: 0x00002152 File Offset: 0x00000352
		public ConnectionVersionFailedEventArgs(Version requiredVersion)
		{
			this.RequiredVersion = requiredVersion;
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600002D RID: 45 RVA: 0x00002161 File Offset: 0x00000361
		// (set) Token: 0x0600002E RID: 46 RVA: 0x00002169 File Offset: 0x00000369
		public Version RequiredVersion { get; set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600002F RID: 47 RVA: 0x00002172 File Offset: 0x00000372
		public string VersionMessage
		{
			get
			{
				return "You must update to version " + this.RequiredVersion.ToString() + " or later to use the GimaLink services.";
			}
		}
	}
}
