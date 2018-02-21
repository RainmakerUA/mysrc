using System.Collections.Generic;

namespace RM.UzTicket.Lib.Model
{
	public abstract class ModelPersistable : ModelBase
	{
		public abstract IDictionary<string, string> ToDictionary();
	}
}
