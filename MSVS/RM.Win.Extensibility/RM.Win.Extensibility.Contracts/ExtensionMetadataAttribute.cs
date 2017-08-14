using System;

namespace RM.Win.Extensibility.Contracts
{
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public class ExtensionMetadataAttribute: Attribute
	{
		public ExtensionMetadataAttribute(string name, ExtensionFlags flags)
		{
			Name = name;
			Flags = flags;
		}

		public string Name { get; }

		public ExtensionFlags Flags { get; }
	}
}
