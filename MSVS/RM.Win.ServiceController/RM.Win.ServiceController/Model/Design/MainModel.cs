using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using RM.Lib.Wpf.Common.Commands;
using RM.Win.ServiceController.Settings;

namespace RM.Win.ServiceController.Model.Design
{
	public sealed class MainModel
	{
		private static readonly Service[] _services =
													{
														new("Service I", true),
														new("Service II", true),
														new("Test NotValid", false)
													};

		public IEnumerable<Service> Services => _services;

		public Geometry Geometry { get; } = new Geometry
												{
													Left = Double.NaN,
													Top = Double.NaN,
													Width = 595,
													Height = 340,
													State = WindowState.Normal,
												};

		public ICommand StartCommand { get; } = new EmptyCommand();

		public ICommand StopCommand { get; } = new EmptyCommand();

		public ICommand RestartCommand { get; } = new EmptyCommand();

		public ICommand ShowSettingsCommand { get; } = new EmptyCommand();
	}
}
