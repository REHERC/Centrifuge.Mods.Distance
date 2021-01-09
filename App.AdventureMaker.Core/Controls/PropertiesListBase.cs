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
				/*switch (content)
				{
					case CheckBox checkbox:
						checkbox.Checked = !checkbox.Checked;
						break;
				}*/
			};
			

			TableRow row = new TableRow()
			{
				ScaleHeight = false,
				Cells =
				{
					new TableCell(label),
					new TableCell(content)
				}
			};

			Rows.Add(row);

			return content;
		}

		public void CompleteRows() => CompleteRows(Size.Empty);

		public void CompleteRows(Size spacing)
		{
			Spacing = spacing;
			Rows.Add(new TableRow());
		}
	}
}
