using System;
using System.Windows.Forms;

namespace RM.BossKey.Components
{
	internal sealed class ShowHideNativeWindow : NativeWindow
	{
		private bool _hidden;

		private ShowHideNativeWindow()
		{
		}



		public static ShowHideNativeWindow Find(string className, string title)
		{
			// TODO:
			//  • FindWindow
			//  • EnumWindows
			throw new NotImplementedException();
		}
	}
}
