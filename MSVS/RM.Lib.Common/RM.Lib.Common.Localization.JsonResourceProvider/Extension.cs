using System.Reflection;
using System.Resources;

namespace RM.Lib.Common.Localization.JsonResourceProvider
{
	internal static class Extension
	{
		public static bool IsEmbeddedResource(this Assembly assembly, string resourceName)
		{
			return assembly.GetManifestResourceInfo(resourceName) is { } info
					&& (info.ResourceLocation & ResourceLocation.Embedded) == ResourceLocation.Embedded;
		}

		public static int GetResourceCount(this Assembly assembly, string resourceName)
		{
			var resStream = assembly.GetManifestResourceStream(resourceName);
			
			if (resStream is not null)
			{
				try
				{
					using var reader = new ResourceReader(resStream);
					var value = reader.GetType()
										.GetField("_numResources", BindingFlags.NonPublic | BindingFlags.Instance)?
										.GetValue(reader);
					return value is null ? 0 : (int) value;
				}
				finally
				{
					resStream.Dispose();
				}
			}

			return 0;
		}
	}
}
