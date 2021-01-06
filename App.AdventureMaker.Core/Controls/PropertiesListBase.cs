using Eto.Drawing;
using Eto.Forms;

namespace App.AdventureMaker.Core.Controls
{
	public abstract class PropertiesListBase : TableLayout
	{
		protected static readonly Size DefaultSpacing = new Size(8, 8);

		protected T AddRow<T>(string title, T content) where T : Control
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

		protected void CompleteRows() => CompleteRows(Size.Empty);

		protected void CompleteRows(Size spacing)
		{
			Spacing = spacing;
			Rows.Add(new TableRow());
		}
	}
}
