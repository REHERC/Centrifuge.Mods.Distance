using Eto.Forms;

namespace App.AdventureMaker.Core.Views.Pages
{
	public abstract class TableLayoutBase : TableLayout
	{
		protected T AddRow<T>(string label, T content) where T : Control
		{
			TableRow row = new TableRow()
			{
				ScaleHeight = false,
				Cells =
				{
					new TableCell(new Label()
					{
						Text = label,
						VerticalAlignment = VerticalAlignment.Center,
						TextAlignment = TextAlignment.Right
					}),
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
