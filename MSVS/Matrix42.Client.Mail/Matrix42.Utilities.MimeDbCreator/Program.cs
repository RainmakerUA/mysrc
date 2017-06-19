using System;
using System.Collections.Generic;
using System.IO;
using System.Json;
using System.Linq;
using System.Net;
using Microsoft.Win32;

namespace Matrix42.Utilities.MimeDbCreator
{
	internal static class Program
	{
		private const string _mimeDbUrl = "https://cdn.rawgit.com/jshttp/mime-db/master/db.json";

		private static void Main(string[] args)
		{
			// TODO: Parse args
			var useMimeDb = true;
			var useRegistry = true;
			var outFileName = @"E:\_Temp\mimetype.dat";

			var mdb = useMimeDb ? GetMimeDb() : null;
			var result = useRegistry ? AddRegistryData(mdb) : mdb;
			WriteMimeData(outFileName, result);

			Console.WriteLine("Press any key to exit");
			Console.ReadKey(true);
		}

		private static IDictionary<string, MimeDbEntry> GetMimeDb()
		{
			var wc = new WebClient();
			var json = wc.DownloadString(_mimeDbUrl);
			var jValue = JsonValue.Parse(json);

			if (jValue is JsonObject jObj)
			{
				var result = jObj.ToDictionary(kv => kv.Key, kv =>
				{
					if (kv.Value is JsonObject obj && obj.TryGetValue("source", out var sourceJv))
					{
						var entry = new MimeDbEntry{source = sourceJv.ReadAs<string>()}	;

						if(obj.TryGetValue("compressible", out var comprJv))
						{
							entry.compressible = comprJv.ReadAs<bool>();
						}

						if (obj.TryGetValue("extensions", out var extJv))
						{
							entry.extensions = extJv is JsonArray jarr ? jarr.Cast<JsonValue>().Select(jv => jv.ReadAs<string>()).ToArray() : null;
						}
					
						return entry;
					}

					return null;
				});

				return result;
			}
			
			return null;
		}

		private static IDictionary<string, MimeDbEntry> AddRegistryData(IDictionary<string, MimeDbEntry> dict)
		{
			if (dict == null)
			{
				dict = new Dictionary<string, MimeDbEntry>();
			}

			using (var key = Registry.ClassesRoot.OpenSubKey(@"MIME\Database\Content Type\"))
			{
				if (key != null)
				{
					foreach (var mime in key.GetSubKeyNames())
					{
						if (mime.Contains("/"))
						{
							var mimeKey = key.OpenSubKey(mime);
							var ext = mimeKey?.GetValue("Extension", null) as string;

							if (!String.IsNullOrEmpty(ext))
							{
								if (ext.StartsWith("."))
								{
									ext = ext.Substring(1);
								}

								if (!String.IsNullOrEmpty(ext))
								{
									if (!dict.TryGetValue(mime, out var entry) || entry == null)
									{
										entry = new MimeDbEntry { source = "registry" };
										dict[mime] = entry;
									}

									if (entry.extensions == null || entry.extensions.Length == 0)
									{
										entry.extensions = new[] { ext };
									}
									else if (Array.IndexOf(entry.extensions, ext) == -1)
									{
										var list = new List<string>(entry.extensions) { ext };
										entry.extensions = list.ToArray();
									}
								}
							}
						}
					}
				}
			}

			return dict;
		}

		private static void WriteMimeData(string filename, IDictionary<string, MimeDbEntry> mimeData)
		{
			using (var fs = File.OpenWrite(filename))
			{
				using (var tw = new StreamWriter(fs))
				{
					var keys = mimeData.Keys.ToArray();
					Array.Sort(keys);

					foreach (var key in keys)
					{
						var extensions = mimeData[key]?.extensions;

						if (extensions != null && extensions.Length > 0)
						{
							tw.WriteLine("{0} {1}", key, String.Join("\u0020", extensions));
						}
					}
				}
			}
		}
	}
}
