using System.Windows;

namespace RM.Win.ServiceController.Settings
{
	public sealed class Geometry
	{
		public double Left { get; set; }

		public double Top { get; set; }

		public double Width { get; set; }

		public double Height { get; set; }

		public WindowState State { get; set; }

		public Geometry Clone() => (MemberwiseClone() as Geometry)!;
	}
}
