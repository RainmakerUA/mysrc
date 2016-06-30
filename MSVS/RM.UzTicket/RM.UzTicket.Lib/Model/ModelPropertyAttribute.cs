using System;

namespace RM.UzTicket.Lib.Model
{
	[AttributeUsage(AttributeTargets.Property)]
	public class ModelPropertyAttribute : Attribute
	{
		public ModelPropertyAttribute()
		{
		}

		public ModelPropertyAttribute(string name)
		{
			Name = name;
		}

		public string Name { get; }
	}
}
