using System;
using System.Windows.Automation;
using RM.Shooter.Native;

namespace RM.Shooter.Modules
{
	internal sealed class VtAntiAfk
	{
		private readonly IntPtr _vtWnd;
		private int _lastError;

		public VtAntiAfk()
		{
			_vtWnd = Win32.GetWindow(null, "Ventrilo", out _lastError);
		}

		public void PerformActivity()
		{
			var desktop = AutomationElement.RootElement;
			var vt = desktop.FindFirst(
									TreeScope.Children,
									new PropertyCondition(AutomationElement.NameProperty, "Ventrilo")
								);
			if (vt != null)
			{
				var chatButton = vt.FindFirst(
											TreeScope.Descendants,
											new AndCondition(
														new PropertyCondition(AutomationElement.ClassNameProperty, "Button"),
														new PropertyCondition(AutomationElement.NameProperty, "Chat")
													)
										);
				if (chatButton != null && chatButton.Current.IsEnabled)
				{
					var pattern = chatButton.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
					if (pattern != null)
					{
						pattern.Invoke();
					}
				}
			}
		}
	}
}
