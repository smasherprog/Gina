using System;

namespace GalaSoft.MvvmLight.Messaging
{
	// Token: 0x0200000B RID: 11
	public class NotificationMessage : MessageBase
	{
		// Token: 0x0600003B RID: 59 RVA: 0x00002B50 File Offset: 0x00000D50
		public NotificationMessage(string notification)
		{
			this.Notification = notification;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002B63 File Offset: 0x00000D63
		public NotificationMessage(object sender, string notification)
			: base(sender)
		{
			this.Notification = notification;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002B77 File Offset: 0x00000D77
		public NotificationMessage(object sender, object target, string notification)
			: base(sender, target)
		{
			this.Notification = notification;
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600003E RID: 62 RVA: 0x00002B8C File Offset: 0x00000D8C
		// (set) Token: 0x0600003F RID: 63 RVA: 0x00002BA3 File Offset: 0x00000DA3
		public string Notification { get; private set; }
	}
}
