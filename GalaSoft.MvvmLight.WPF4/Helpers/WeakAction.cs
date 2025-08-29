using System;

namespace GalaSoft.MvvmLight.Helpers
{
	// Token: 0x0200000F RID: 15
	public class WeakAction
	{
		// Token: 0x0600004A RID: 74 RVA: 0x00002C9E File Offset: 0x00000E9E
		public WeakAction(object target, Action action)
		{
			this._reference = new WeakReference(target);
			this._action = action;
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600004B RID: 75 RVA: 0x00002CBC File Offset: 0x00000EBC
		public Action Action
		{
			get
			{
				return this._action;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600004C RID: 76 RVA: 0x00002CD4 File Offset: 0x00000ED4
		public bool IsAlive
		{
			get
			{
				return this._reference != null && this._reference.IsAlive;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600004D RID: 77 RVA: 0x00002D08 File Offset: 0x00000F08
		public object Target
		{
			get
			{
				object obj;
				if (this._reference == null)
				{
					obj = null;
				}
				else
				{
					obj = this._reference.Target;
				}
				return obj;
			}
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002D3C File Offset: 0x00000F3C
		public void Execute()
		{
			if (this._action != null && this.IsAlive)
			{
				this._action();
			}
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002D70 File Offset: 0x00000F70
		public void MarkForDeletion()
		{
			this._reference = null;
		}

		// Token: 0x0400000F RID: 15
		private readonly Action _action;

		// Token: 0x04000010 RID: 16
		private WeakReference _reference;
	}
}
