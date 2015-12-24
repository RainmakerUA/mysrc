using System;
using System.Windows.Forms;

namespace RM.Shooter
{
	internal class ProgramAppContext : ApplicationContext
	{
		private Form _mainForm;

		private ProgramAppContext(Form mainForm)
		{
			_mainForm = mainForm;
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			if (disposing && _mainForm != null)
			{
				if (!_mainForm.IsDisposed)
				{
					_mainForm.Dispose();
				}

				_mainForm = null;
			}
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new ProgramAppContext(new MainForm { Visible = false }));
		}
	}
}
