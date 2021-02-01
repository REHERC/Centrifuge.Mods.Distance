using Eto.Drawing;
using Eto.Forms;
using System;

namespace App.AdventureMaker.Core.Controls.Cells
{
	public class ButtonCell : CustomCell
	{
		private readonly GridView parent;

		public ButtonCell(ref GridView parent)
		{
			this.parent = parent;
		}

		protected override void OnPaint(CellPaintEventArgs args)
		{
			base.OnPaint(args);

			var g = args.Graphics;
			g.ImageInterpolation = ImageInterpolation.High;
			const float margin = 1.5f;

			RectangleF rect = args.ClipRectangle;

			float sideLength = Math.Min(rect.Width, rect.Height) - margin;
			SizeF size = new SizeF(sideLength, sideLength);
			PointF position = new PointF(rect.X + (rect.Width - size.Width) / 2, rect.Y + (rect.Height - size.Height) / 2);

			RectangleF newRect = new RectangleF(position, size);
			//MessageBox.Show($"X:{rect.X}\nY:{rect.Y}\nW:{rect.Width}\nH:{rect.Height}");

			RectangleF borderRect = rect;
			borderRect.Offset(-1, -1);


			g.DrawRectangle(new Pen(Brushes.DarkGray), borderRect);
			g.DrawImage(Resources.GetIcon("Pencil.ico"), newRect);
		}

		protected override float OnGetPreferredWidth(CellEventArgs args)
		{
			return 32;
		}

		protected override void OnBeginEdit(CellEventArgs e)
		{
			base.OnBeginEdit(e);

			MessageBox.Show("BRUH");
			
			
			//OnCancelEdit();
		}
	}
}
