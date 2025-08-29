using System;
using System.Runtime.Serialization;

namespace GimaSoft.Service.GINA
{
	// Token: 0x02000006 RID: 6
	[DataContract]
	public class RepositorySubCategory
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000038 RID: 56 RVA: 0x000021CD File Offset: 0x000003CD
		// (set) Token: 0x06000039 RID: 57 RVA: 0x000021D5 File Offset: 0x000003D5
		[DataMember]
		public int SubCategoryId { get; set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600003A RID: 58 RVA: 0x000021DE File Offset: 0x000003DE
		// (set) Token: 0x0600003B RID: 59 RVA: 0x000021E6 File Offset: 0x000003E6
		[DataMember]
		public string SubCategoryName { get; set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600003C RID: 60 RVA: 0x000021EF File Offset: 0x000003EF
		// (set) Token: 0x0600003D RID: 61 RVA: 0x000021F7 File Offset: 0x000003F7
		[DataMember]
		public string CategoryName { get; set; }
	}
}
