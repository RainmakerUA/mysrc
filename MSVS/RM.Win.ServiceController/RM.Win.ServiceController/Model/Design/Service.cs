using System.Windows.Input;
using RM.Lib.Wpf.Common.Commands;

namespace RM.Win.ServiceController.Model.Design
{
	public sealed class Service
	{
		public Service(string name, bool isValid)
		{
			StartCommand = new EmptyCommand(isValid);
			StopCommand = new EmptyCommand(isValid);
			RestartCommand = new EmptyCommand(isValid);

			IsValid = isValid;

			DisplayName = $"Design Service: {name}";
			StatusText = isValid ? "Desinging" : "Failing";
		}

		public ICommand StartCommand { get; }

		public ICommand StopCommand { get; }

		public ICommand RestartCommand { get; }

		public bool IsValid { get; }

		public bool IsEnabled
		{
			get => IsValid;
			set {}
		}

		public string DisplayName { get; }

		public string StatusText { get; }

		public static Service DesignModel { get; } = new("Design One X", true);
	}
}
