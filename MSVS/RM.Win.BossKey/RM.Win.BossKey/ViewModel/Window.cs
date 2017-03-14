using System.Windows.Media;

namespace RM.Win.BossKey.ViewModel
{
	public class Window
	{
		public Window(long handle, string title, string @class, string exeName, ImageSource icon, bool isVsiible)
		{
			Handle = handle;
			Title = title;
			Class = @class;
			ExeName = exeName;
			Icon = icon;
			IsVisible = isVsiible;
		}

		public long Handle { get; }

		public string Title { get; }

		public string Class { get; }

		public string ExeName { get; }

		public ImageSource Icon { get; }

		public bool IsVisible { get; }

		public override string ToString()
		{
			return $"[{ExeName}] '{Title}'";
		}

		public static Window Create(long handle, string title, string @class, string exeName, ImageSource icon, bool isVsiible)
		{
			return new Window(handle, title, @class, exeName, icon, isVsiible);
		}
	}
}
