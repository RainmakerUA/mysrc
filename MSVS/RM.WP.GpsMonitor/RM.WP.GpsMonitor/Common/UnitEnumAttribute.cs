using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.WP.GpsMonitor.Common
{
	[AttributeUsage(AttributeTargets.Enum)]
	public class UnitEnumAttribute : Attribute
	{
		public UnitEnumAttribute(string resourcePrefix = null)
		{
			ResourcePrefix = resourcePrefix;
		}

		public string ResourcePrefix { get; }
	}

	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class UnitEnumValueAttribute : Attribute
	{
		public UnitEnumValueAttribute(string nameKey = null)
		{
			NameKey = nameKey;
			Coefficient = 1.0;
		}

		public string NameKey { get; set; }

		public string DescriptionKey { get; set; }

		public double Coefficient { get; set; }
	}
}
