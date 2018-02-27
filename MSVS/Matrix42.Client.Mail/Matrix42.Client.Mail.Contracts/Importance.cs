using System;

namespace Matrix42.Client.Mail.Contracts
{
	[Serializable]
	public enum Importance
	{
		None = 0,

		/// <summary>
		/// Low importance
		/// </summary>
		Low = 1,

		/// <summary>
		/// Normal importance
		/// </summary>
		Normal = 2,

		/// <summary>
		/// High importance.
		/// </summary>
		High = 3
	}
}
