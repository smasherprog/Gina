using System;

namespace GalaSoft.MvvmLight.Messaging
{
	// Token: 0x0200000C RID: 12
	public class NotificationMessageWithCallback : NotificationMessage
	{
		// Token: 0x06000040 RID: 64 RVA: 0x00002BAC File Offset: 0x00000DAC
		public NotificationMessageWithCallback(string notification, Delegate callback)
			: base(notification)
		{
			NotificationMessageWithCallback.CheckCallback(callback);
			this._callback = callback;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002BC6 File Offset: 0x00000DC6
		public NotificationMessageWithCallback(object sender, string notification, Delegate callback)
			: base(sender, notification)
		{
			NotificationMessageWithCallback.CheckCallback(callback);
			this._callback = callback;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002BE1 File Offset: 0x00000DE1
		public NotificationMessageWithCallback(object sender, object target, string notification, Delegate callback)
			: base(sender, target, notification)
		{
			NotificationMessageWithCallback.CheckCallback(callback);
			this._callback = callback;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002C00 File Offset: 0x00000E00
		public virtual object Execute(params object[] arguments)
		{
			return this._callback.DynamicInvoke(arguments);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002C20 File Offset: 0x00000E20
		private static void CheckCallback(Delegate callback)
		{
			if (callback == null)
			{
				throw new ArgumentNullException("callback", "Callback may not be null");
			}
		}

		// Token: 0x0400000E RID: 14
		private readonly Delegate _callback;
	}
}
