using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

namespace RM.Win.FlashNotifier.Utility
{
	internal static class FileStorage
	{
		private static readonly string _fileName;

		static FileStorage()
		{
			var asmFile = Assembly.GetExecutingAssembly().Location;
			_fileName = Path.Combine(Path.GetDirectoryName(asmFile) ?? ".\\", "storage.xml");
		}

		public static void Store<T>(T data)
		{
			var ser = new XmlSerializer(typeof(T));
			using var fs = new FileStream(_fileName, FileMode.Create, FileAccess.Write, FileShare.None);

			ser.Serialize(fs, data);
		}

		[return:MaybeNull]
		public static T Load<T>()
		{
			if (File.Exists(_fileName))
			{
				var ser = new XmlSerializer(typeof(T));
				using var fs = new FileStream(_fileName, FileMode.Open, FileAccess.Read, FileShare.Read);

				return (T) ser.Deserialize(fs);
			}

			return default;
		}
	}
}
