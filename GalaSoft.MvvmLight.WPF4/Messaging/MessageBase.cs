using System;

namespace GalaSoft.MvvmLight.Messaging
{
	// Token: 0x02000003 RID: 3
	public class MessageBase
	{
		// Token: 0x06000002 RID: 2 RVA: 0x000020D0 File Offset: 0x000002D0
		public MessageBase()
		{
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000020DB File Offset: 0x000002DB
		public MessageBase(object sender)
		{
			this.Sender = sender;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020EE File Offset: 0x000002EE
		public MessageBase(object sender, object target)
			: this(sender)
		{
			this.Target = target;
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000005 RID: 5 RVA: 0x00002104 File Offset: 0x00000304
		// (set) Token: 0x06000006 RID: 6 RVA: 0x0000211B File Offset: 0x0000031B
		public object Sender { get; protected set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000007 RID: 7 RVA: 0x00002124 File Offset: 0x00000324
		// (set) Token: 0x06000008 RID: 8 RVA: 0x0000213B File Offset: 0x0000033B
		public object Target { get; protected set; }
	}
}
