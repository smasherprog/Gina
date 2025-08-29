using System;
using System.Runtime.Serialization;

namespace GimaSoft.Service.GINA
{
	// Token: 0x02000008 RID: 8
	[DataContract]
	public class UploadResult
	{
		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600003F RID: 63 RVA: 0x00002208 File Offset: 0x00000408
		// (set) Token: 0x06000040 RID: 64 RVA: 0x00002210 File Offset: 0x00000410
		[DataMember]
		public bool Success { get; set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000041 RID: 65 RVA: 0x00002219 File Offset: 0x00000419
		// (set) Token: 0x06000042 RID: 66 RVA: 0x00002221 File Offset: 0x00000421
		[DataMember]
		public ShareErrors Error { get; set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000043 RID: 67 RVA: 0x0000222A File Offset: 0x0000042A
		// (set) Token: 0x06000044 RID: 68 RVA: 0x00002232 File Offset: 0x00000432
		[DataMember]
		public Guid SessionId { get; set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000045 RID: 69 RVA: 0x0000223B File Offset: 0x0000043B
		// (set) Token: 0x06000046 RID: 70 RVA: 0x00002243 File Offset: 0x00000443
		[DataMember]
		public int MaxUploadSize { get; set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000047 RID: 71 RVA: 0x0000224C File Offset: 0x0000044C
		// (set) Token: 0x06000048 RID: 72 RVA: 0x00002254 File Offset: 0x00000454
		[DataMember]
		public int ChunkSize { get; set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000049 RID: 73 RVA: 0x0000225D File Offset: 0x0000045D
		// (set) Token: 0x0600004A RID: 74 RVA: 0x00002265 File Offset: 0x00000465
		[DataMember]
		public int CumulativeBytesUploaded { get; set; }
	}
}
