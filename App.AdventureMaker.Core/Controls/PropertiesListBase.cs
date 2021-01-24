using Eto.Drawing;
using Eto.Forms;

namespace App.AdventureMaker.Core.Controls
{
	public class PropertiesListBase : TableLayout
	{
		protected static readonly Size DefaultSpacing = new Size(8, 8);

		public T AddRow<T>(string title, T content) where T : Control
		{
			Label label = new Label()
			{
				Text = title,
				VerticalAlignment = VerticalAlignment.Center,
				TextAlignment = TextAlignment.Right
			};

			label.MouseDown += (sender, e) =>
			{
				content.Focus();
			};

			TableRow row = new TableRow()
			{
				ScaleHeight = false,
				Cells =
				{
					new TableCell(label),
					new TableCell(content)
				},
			};

			Rows.Add(row);

			return content;
		}

		public void Separator() => AddRow(string.Empty, Line());

		private Panel Line()
		{
			return new Panel()
			{
				BackgroundColor = Colors.Transparent,
				Padding = new Padding(0, 4),
				Content = new Panel()
				{
					BackgroundColor = Colors.DarkGray,
					Height = 1
				}
			};
		}

		public void CompleteRows() => CompleteRows(DefaultSpacing);

		public void CompleteRows(Size spacing)
		{
			Spacing = spacing;
			Rows.Add(new TableRow());
		}
	}
}
