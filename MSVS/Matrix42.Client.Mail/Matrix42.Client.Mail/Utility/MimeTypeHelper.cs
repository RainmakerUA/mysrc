using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Matrix42.Client.Mail.Utility
{
	internal static class MimeTypeHelper
	{
		private const char _space = '\u0020';
		private const string _dot = ".";
		private const string _resourceDir = "Resources";
		private const string _mimeResource = "mimetype.dat";

		private static readonly Type _thisType = typeof(MimeTypeHelper);

		private static string[] _resourceLines;
		private static Dictionary<string, string[]> _mimeTypeToExtensions;
		private static Dictionary<string, string[]> _extensionToMimeTypes;

		public static IReadOnlyList<string> GetExtensions(string mimeType)
		{
			if (_mimeTypeToExtensions == null)
			{
				_mimeTypeToExtensions = GetMimeTypeToExtensionsMapping();
			}

			return _mimeTypeToExtensions.TryGetValue(mimeType, out var result) ? result : new string[0];
		}

		public static IReadOnlyList<string> GetMimeTypes(string extension)
		{
			if (_extensionToMimeTypes == null)
			{
				_extensionToMimeTypes = GetExtensionToMimeTypesMapping();
			}

			if (extension.StartsWith(_dot))
			{
				extension = extension.Substring(1);
			}

			return _extensionToMimeTypes.TryGetValue(extension, out var mime) ? mime : null;
		}

		private static Dictionary<string, string[]> GetMimeTypeToExtensionsMapping()
		{
			if (_resourceLines == null)
			{
				_resourceLines = ReadMimeResource();
			}

			var result = new Dictionary<string, string[]>();

			foreach (var line in _resourceLines)
			{
				var parts = line.Split(new[] { _space }, StringSplitOptions.RemoveEmptyEntries);

				if (parts.Length > 1)
				{
					var key = parts[0].Trim();
					var values = parts.Skip(1).Select(s => s.Trim()).ToArray();

					if (!result.TryGetValue(key, out var vals) || vals == null || vals.Length == 0)
					{
						result[key] = values;
					}
				}
			}

			return result;
		}

		private static Dictionary<string, string[]> GetExtensionToMimeTypesMapping()
		{
			if (_resourceLines == null)
			{
				_resourceLines = ReadMimeResource();
			}

			var result = new Dictionary<string, string[]>();

			foreach (var line in _resourceLines)
			{
				var parts = line.Split(new[] { _space }, StringSplitOptions.RemoveEmptyEntries);

				if (parts.Length > 1)
				{
					var value = parts[0].Trim();
					var keys = parts.Skip(1).Select(s => s.Trim()).ToArray();

					foreach (var key in keys)
					{
						var newVal = result.TryGetValue(key, out var val)
										? new List<string>(val){ value }.ToArray()
										: new []{ value };

						result[key] = newVal;
					}
				}
			}

			return result;
		}

		private static string[] ReadMimeResource()
		{
			var ns = _thisType.Namespace ?? String.Empty;
			ns = ns.Substring(0, ns.LastIndexOf(_dot, StringComparison.Ordinal));

			var resName = String.Join(_dot, ns, _resourceDir, _mimeResource);
			var lines = new List<string>();

			using (var resStream = _thisType.Assembly.GetManifestResourceStream(resName))
			{
				using (var reader = new StreamReader(resStream, Encoding.ASCII))
				{
					while (!reader.EndOfStream)
					{
						lines.Add(reader.ReadLine());
					}
				}
			}

			return lines.ToArray();
		}
	}
}
