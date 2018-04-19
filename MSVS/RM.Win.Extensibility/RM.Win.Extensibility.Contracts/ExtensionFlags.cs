using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.Win.Extensibility.Contracts
{
	[Flags]
	public enum ExtensionFlags
	{
		None = 0,
		CanExecute = 1,
	}
}
