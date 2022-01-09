using System;
using System.Windows.Input;

namespace RM.Lib.Wpf.Common.Commands
{
	public abstract class BaseDelegateCommand : ICommand
	{
		public event EventHandler? CanExecuteChanged;

		public abstract bool CanExecute(object? parameter);

		public abstract void Execute(object? parameter);

		public void RaiseCanExecuteChanged()
		{
			CanExecuteChanged?.Invoke(this, EventArgs.Empty);
		}
	}
}
