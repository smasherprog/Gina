using System;

namespace GalaSoft.MvvmLight.Messaging
{
	// Token: 0x0200000A RID: 10
	public class NotificationMessage<T> : GenericMessage<T>
	{
		// Token: 0x06000036 RID: 54 RVA: 0x00002AF0 File Offset: 0x00000CF0
		public NotificationMessage(T content, string notification)
			: base(content)
		{
			this.Notification = notification;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002B04 File Offset: 0x00000D04
		public NotificationMessage(object sender, T content, string notification)
			: base(sender, content)
		{
			this.Notification = notification;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002B19 File Offset: 0x00000D19
		public NotificationMessage(object sender, object target, T content, string notification)
			: base(sender, target, content)
		{
			this.Notification = notification;
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000039 RID: 57 RVA: 0x00002B30 File Offset: 0x00000D30
		// (set) Token: 0x0600003A RID: 58 RVA: 0x00002B47 File Offset: 0x00000D47
		public string Notification { get; private set; }
	}
}
