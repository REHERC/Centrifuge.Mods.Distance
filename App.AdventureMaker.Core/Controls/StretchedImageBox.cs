using Eto.Drawing;
using Eto.Forms;

namespace App.AdventureMaker.Core.Controls
{
	public class StretchedImageBox : Drawable
	{
		private Image image_;
		public Image Image
		{
			get => image_;
			set
			{
				image_ = value;
				Invalidate();
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			var g = e.Graphics;

			g.Clear(BackgroundColor);

			if (image_ != null)
			{
				g.DrawImage(image_, 0, 0, Width, Height);
			}
		}
	}
}
