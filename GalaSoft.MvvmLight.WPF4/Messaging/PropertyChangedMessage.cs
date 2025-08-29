using System;

namespace GalaSoft.MvvmLight.Messaging
{
	// Token: 0x02000008 RID: 8
	public class PropertyChangedMessage<T> : PropertyChangedMessageBase
	{
		// Token: 0x0600002A RID: 42 RVA: 0x000029F8 File Offset: 0x00000BF8
		public PropertyChangedMessage(object sender, T oldValue, T newValue, string propertyName)
			: base(sender, propertyName)
		{
			this.OldValue = oldValue;
			this.NewValue = newValue;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002A16 File Offset: 0x00000C16
		public PropertyChangedMessage(T oldValue, T newValue, string propertyName)
			: base(propertyName)
		{
			this.OldValue = oldValue;
			this.NewValue = newValue;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002A32 File Offset: 0x00000C32
		public PropertyChangedMessage(object sender, object target, T oldValue, T newValue, string propertyName)
			: base(sender, target, propertyName)
		{
			this.OldValue = oldValue;
			this.NewValue = newValue;
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600002D RID: 45 RVA: 0x00002A54 File Offset: 0x00000C54
		// (set) Token: 0x0600002E RID: 46 RVA: 0x00002A6B File Offset: 0x00000C6B
		public T NewValue { get; private set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600002F RID: 47 RVA: 0x00002A74 File Offset: 0x00000C74
		// (set) Token: 0x06000030 RID: 48 RVA: 0x00002A8B File Offset: 0x00000C8B
		public T OldValue { get; private set; }
	}
}
