using System;

namespace GalaSoft.MvvmLight.Helpers
{
	// Token: 0x02000010 RID: 16
	public class WeakAction<T> : WeakAction, IExecuteWithObject
	{
		// Token: 0x06000050 RID: 80 RVA: 0x00002D7A File Offset: 0x00000F7A
		public WeakAction(object target, Action<T> action)
			: base(target, null)
		{
			this._action = action;
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000051 RID: 81 RVA: 0x00002D90 File Offset: 0x00000F90
		public new Action<T> Action
		{
			get
			{
				return this._action;
			}
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00002DA8 File Offset: 0x00000FA8
		public new void Execute()
		{
			if (this._action != null && base.IsAlive)
			{
				this._action(default(T));
			}
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00002DE8 File Offset: 0x00000FE8
		public void Execute(T parameter)
		{
			if (this._action != null && base.IsAlive)
			{
				this._action(parameter);
			}
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00002E20 File Offset: 0x00001020
		public void ExecuteWithObject(object parameter)
		{
			T t = (T)((object)parameter);
			this.Execute(t);
		}

		// Token: 0x04000011 RID: 17
		private readonly Action<T> _action;
	}
}
