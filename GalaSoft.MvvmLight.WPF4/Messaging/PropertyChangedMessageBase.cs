using System;

namespace GalaSoft.MvvmLight.Messaging
{
	// Token: 0x02000004 RID: 4
	public abstract class PropertyChangedMessageBase : MessageBase
	{
		// Token: 0x06000009 RID: 9 RVA: 0x00002144 File Offset: 0x00000344
		protected PropertyChangedMessageBase(object sender, string propertyName)
			: base(sender)
		{
			this.PropertyName = propertyName;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002158 File Offset: 0x00000358
		protected PropertyChangedMessageBase(object sender, object target, string propertyName)
			: base(sender, target)
		{
			this.PropertyName = propertyName;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x0000216D File Offset: 0x0000036D
		protected PropertyChangedMessageBase(string propertyName)
		{
			this.PropertyName = propertyName;
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000C RID: 12 RVA: 0x00002180 File Offset: 0x00000380
		// (set) Token: 0x0600000D RID: 13 RVA: 0x00002197 File Offset: 0x00000397
		public string PropertyName { get; protected set; }
	}
}
