using Eto.Forms;

namespace App.AdventureMaker.Core.Views.Pages
{
	public class LevelSetsPage : TableLayout
	{
		// TODO: Move level sets list from the left to a new class
		public LevelSetsPage()
		{
			// BackgroundColor = Colors.SlateGray;

			TableRow row = new TableRow()
			{
				Cells =
				{
					new TableCell()
					{
						Control = new LevelSetsView()
					},
					new TableCell(new Label() { Text = "Levels are displayed here."})
				}
			};

			Rows.Add(row);
		}
	}
}