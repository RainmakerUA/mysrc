using System;

namespace RM.Lib.Utility
{
	public interface ILogFactory
	{
		ILog GetLog();

		ILog GetLog(Type type);
		
		ILog GetLog(string name);
	}
}
