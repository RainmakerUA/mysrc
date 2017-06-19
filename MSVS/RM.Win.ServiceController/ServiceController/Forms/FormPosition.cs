using System;
using System.Windows.Forms;

namespace ServiceController.Forms
{
	public partial class FormPosition : Form
	{
		public FormPosition()
		{
			InitializeComponent();
		}

		private void UpdateGeometry()
		{
			int x = Location.X;
			int y = Location.Y;
			int w = Size.Width;
			int h = Size.Height;

			Text = String.Format("Window geometry: {0} ; {1} | {2} x {3}", x, y, w, h);
			labelGeometry.Text = String.Format(
					"{0:0000} ; {1:0000}\n{2:0000} x {3:0000}",
					x, y, w, h
				);
		}

		private void OnLocationChanged(object sender, EventArgs e)
		{
			UpdateGeometry();
		}

		private void OnResize(object sender, EventArgs e)
		{
			UpdateGeometry();
		}
	}
}