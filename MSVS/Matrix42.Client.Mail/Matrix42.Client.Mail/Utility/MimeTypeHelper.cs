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
		private static Dictionary<string, string[]> _mimeTypeToExtension;
		private static Dictionary<string, string> _extensionToMimeType;

		public static IReadOnlyList<string> GetExtensions(string mimeType)
		{
			if (_mimeTypeToExtension == null)
			{
				_mimeTypeToExtension = GetMimeTypeToExtensionMapping();
			}

			return _mimeTypeToExtension.TryGetValue(mimeType, out var result) ? result : new string[0];
		}

		public static string GetMimeType(string extension)
		{
			if (_extensionToMimeType == null)
			{
				_extensionToMimeType = GetExtensionToMimeTypeMapping();
			}

			if (extension.StartsWith(_dot))
			{
				extension = extension.Substring(1);
			}

			return _extensionToMimeType.TryGetValue(extension, out var mime) ? mime : null;
		}

		private static Dictionary<string, string[]> GetMimeTypeToExtensionMapping()
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

		private static Dictionary<string, string> GetExtensionToMimeTypeMapping()
		{
			if (_resourceLines == null)
			{
				_resourceLines = ReadMimeResource();
			}

			var result = new Dictionary<string, string>();

			foreach (var line in _resourceLines)
			{
				var parts = line.Split(new[] { _space }, StringSplitOptions.RemoveEmptyEntries);

				if (parts.Length > 1)
				{
					var value = parts[0].Trim();
					var keys = parts.Skip(1).Select(s => s.Trim()).ToArray();

					foreach (var key in keys)
					{
						if (!result.TryGetValue(key, out var val) || String.IsNullOrEmpty(val))
						{
							result[key] = value;
						} 
					}
				}
			}

			return result;
		}

		private static string[] ReadMimeResource()
		{
			var @namespace = _thisType.Namespace ?? String.Empty;
			@namespace = @namespace.Substring(0, @namespace.LastIndexOf(_dot, StringComparison.Ordinal));

			var resName = String.Join(_dot, @namespace, _resourceDir, _mimeResource);
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
