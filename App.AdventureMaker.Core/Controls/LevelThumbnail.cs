using Eto.Drawing;
using Eto.Forms;

namespace App.AdventureMaker.Core.Controls
{
	public class LevelThumbnail : Drawable
	{
		private Image image_ = null;
		public Image Image
		{
			get => image_;
			set
			{
				image_ = value;
				Invalidate();
			}
		}

		private float gradientDelta_ = 0.75f;
		public float GradientDelta
		{
			get => gradientDelta_;
			set
			{
				gradientDelta_ = value;
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

			g.FillRectangle(new LinearGradientBrush(
				Color.FromArgb(BackgroundColor.Rb, BackgroundColor.Gb, BackgroundColor.Bb, 96),
				BackgroundColor, 
				new Point((int)(Width * gradientDelta_), 0),
				new Point(Width, 0)
			), Bounds);
		}
	}
}
