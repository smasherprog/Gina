using System;

namespace WPFShared
{
	// Token: 0x02000040 RID: 64
	public class UniqueNameGenerator : BindableObject
	{
		// Token: 0x060001DF RID: 479 RVA: 0x00008A58 File Offset: 0x00006C58
		public UniqueNameGenerator()
		{
			this.Name = Guid.NewGuid().ToString("N");
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060001E0 RID: 480 RVA: 0x00008A83 File Offset: 0x00006C83
		// (set) Token: 0x060001E1 RID: 481 RVA: 0x00008A90 File Offset: 0x00006C90
		public string Name
		{
			get
			{
				return base.Get<string>("Name");
			}
			set
			{
				base.Set("Name", value);
			}
		}
	}
}
