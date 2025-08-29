using System;

namespace GalaSoft.MvvmLight.Messaging
{
	// Token: 0x02000011 RID: 17
	public class NotificationMessageAction : NotificationMessageWithCallback
	{
		// Token: 0x06000055 RID: 85 RVA: 0x00002E3D File Offset: 0x0000103D
		public NotificationMessageAction(string notification, Action callback)
			: base(notification, callback)
		{
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00002E4A File Offset: 0x0000104A
		public NotificationMessageAction(object sender, string notification, Action callback)
			: base(sender, notification, callback)
		{
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00002E58 File Offset: 0x00001058
		public NotificationMessageAction(object sender, object target, string notification, Action callback)
			: base(sender, target, notification, callback)
		{
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00002E68 File Offset: 0x00001068
		public void Execute()
		{
			base.Execute(new object[0]);
		}
	}
}
