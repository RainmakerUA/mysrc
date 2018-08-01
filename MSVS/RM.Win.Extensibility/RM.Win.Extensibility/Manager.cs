using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.Win.Extensibility
{
    public sealed class Manager
    {
		private static class SingletonProvider
		{
			public static readonly Manager ManagerInstance = new Manager();

			static SingletonProvider()
			{
			}
		}

	    private Manager()
	    {
		    
	    }

	    public static Manager Instance => SingletonProvider.ManagerInstance;

	    public void Initialize<TApp>(TApp app)
	    {
		    var container = ExtensionContainer.LoadExtensionAssembly("Plugins\\RM.Ext.PluginTest.dll", app);
	    }
    }
}
