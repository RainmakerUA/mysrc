using System;
using System.Windows.Input;

namespace RM.Lib.Wpf.Common.Commands
{
	public sealed class DelegateCommand : BaseDelegateCommand
	{
		private readonly Action<object?> _execute;
		private readonly Func<object?, bool>? _canExecute;

		public DelegateCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
		{
			_execute = execute;
			_canExecute = canExecute;
		}

		public override bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter) ?? true;

		public override void Execute(object? parameter)
		{
			_execute(parameter);
		}
	}

	public sealed class DelegateCommand<T> : BaseDelegateCommand
	{
		private readonly Action<T?> _execute;

		private readonly Func<T?, bool>? _canExecute;

		public DelegateCommand(Action<T?> execute, Func<T?, bool>? canExecute = null)
		{
			_execute = execute;
			_canExecute = canExecute;
		}

		public override bool CanExecute(object? parameter)
		{
			return _canExecute?.Invoke(parameter is T arg ? arg : default) ?? true;
		}

		public override void Execute(object? parameter)
		{
			_execute(parameter is T arg ? arg : default);
		}
	}
}
