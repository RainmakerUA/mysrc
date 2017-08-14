using System;
using RM.Win.Extensibility.Contracts;
using RM.Win.Extensibility.TestContracts;

namespace RM.Ext.PluginTest
{
	[ExtensionMetadata("Test Plugin", ExtensionFlags.CanExecute)]
    public class TestPlugin: MarshalByRefObject, IExtension<IWinTest>
    {
	    private IWinTest _app;

		public void OnInitializing(IWinTest app)
		{
			_app = app;
		}

		public void OnInitialized()
		{
			//throw new NotImplementedException();
		}

		public void OnExecute()
		{
			_app.ShowMessage("TestPlugin is executed!", "Extensibility Success");
		}

		public void OnUnloading()
		{
			//throw new NotImplementedException();
		}
	}
}
