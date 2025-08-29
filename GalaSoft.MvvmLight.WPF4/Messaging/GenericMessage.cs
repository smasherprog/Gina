using System;

namespace GalaSoft.MvvmLight.Messaging
{
	// Token: 0x02000009 RID: 9
	public class GenericMessage<T> : MessageBase
	{
		// Token: 0x06000031 RID: 49 RVA: 0x00002A94 File Offset: 0x00000C94
		public GenericMessage(T content)
		{
			this.Content = content;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002AA7 File Offset: 0x00000CA7
		public GenericMessage(object sender, T content)
			: base(sender)
		{
			this.Content = content;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002ABB File Offset: 0x00000CBB
		public GenericMessage(object sender, object target, T content)
			: base(sender, target)
		{
			this.Content = content;
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000034 RID: 52 RVA: 0x00002AD0 File Offset: 0x00000CD0
		// (set) Token: 0x06000035 RID: 53 RVA: 0x00002AE7 File Offset: 0x00000CE7
		public T Content { get; protected set; }
	}
}
