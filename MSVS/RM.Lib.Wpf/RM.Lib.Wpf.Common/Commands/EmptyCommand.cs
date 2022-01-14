
namespace RM.Lib.Wpf.Common.Commands
{
	public sealed class EmptyCommand : BaseDelegateCommand
	{
		private readonly bool _canExecute;

		public EmptyCommand(bool canExecute = true)
		{
			_canExecute = canExecute;
		}

		public override bool CanExecute(object? parameter) => _canExecute;

		public override void Execute(object? parameter)
		{
		}
	}
}
