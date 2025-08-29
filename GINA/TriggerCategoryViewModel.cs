using System;
using GimaSoft.Business.GINA;

namespace GimaSoft.GINA
{
	// Token: 0x02000022 RID: 34
	public class TriggerCategoryViewModel : GINAViewModel
	{
		// Token: 0x17000153 RID: 339
		// (get) Token: 0x0600038A RID: 906 RVA: 0x0000C1CC File Offset: 0x0000A3CC
		// (set) Token: 0x0600038B RID: 907 RVA: 0x0000C1D9 File Offset: 0x0000A3D9
		public TriggerCategory Category
		{
			get
			{
				return base.Get<TriggerCategory>("Category");
			}
			set
			{
				base.Set("Category", value);
			}
		}
	}
}
