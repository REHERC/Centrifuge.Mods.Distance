using Eto.Drawing;
using Eto.Forms;
using System;

namespace App.AdventureMaker.Core.Controls
{
	public class ReorderableListBox : StackLayout
	{
		#region Members
		public readonly ListBox listBox;
		private readonly TableLayout buttonsTable;

		private readonly Button addButton;
		private readonly ButtonMenuItem addMenuButton;

		private readonly Button removeButton;
		private readonly ButtonMenuItem removeMenuButton;

		private readonly Button moveUpButton;
		private readonly ButtonMenuItem moveUpMenuButton;

		private readonly Button moveDownButton;
		private readonly ButtonMenuItem moveDownMenuButton;
		#endregion

		#region Properties
		public new ListItemCollection Items => listBox.Items;

		public int SelectedIndex
		{
			get => listBox.SelectedIndex;
			set => listBox.SelectedIndex = value;
		}

		public object SelectedValue
		{
			get => listBox.SelectedValue;
			set => listBox.SelectedValue = value;
		}

		public string SelectedKey
		{
			get => listBox.SelectedKey;
			set => listBox.SelectedKey = value;
		}

		public ListBox ListControl => listBox;
		#endregion

		#region Constructors
		public ReorderableListBox()
		{
			MinimumSize = new Size(250, 0);

			buttonsTable = new TableLayout()
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
							(addButton = new Button(OnAddItem)
							{
								Style = "icon",
								Image = Resources.GetIcon("AddGreen.ico", 16)
							}),
							(removeButton = new Button(OnRemoveItem)
							{
								Style = "icon",
								Image = Resources.GetIcon("CloseRed.ico", 16)
							}),
							(moveUpButton = new Button(OnMoveUpItem)
							{
								Style = "icon",
								Image = Resources.GetIcon("UpBlue.ico", 16)
							}),
							(moveDownButton = new Button(OnMoveDownItem)
							{
								Style = "icon",
								Image = Resources.GetIcon("DownBlue.ico", 16)
							}),
							null,
						}
					}
				}
			};

			listBox = new ListBox()
			{
				ContextMenu = new ContextMenu()
				{
					Items =
					{
						(addMenuButton = new ButtonMenuItem(OnAddItem)
						{
							Image = Resources.GetIcon("AddGreen.ico"),
							Text = "Add"
						}),
						(removeMenuButton = new ButtonMenuItem(OnRemoveItem)
						{
							Image = Resources.GetIcon("CloseRed.ico"),
							Text = "Remove"
						}),
						new SeparatorMenuItem(),
						(moveUpMenuButton = new ButtonMenuItem(OnMoveUpItem)
						{
							Image = Resources.GetIcon("UpBlue.ico"),
							Text = "Move up"
						}),
						(moveDownMenuButton = new ButtonMenuItem(OnMoveDownItem)
						{
							Image = Resources.GetIcon("DownBlue.ico"),
							Text = "Move down"
						}),
					}
				}
			};

			base.Items.Add(new StackLayoutItem(listBox) { Expand = true });
			base.Items.Add(new StackLayoutItem(buttonsTable) { Expand = false });

			HorizontalContentAlignment = HorizontalAlignment.Stretch;

			SubscribeToEvents();

			UnselectItem();
		}
		#endregion

		#region Methods
		private void MoveItem(int shift)
		{
			int idx = SelectedIndex;
			raiseListEvents = false;
			Items.Move(idx, idx + shift);
			SelectedIndex = idx + shift;
			raiseListEvents = true;

			ItemsReordered?.Invoke(this, EventArgs.Empty);
		}

		public void SelectIndex(int index)
		{
			var e = EventArgs.Empty;

			raiseListEvents = false;
			SelectedIndex = index;
			OnSelectedIndexChanged(listBox, e);
			raiseListEvents = true;

			SelectedIndexChanged?.Invoke(listBox, e);
			SelectedKeyChanged?.Invoke(listBox, e);
			SelectedValueChanged?.Invoke(listBox, e);
		}

		public void UnselectItem()
		{
			SelectIndex(-1);
		}
		#endregion

		#region Events
		#region Flags
		private bool raiseListEvents = true;
		#endregion
		#region Declarations
		public event EventHandler<EventArgs> AddItem;
		public event EventHandler<EventArgs> RemoveItem;

		public event EventHandler<EventArgs> SelectedIndexChanged;
		public event EventHandler<EventArgs> SelectedKeyChanged;
		public event EventHandler<EventArgs> SelectedValueChanged;
		public event EventHandler<EventArgs> ItemsReordered;
		#endregion
		#region Event Subscribing
		private void SubscribeToEvents()
		{
			listBox.SelectedIndexChanged += (sender, e) =>
			{
				OnSelectedIndexChanged(sender, e);
				SelectedIndexChanged?.Invoke(sender, e);
			};

			listBox.SelectedKeyChanged += (sender, e) =>
			{
				if (raiseListEvents) SelectedKeyChanged?.Invoke(sender, e);
			};

			listBox.SelectedValueChanged += (sender, e) =>
			{
				if (raiseListEvents) SelectedValueChanged?.Invoke(sender, e);
			};
		}
		#endregion
		#region Event Handlers
		#region List Events
		protected void OnSelectedIndexChanged(object sender, EventArgs e)
		{
			int idx = SelectedIndex;
			removeButton.Enabled = removeMenuButton.Enabled = idx >= 0;
			moveUpButton.Enabled = moveUpMenuButton.Enabled = idx > 0;
			moveDownButton.Enabled = moveDownMenuButton.Enabled = idx >= 0 && idx < Items.Count - 1;
		}
		#endregion
		#region Button/Menu Events
		protected virtual void OnAddItem(object sender, EventArgs e)
		{
			AddItem?.Invoke(sender, e);
		}

		protected virtual void OnRemoveItem(object sender, EventArgs e)
		{
			RemoveItem?.Invoke(sender, e);
		}

		protected void OnMoveUpItem(object sender, EventArgs e)
		{
			MoveItem(-1);
		}

		protected void OnMoveDownItem(object sender, EventArgs e)
		{
			MoveItem(+1);
		}
		#endregion
		#endregion
		#endregion
	}
}