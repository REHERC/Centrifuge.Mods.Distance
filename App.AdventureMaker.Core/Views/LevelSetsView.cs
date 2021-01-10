using Eto.Drawing;
using Eto.Forms;
using System;

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

		public event Action<int> OnIndexChanged;

		private bool raiseEvents = true;

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

			itemList.Items.Add("Item 1");
			itemList.Items.Add("Item 2");
			itemList.Items.Add("Item 3");
			itemList.SelectedIndexChanged += (sender, e) => OnItemSelected(itemList.SelectedIndex);
			OnItemSelected(itemList.SelectedIndex = -1);


			removeButton.Click += OnRemoveClicked;
			moveUpButton.Click += OnMoveUpClicked;
			moveDownButton.Click += OnMoveDownClicked;
		}

		private void OnItemSelected(int index)
		{
			if (raiseEvents)
			{
				removeButton.Enabled = index != -1;
				moveUpButton.Enabled = index > 0;
				moveDownButton.Enabled = removeButton.Enabled && index < itemList.Items.Count - 1;

				OnIndexChanged?.Invoke(index);
			}
		}

		private void OnRemoveClicked(object sender, EventArgs e)
		{
			if (MessageBox.Show("Are you sure you want to remove this level set ?", "Remove playlist", MessageBoxButtons.YesNo, MessageBoxType.Warning) == DialogResult.Yes)
			{
				int currentIndex = itemList.SelectedIndex;
				itemList.Items.RemoveAt(currentIndex);
			}
		}

		private void OnMoveUpClicked(object sender, EventArgs e)
		{
			int currentIndex = itemList.SelectedIndex;
			raiseEvents = false;
			itemList.Items.Move(currentIndex, currentIndex - 1);
			raiseEvents = true;
			itemList.SelectedIndex = currentIndex - 1;
		}

		private void OnMoveDownClicked(object sender, EventArgs e)
		{
			int currentIndex = itemList.SelectedIndex;
			raiseEvents = false;
			itemList.Items.Move(currentIndex, currentIndex + 1);
			raiseEvents = true;
			itemList.SelectedIndex = currentIndex + 1;
		}
	}
}