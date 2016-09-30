using System.Collections.Generic;

namespace RM.BossKey.Ini
{
	public interface IKeyedStorage<TK, TV>
	{
		IEnumerable<TK> Keys { get; }

		TV this[TK key] { get; set; }
	}
}
