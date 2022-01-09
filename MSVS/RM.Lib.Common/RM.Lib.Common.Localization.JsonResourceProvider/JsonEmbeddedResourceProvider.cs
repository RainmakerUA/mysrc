using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace RM.Lib.Common.Localization.JsonResourceProvider
{
	public class JsonEmbeddedResourceProvider : JsonBaseResourceProvider
	{
		private const string _resSeparator = ".";
		
		public JsonEmbeddedResourceProvider(Assembly? resourceAssembly = null, string? resourceEntryPrefix = null)
				: base(resourceAssembly, resourceEntryPrefix)
		{
		}

		protected override string ResourceSeparator => _resSeparator;

		protected override IDictionary<string, Stream> GetStreams(Assembly resAssembly)
		{
			return resAssembly.GetManifestResourceNames().Where(resAssembly.IsEmbeddedResource)
															.ToDictionary(name => name, name => resAssembly.GetManifestResourceStream(name)!);
		}
	}
}
