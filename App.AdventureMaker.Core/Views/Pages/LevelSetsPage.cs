using Eto.Drawing;
using Eto.Forms;

namespace App.AdventureMaker.Core.Views.Pages
{
	public class LevelSetsPage : TableLayout
	{
		public LevelSetsPage()
		{
			// BackgroundColor = Colors.SlateGray;

			TableRow row = new TableRow()
			{
				Cells =
				{
					new TableCell()
					{
						Control = new StackLayout()
						{
							Width = 200,
							Orientation = Orientation.Vertical,
							HorizontalContentAlignment = HorizontalAlignment.Stretch,
							Items =
							{
								new StackLayoutItem(new ListBox()
								{
									// BackgroundColor = Colors.Crimson,
								})
								{
									Expand = true
								},
								new StackLayoutItem(new StackLayout()
								{
									// BackgroundColor = Colors.DarkRed,
									Style = "no-padding",
									Spacing = 0,
									Orientation = Orientation.Horizontal,
									VerticalContentAlignment = VerticalAlignment.Stretch,
									HorizontalContentAlignment = HorizontalAlignment.Center,
									Height = 32,
									Items =
									{
										new Button() { Style = "icon", Image = Resources.GetIcon("AddGreen.ico", 16) },
										new Button() { Style = "icon", Image = Resources.GetIcon("CloseRed.ico", 16), Enabled = false },
										new Button() { Style = "icon", Image = Resources.GetIcon("UpBlue.ico", 16), Enabled = false },
										new Button() { Style = "icon", Image = Resources.GetIcon("DownBlue.ico", 16), Enabled = false }
									}
								})
								{
									Expand = false
								},
							},

							Padding = Padding.Empty
						}
					},
					new TableCell(new Label() { Text = "Levels are displayed here."})
				}
			};

			Rows.Add(row);
		}
	}
}