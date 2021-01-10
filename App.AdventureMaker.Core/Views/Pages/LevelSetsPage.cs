using App.AdventureMaker.Core.Controls;
using Eto.Forms;

namespace App.AdventureMaker.Core.Views.Pages
{
	public class LevelSetsPage : TableLayout
	{
		public readonly ExtendedTabControl tabs;
		public readonly LevelSetsView levelSets;
		public readonly LevelSetsPropertiesView levelSetProperties;

		// TODO: Move level sets list from the left to a new class
		public LevelSetsPage()
		{
			// BackgroundColor = Colors.SlateGray;

			tabs = new ExtendedTabControl();
			tabs.AddPage("Properties", levelSetProperties = new LevelSetsPropertiesView(), scrollable: true);
			tabs.AddPage("Levels", new Panel(), scrollable: true);

			TableRow row = new TableRow()
			{
				Cells =
				{
					new TableCell(levelSets = new LevelSetsView()),
					new TableCell(tabs),
					//new TableCell(new Label() { Text = "Levels are displayed here."})
				}
			};

			Rows.Add(row);

			OnPlaylistSelected(-1);
			levelSets.OnIndexChanged += OnPlaylistSelected;
		}

		private void OnPlaylistSelected(int index)
		{
			tabs.Enabled = index != -1;
		}
	}
}