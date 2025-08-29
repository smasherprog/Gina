using System;

namespace GalaSoft.MvvmLight.Messaging
{
	// Token: 0x0200000D RID: 13
	public class NotificationMessageAction<TCallbackParameter> : NotificationMessageWithCallback
	{
		// Token: 0x06000045 RID: 69 RVA: 0x00002C4A File Offset: 0x00000E4A
		public NotificationMessageAction(string notification, Action<TCallbackParameter> callback)
			: base(notification, callback)
		{
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002C57 File Offset: 0x00000E57
		public NotificationMessageAction(object sender, string notification, Action<TCallbackParameter> callback)
			: base(sender, notification, callback)
		{
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002C65 File Offset: 0x00000E65
		public NotificationMessageAction(object sender, object target, string notification, Action<TCallbackParameter> callback)
			: base(sender, target, notification, callback)
		{
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002C78 File Offset: 0x00000E78
		public void Execute(TCallbackParameter parameter)
		{
			base.Execute(new object[] { parameter });
		}
	}
}
