using System;
using GimaSoft.Business.GINA;
using GimaSoft.Service.GINA;
using WPFShared;

namespace GimaSoft.GINA
{
	// Token: 0x02000004 RID: 4
	public class RepositoryEntryFacade : BindableObject
	{
		// Token: 0x0600004E RID: 78 RVA: 0x00002DC6 File Offset: 0x00000FC6
		public RepositoryEntryFacade(RepositoryEntry entry)
		{
			this.Entry = entry;
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600004F RID: 79 RVA: 0x00002DD5 File Offset: 0x00000FD5
		// (set) Token: 0x06000050 RID: 80 RVA: 0x00002DDD File Offset: 0x00000FDD
		public RepositoryEntry Entry { get; set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000051 RID: 81 RVA: 0x00002DE6 File Offset: 0x00000FE6
		// (set) Token: 0x06000052 RID: 82 RVA: 0x00002DF3 File Offset: 0x00000FF3
		public bool IsSelected
		{
			get
			{
				return base.Get<bool>("IsSelected");
			}
			set
			{
				base.Set("IsSelected", value);
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000053 RID: 83 RVA: 0x00002E06 File Offset: 0x00001006
		public string FullCategory
		{
			get
			{
				return string.Format("{0} / {1}", this.Entry.Category, this.Entry.SubCategory);
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000054 RID: 84 RVA: 0x00002E28 File Offset: 0x00001028
		public bool IsNew
		{
			get
			{
				return Configuration.Current.RepositoryLastViewedAtStartup != DateTimeOffset.MinValue && this.Entry.ModifiedGMT >= Configuration.Current.RepositoryLastViewedAtStartup;
			}
		}
	}
}
