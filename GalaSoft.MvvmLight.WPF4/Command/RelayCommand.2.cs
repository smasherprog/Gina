using System;
using System.Diagnostics;
using System.Windows.Input;

namespace GalaSoft.MvvmLight.Command
{
	// Token: 0x02000013 RID: 19
	public class RelayCommand : ICommand
	{
		// Token: 0x06000060 RID: 96 RVA: 0x00002F5B File Offset: 0x0000115B
		public RelayCommand(Action execute)
			: this(execute, null)
		{
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00002F68 File Offset: 0x00001168
		public RelayCommand(Action execute, Func<bool> canExecute)
		{
			if (execute == null)
			{
				throw new ArgumentNullException("execute");
			}
			this._execute = execute;
			this._canExecute = canExecute;
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000062 RID: 98 RVA: 0x00002FA4 File Offset: 0x000011A4
		// (remove) Token: 0x06000063 RID: 99 RVA: 0x00002FC8 File Offset: 0x000011C8
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

		// Token: 0x06000064 RID: 100 RVA: 0x00002FEC File Offset: 0x000011EC
		public void RaiseCanExecuteChanged()
		{
			CommandManager.InvalidateRequerySuggested();
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00002FF8 File Offset: 0x000011F8
		[DebuggerStepThrough]
		public bool CanExecute(object parameter)
		{
			return this._canExecute == null || this._canExecute();
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00003020 File Offset: 0x00001220
		public void Execute(object parameter)
		{
			this._execute();
		}

		// Token: 0x04000014 RID: 20
		private readonly Action _execute;

		// Token: 0x04000015 RID: 21
		private readonly Func<bool> _canExecute;
	}
}
