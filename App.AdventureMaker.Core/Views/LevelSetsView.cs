using Eto.Drawing;
using Eto.Forms;

namespace App.AdventureMaker.Core.Views
{
	public class LevelSetsView : StackLayout
	{
		public readonly ListBox itemList;

		public readonly TableLayout buttonTable;
		public readonly Button addButton;
		public readonly Button removeButton;
		public readonly Button moveUpButton;
		public readonly Button moveDownButton;
		
		public LevelSetsView()
		{
			itemList = new ListBox();

			addButton = new Button()
			{
				Style = "icon",
				//Text = "Add",
				Image = Resources.GetIcon("AddGreen.ico", 16)
			};

			removeButton = new Button()
			{
				Style = "icon",
				//Text = "Remove",
				Image = Resources.GetIcon("CloseRed.ico", 16)
			};

			moveUpButton = new Button()
			{
				Style = "icon",
				//Text = "Move Up",
				Image = Resources.GetIcon("UpBlue.ico", 16)
			};

			moveDownButton = new Button()
			{
				Style = "icon",
				//Text = "Move Down",
				Image = Resources.GetIcon("DownBlue.ico", 16)
			};

			buttonTable = new TableLayout()
			{
				Style = "no-padding",
				Spacing = Size.Empty,
				Rows =
				{
					new TableRow()
					{
						Cells =
						{
							null,
							addButton,
							removeButton,
							moveUpButton,
							moveDownButton,
							null,
						}
					}
				}
			};

			MinimumSize = new Size(250, 0);
			Orientation = Orientation.Vertical;
			HorizontalContentAlignment = HorizontalAlignment.Stretch;
			Style = "no-padding";

			Items.Add(new StackLayoutItem(itemList) { Expand = true });
			Items.Add(new StackLayoutItem(buttonTable) { Expand = false });
		}
	}
}