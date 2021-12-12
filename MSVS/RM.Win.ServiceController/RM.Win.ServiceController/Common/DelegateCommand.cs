using System;
using System.Windows.Input;

namespace RM.Win.ServiceController.Common
{
	public sealed class DelegateCommand<T> : ICommand
	{
		private readonly Action<T?> _execute;

		private readonly Func<T?, bool>? _canExecute;

		public DelegateCommand(Action<T?> execute, Func<T?, bool>? canExecute = null)
		{
			_execute = execute;
			_canExecute = canExecute;
		}

		public event EventHandler? CanExecuteChanged;

		public bool CanExecute(object? parameter)
		{
			return _canExecute?.Invoke(parameter is T arg ? arg : default) ?? true;
		}

		public void Execute(object? parameter)
		{
			_execute(parameter is T arg ? arg : default);
		}

		public void RaiseCanExecuteChanged()
		{
			CanExecuteChanged?.Invoke(this, EventArgs.Empty);
		}
	}
}
