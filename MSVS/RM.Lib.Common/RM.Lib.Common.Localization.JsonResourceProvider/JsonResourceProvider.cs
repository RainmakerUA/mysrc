using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace RM.Lib.Common.Localization.JsonResourceProvider
{
	public class JsonResourceProvider : JsonBaseResourceProvider
	{
		private const string _resSeparator = "/";

		private readonly string? _resourceName;
		
		public JsonResourceProvider(Assembly? resourceAssembly = null, string? resourceName = null, string? resourceEntryPrefix = null)
				: base(resourceAssembly, resourceEntryPrefix)
		{
			_resourceName = resourceName;
		}

		protected override string ResourceSeparator => _resSeparator;

		protected override IDictionary<string, Stream> GetStreams(Assembly resAssembly)
		{
			Stream? baseResStream;

			if (!String.IsNullOrEmpty(_resourceName))
			{
				baseResStream = resAssembly.GetManifestResourceStream(_resourceName);
			}
			else
			{
				// TODO: Find correct resource to load
				var firstLinkedResName = resAssembly.GetManifestResourceNames()
													.FirstOrDefault(name => IsSuitableResource(resAssembly, name));
				baseResStream = firstLinkedResName is null
									? null
									: resAssembly.GetManifestResourceStream(firstLinkedResName);
			}

			if (baseResStream is null)
			{
				throw new MissingManifestResourceException("No suitable manifest resource is found");
			}

			return new ResourceReader(baseResStream).Cast<DictionaryEntry>()
													.ToDictionary(de => (de.Key as string)!, de => (de.Value as Stream)!);

			static bool IsSuitableResource(Assembly asm, string name)
			{
				return asm.IsEmbeddedResource(name) && asm.GetResourceCount(name) > 1;
			}
		}
	}
}
