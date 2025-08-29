using System;

namespace GalaSoft.MvvmLight.Messaging
{
	// Token: 0x02000005 RID: 5
	public interface IMessenger
	{
		// Token: 0x0600000E RID: 14
		void Register<TMessage>(object recipient, Action<TMessage> action);

		// Token: 0x0600000F RID: 15
		void Register<TMessage>(object recipient, bool receiveDerivedMessagesToo, Action<TMessage> action);

		// Token: 0x06000010 RID: 16
		void Send<TMessage>(TMessage message);

		// Token: 0x06000011 RID: 17
		void Send<TMessage, TTarget>(TMessage message);

		// Token: 0x06000012 RID: 18
		void Unregister(object recipient);

		// Token: 0x06000013 RID: 19
		void Unregister<TMessage>(object recipient);

		// Token: 0x06000014 RID: 20
		void Unregister<TMessage>(object recipient, Action<TMessage> action);
	}
}
