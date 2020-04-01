using System;
using System.Xml.Serialization;

namespace RM.Win.FlashNotifier.Utility
{
	[Serializable]
	[XmlRoot(ElementName = "windata")]
	public class WindowData
	{
		[XmlArray(ElementName = "titles")]
		public string[] Titles { get; set; } = Array.Empty<string>();

		[XmlArray(ElementName = "classes")]
		public string[] Classes { get; set; } = Array.Empty<string>();
	}
}
