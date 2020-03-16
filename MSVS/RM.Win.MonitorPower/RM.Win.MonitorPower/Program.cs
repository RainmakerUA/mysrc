using System;
using System.Threading;
using System.Windows.Forms;

namespace RM.Win.MonitorPower
{
	internal static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main()
		{
			AppDomain.CurrentDomain.UnhandledException += OnCurrentDomainUnhandledException;
			Application.ThreadException += OnApplicationOnThreadException;

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new FormMain());
		}

		private static void OnCurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			MessageBox.Show($"{e.ExceptionObject}\r\n\r\n{(e.IsTerminating ? "Terminating" : "Going on")}", "Unhandled exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		private static void OnApplicationOnThreadException(object sender, ThreadExceptionEventArgs e)
		{
			MessageBox.Show(e.Exception.ToString(), "Unhandled exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
	}
}
