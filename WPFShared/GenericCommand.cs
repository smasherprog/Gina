using System;
using System.Windows.Input;

namespace WPFShared
{
	// Token: 0x02000046 RID: 70
	public class GenericCommand : ICommand
	{
		// Token: 0x060001FB RID: 507 RVA: 0x00009014 File Offset: 0x00007214
		public void FireCanExecuteChanged()
		{
			if (this._CanExecuteChanged != null)
			{
				this._CanExecuteChanged(this, new EventArgs());
			}
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0000902F File Offset: 0x0000722F
		public GenericCommand()
		{
		}

		// Token: 0x060001FD RID: 509 RVA: 0x00009037 File Offset: 0x00007237
		public GenericCommand(GenericCommand.ExecuteDelegate execute)
		{
			this.Execute = execute;
		}

		// Token: 0x060001FE RID: 510 RVA: 0x00009046 File Offset: 0x00007246
		bool ICommand.CanExecute(object parameter)
		{
			return this.CanExecute == null || this.CanExecute(parameter);
		}

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x060001FF RID: 511 RVA: 0x0000905E File Offset: 0x0000725E
		// (remove) Token: 0x06000200 RID: 512 RVA: 0x00009077 File Offset: 0x00007277
		event EventHandler ICommand.CanExecuteChanged
		{
			add
			{
				this._CanExecuteChanged = (EventHandler)Delegate.Combine(this._CanExecuteChanged, value);
			}
			remove
			{
				this._CanExecuteChanged = (EventHandler)Delegate.Remove(this._CanExecuteChanged, value);
			}
		}

		// Token: 0x06000201 RID: 513 RVA: 0x00009090 File Offset: 0x00007290
		void ICommand.Execute(object parameter)
		{
			if (this.Execute != null)
			{
				this.Execute(parameter);
			}
		}

		// Token: 0x040000A5 RID: 165
		public GenericCommand.CanExecuteDelegate CanExecute;

		// Token: 0x040000A6 RID: 166
		public GenericCommand.ExecuteDelegate Execute;

		// Token: 0x040000A7 RID: 167
		private EventHandler _CanExecuteChanged;

		// Token: 0x02000047 RID: 71
		// (Invoke) Token: 0x06000203 RID: 515
		public delegate bool CanExecuteDelegate(object parameter);

		// Token: 0x02000048 RID: 72
		// (Invoke) Token: 0x06000207 RID: 519
		public delegate void ExecuteDelegate(object parameter);
	}
}
