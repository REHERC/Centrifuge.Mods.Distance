using App.AdventureMaker.Core.Controls;
using App.AdventureMaker.Core.Forms;
using App.AdventureMaker.Core.Interfaces;
using Distance.AdventureMaker.Common.Models;
using Eto.Drawing;
using Eto.Forms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using static Constants;

namespace App.AdventureMaker.Core.Views
{
	public class LevelSetsPlaylistView : StackLayout, ISaveLoad<CampaignPlaylist>
	{
		public event EventHandler OnModified;

		private readonly IEditor<CampaignFile> editor;

		private readonly StretchedImageBox selectionThumbnail;
		private readonly Label selectionHeader;
		private readonly Label selectionText;

		private readonly Button addButton;
		private readonly ButtonMenuItem addMenuButton;
		private readonly Button editButton;
		private readonly ButtonMenuItem editMenuButton;
		private readonly Button removeButton;
		private readonly ButtonMenuItem removeMenuButton;
		private readonly Button moveUpButton;
		private readonly ButtonMenuItem moveUpMenuButton;
		private readonly Button moveDownButton;
		private readonly ButtonMenuItem moveDownMenuButton;

		private readonly GridView<CampaignLevel> levelList;

		private readonly ObservableCollection<CampaignLevel> collection;

		public LevelSetsPlaylistView(IEditor<CampaignFile> editor)
		{
			this.editor = editor;
			collection = new ObservableCollection<CampaignLevel>();

			Style = "vertical";
			Spacing = 0;

			Items.Add(new StackLayoutItem(new StackLayout()
			{
				Spacing = 0,
				Style = "no-padding horizontal",

				Items =
				{
					null,
					(addButton = new Button(AddLevel)
					{
						Style = "icon",
						Image = Resources.GetIcon("AddGreen.ico", 16)
					}),
					(editButton = new Button(EditLevel)
					{
						Style = "icon",
						Image = Resources.GetIcon("Pencil.ico", 16),
						Enabled = false
					}),
					(removeButton = new Button(RemoveLevel)
					{
						Style = "icon",
						Image = Resources.GetIcon("CloseRed.ico", 16),
						Enabled = false
					}),
					(moveUpButton = new Button(MoveLevelUp)
					{
						Style = "icon",
						Image = Resources.GetIcon("UpBlue.ico", 16),
						Enabled = false
					}),
					(moveDownButton = new Button(MoveLevelDown)
					{
						Style = "icon",
						Image = Resources.GetIcon("DownBlue.ico", 16),
						Enabled = false
					}),
					null
				}
			}, false));

			Items.Add(new StackLayoutItem(levelList = new GridView<CampaignLevel>()
			{
				GridLines = GridLines.Both,

				Columns =
				{
					new GridColumn()
					{
						HeaderText = string.Empty,
						Editable = false,
						Sortable = false,
						Resizable = false,
						Width = 20,

						DataCell = new TextBoxCell()
						{
							Binding = Binding.Property<CampaignLevel, string>(lvl => $"{collection.IndexOf(lvl) + 1}")
						}
					},
					new GridColumn()
					{
						HeaderText = "Level name",
						Editable = false,
						Sortable = false,
						Resizable = true,
						Width = 190,

						DataCell = new TextBoxCell()
						{
							Binding = Binding.Property<CampaignLevel, string>(lvl => $"{lvl.Name}")
						}
					},
					new GridColumn()
					{
						HeaderText = "Title",
						Editable = false,
						Sortable = false,
						Resizable = true,
						Width = 190,

						DataCell = new TextBoxCell()
						{
							Binding = Binding.Property<CampaignLevel, string>(lvl => $"{lvl.Title}")
						}
					}
				},

				ContextMenu = new ContextMenu
				(
					addMenuButton = new ButtonMenuItem(AddLevel)
					{
						Image = Resources.GetIcon("AddGreen.ico"),
						Text = "Add new level to playlist"
					},
					editMenuButton = new ButtonMenuItem(EditLevel)
					{
						Image = Resources.GetIcon("Pencil.ico"),
						Text = "Edit level information"
					},
					removeMenuButton = new ButtonMenuItem(RemoveLevel)
					{
						Image = Resources.GetIcon("CloseRed.ico"),
						Text = "Remove level from playlist"
					},
					new SeparatorMenuItem(),
					moveUpMenuButton = new ButtonMenuItem(MoveLevelUp)
					{
						Image = Resources.GetIcon("UpBlue.ico"),
						Text = "Move level up"
					},
					moveDownMenuButton = new ButtonMenuItem(MoveLevelDown)
					{
						Image = Resources.GetIcon("DownBlue.ico"),
						Text = "Move level down"
					}
				)
			}, true));

			Items.Add(new StackLayoutItem(new StackLayout()
			{
				Spacing = 0,
				Style = "no-padding horizontal",

				BackgroundColor = Color.FromArgb(48, 48, 48),

				Height = THUMBNAIL_HEIGHT_SMALL,

				Items =
				{
					new StackLayoutItem(selectionThumbnail = new StretchedImageBox()
					{
						//Image = Resources.GetImage("NoLevelImageFound.png"),
						Image = Resources.GetImage("sample thumbnail.bytes.png"),
						Width = THUMBNAIL_WIDTH_SMALL,
						Height = THUMBNAIL_HEIGHT_SMALL
					}, false),
					new StackLayoutItem(new StackLayout()
					{
						Style = "vertical",
						HorizontalContentAlignment = HorizontalAlignment.Left,
						Spacing = 4,
						Padding = new Padding(18),

						Items =
						{
							(selectionHeader = new Label()
							{
								Text = "LEVEL NAME",
								TextColor = Colors.White,
								Font = new Font(FontFamilies.Sans, 12, FontStyle.Bold)
							}),
							(selectionText = new Label()
							{
								Text = "Second line æ",
								TextColor = Colors.White,
								Font = new Font(FontFamilies.Sans, 10, FontStyle.None)
							})
						}
					}, true)
				}
			}, false));

			levelList.DataStore = collection;

			levelList.CellDoubleClick += EditLevel;
			levelList.SelectionChanged += SelectionChanged;

			OnModified += (sender, e) => editor.Modified = true;
		}

		private void SelectionChanged(object sender, EventArgs e)
		{
			CampaignLevel level = levelList.SelectedItem;

			if (Equals(level, null))
			{
				level = new CampaignLevel()
				{
					Name = " ",
					Title = "<no level selected>"
				};
			}

			selectionHeader.Text = level?.Name?.ToUpper();
			selectionText.Text = level?.Title;

			int index = levelList.SelectedRow;

			moveUpButton.Enabled
			= moveUpMenuButton.Enabled
			= index > 0;

			moveDownButton.Enabled
			= moveDownMenuButton.Enabled
			= index >= 0 && index < collection.Count - 1;

			editButton.Enabled
			= editMenuButton.Enabled
			= removeButton.Enabled
			= removeMenuButton.Enabled
			= index != -1;
		}

		private void AddLevel(object sender, EventArgs e)
		{
			CampaignLevel level = new LevelPropertiesWindow(editor, null).ShowModal();

			if (!Equals(level, null))
			{
				collection.Add(level);
				OnModified?.Invoke(this, EventArgs.Empty);
			}
		}

		private void EditLevel(object sender, EventArgs e)
		{
			CampaignLevel level = levelList.SelectedItem;

			if (!Equals(level, null))
			{
				int index = levelList.SelectedRow;

				level = new LevelPropertiesWindow(editor, level).ShowModal();

				if (!Equals(level, null))
				{
					collection[index] = level;
					OnModified?.Invoke(this, EventArgs.Empty);

					levelList.SuspendLayout();

					levelList.UpdateBindings();
					levelList.Invalidate();

					levelList.SelectedRow = index;

					levelList.ResumeLayout();
				}
			}
		}

		private void RemoveLevel(object sender, EventArgs e)
		{
			CampaignLevel level = levelList.SelectedItem;

			if (!Equals(level, null) && Messages.RemoveLevel(level) == DialogResult.Yes)
			{
				collection.Remove(level);
				OnModified?.Invoke(this, EventArgs.Empty);
			}
		}

		private void MoveLevelUp(object sender, EventArgs e)
		{
			MoveCurrentItem(-1);
		}

		private void MoveLevelDown(object sender, EventArgs e)
		{
			MoveCurrentItem(+1);
		}

		private void MoveCurrentItem(int offset)
		{
			int currentIndex = levelList.SelectedRow;
			int newIndex = currentIndex + offset;

			levelList.SuspendLayout();

			collection.Move(currentIndex, newIndex);
			levelList.SelectedRow = newIndex;

			levelList.ResumeLayout();

			OnModified?.Invoke(this, EventArgs.Empty);
		}

		public void LoadData(CampaignPlaylist playlist, bool resetUI = true)
		{
			collection.Clear();

			levelList.SuspendLayout();

			foreach (CampaignLevel level in playlist?.Levels?.ToArray() ?? new CampaignLevel[0])
			{
				collection.Add(level);
			}

			levelList.ResumeLayout();
			levelList.UpdateBindings();
		}

		public void SaveData(CampaignPlaylist playlist)
		{
			playlist.Levels = new List<CampaignLevel>();

			foreach (CampaignLevel level in collection)
			{
				playlist.Levels.Add(level);
			}
		}
	}
}
