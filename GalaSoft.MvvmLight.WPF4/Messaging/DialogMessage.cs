using System;
using System.Windows;

namespace GalaSoft.MvvmLight.Messaging
{
	// Token: 0x02000014 RID: 20
	public class DialogMessage : GenericMessage<string>
	{
		// Token: 0x06000067 RID: 103 RVA: 0x0000302F File Offset: 0x0000122F
		public DialogMessage(string content, Action<MessageBoxResult> callback)
			: base(content)
		{
			this.Callback = callback;
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00003043 File Offset: 0x00001243
		public DialogMessage(object sender, string content, Action<MessageBoxResult> callback)
			: base(sender, content)
		{
			this.Callback = callback;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00003058 File Offset: 0x00001258
		public DialogMessage(object sender, object target, string content, Action<MessageBoxResult> callback)
			: base(sender, target, content)
		{
			this.Callback = callback;
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600006A RID: 106 RVA: 0x00003070 File Offset: 0x00001270
		// (set) Token: 0x0600006B RID: 107 RVA: 0x00003087 File Offset: 0x00001287
		public MessageBoxButton Button { get; set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600006C RID: 108 RVA: 0x00003090 File Offset: 0x00001290
		// (set) Token: 0x0600006D RID: 109 RVA: 0x000030A7 File Offset: 0x000012A7
		public Action<MessageBoxResult> Callback { get; private set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600006E RID: 110 RVA: 0x000030B0 File Offset: 0x000012B0
		// (set) Token: 0x0600006F RID: 111 RVA: 0x000030C7 File Offset: 0x000012C7
		public string Caption { get; set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000070 RID: 112 RVA: 0x000030D0 File Offset: 0x000012D0
		// (set) Token: 0x06000071 RID: 113 RVA: 0x000030E7 File Offset: 0x000012E7
		public MessageBoxResult DefaultResult { get; set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000072 RID: 114 RVA: 0x000030F0 File Offset: 0x000012F0
		// (set) Token: 0x06000073 RID: 115 RVA: 0x00003107 File Offset: 0x00001307
		public MessageBoxImage Icon { get; set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000074 RID: 116 RVA: 0x00003110 File Offset: 0x00001310
		// (set) Token: 0x06000075 RID: 117 RVA: 0x00003127 File Offset: 0x00001327
		public MessageBoxOptions Options { get; set; }

		// Token: 0x06000076 RID: 118 RVA: 0x00003130 File Offset: 0x00001330
		public void ProcessCallback(MessageBoxResult result)
		{
			if (this.Callback != null)
			{
				this.Callback(result);
			}
		}
	}
}
