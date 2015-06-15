using System.Collections.Generic;

namespace RM.Shooter.Configuration
{
	public interface IKeyedStorage<TK, TV>
	{
		IEnumerable<TK> Keys { get; }

		TV this[TK key] { get; set; }
	}
}
