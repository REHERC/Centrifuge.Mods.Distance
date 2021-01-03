using Eto.Forms;

namespace App.AdventureMaker.Core.Views
{
	public abstract class PropertiesListBase : TableLayout
	{
		protected T AddRow<T>(string title, T content) where T : Control
		{
			Label label = new Label()
			{
				Text = title,
				VerticalAlignment = VerticalAlignment.Center,
				TextAlignment = TextAlignment.Right
			};

			label.MouseDown += (sender, e) => content.Focus();

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

		protected void CompleteRows()
		{
			Rows.Add(new TableRow());
		}
	}
}
