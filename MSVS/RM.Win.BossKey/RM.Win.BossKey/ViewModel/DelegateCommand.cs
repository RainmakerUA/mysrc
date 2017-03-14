using System;
using System.Windows.Input;

namespace RM.Win.BossKey.ViewModel
{
	public class DelegateCommand : ICommand
	{
		private readonly Action<object> _execute;
		private readonly Func<object, bool> _canExecute;

		public DelegateCommand(Action<object> execute, Func<object, bool> canExecute)
		{
			_execute = execute;
			_canExecute = canExecute;
		}

		public DelegateCommand(Action<object> execute) : this(execute, null)
		{
		}

		public bool CanExecute(object parameter)
		{
			return (_canExecute?.Invoke(parameter)).GetValueOrDefault(true);
		}

		public void Execute(object parameter)
		{
			_execute(parameter);
		}

		public void OnCanExecuteChanged(object sender)
		{
			CanExecuteChanged?.Invoke(sender, EventArgs.Empty);
		}

		public event EventHandler CanExecuteChanged;
	}
}
