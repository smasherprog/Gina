using System;
using System.Windows.Input;

namespace GalaSoft.MvvmLight.Command
{
	// Token: 0x02000012 RID: 18
	public class RelayCommand<T> : ICommand
	{
		// Token: 0x06000059 RID: 89 RVA: 0x00002E78 File Offset: 0x00001078
		public RelayCommand(Action<T> execute)
			: this(execute, null)
		{
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00002E88 File Offset: 0x00001088
		public RelayCommand(Action<T> execute, Predicate<T> canExecute)
		{
			if (execute == null)
			{
				throw new ArgumentNullException("execute");
			}
			this._execute = execute;
			this._canExecute = canExecute;
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600005B RID: 91 RVA: 0x00002EC4 File Offset: 0x000010C4
		// (remove) Token: 0x0600005C RID: 92 RVA: 0x00002EE8 File Offset: 0x000010E8
		public event EventHandler CanExecuteChanged
		{
			add
			{
				if (this._canExecute != null)
				{
					CommandManager.RequerySuggested += value;
				}
			}
			remove
			{
				if (this._canExecute != null)
				{
					CommandManager.RequerySuggested -= value;
				}
			}
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00002F0C File Offset: 0x0000110C
		public void RaiseCanExecuteChanged()
		{
			CommandManager.InvalidateRequerySuggested();
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00002F18 File Offset: 0x00001118
		public bool CanExecute(object parameter)
		{
			return this._canExecute == null || this._canExecute((T)((object)parameter));
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00002F46 File Offset: 0x00001146
		public void Execute(object parameter)
		{
			this._execute((T)((object)parameter));
		}

		// Token: 0x04000012 RID: 18
		private readonly Action<T> _execute;

		// Token: 0x04000013 RID: 19
		private readonly Predicate<T> _canExecute;
	}
}
