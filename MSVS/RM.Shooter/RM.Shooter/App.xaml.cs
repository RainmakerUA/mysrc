using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;

namespace RM.Shooter
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private static readonly string _name;

		static App()
		{
			var guidAttr = Assembly.GetExecutingAssembly().GetCustomAttribute<GuidAttribute>();

			if (guidAttr != null)
			{
				_name = guidAttr.Value;
			}
		}

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			if (!String.IsNullOrEmpty(_name))
			{
				EventWaitHandle eventHandle;
				if (EventWaitHandle.TryOpenExisting(_name, out eventHandle))
				{
					eventHandle.Set();
					Environment.Exit(0);
				}
				else
				{
					using (eventHandle = new EventWaitHandle(false, EventResetMode.AutoReset, _name))
					{
						ThreadPool.RegisterWaitForSingleObject(eventHandle, EventCallback, this, Timeout.Infinite, false);
					}
				}
			}
		}

		private void OnSecondInstanceStarted(EventArgs args)
		{
			var handler = SecondInstanceStarted;
			if (handler != null)
			{
				handler(this, args);
			}
		}

		private static void EventCallback(object state, bool isTimedOut)
		{
			var app = state as App;
			if (app != null)
			{
				app.OnSecondInstanceStarted(EventArgs.Empty);
			}
		}

		public event EventHandler SecondInstanceStarted;
	}
}
