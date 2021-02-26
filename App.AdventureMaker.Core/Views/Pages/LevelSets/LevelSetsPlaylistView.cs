using App.AdventureMaker.Core.Controls;
using App.AdventureMaker.Core.Interfaces;
using Distance.AdventureMaker.Common.Models;
using Eto.Drawing;
using Eto.Forms;
using System.Collections.Generic;
using static Constants;

namespace App.AdventureMaker.Core.Views
{
	public class LevelSetsPlaylistView : StackLayout, ISaveLoad<CampaignPlaylist>
	{
		private readonly IEditor<CampaignFile> editor;
		private readonly GridView<CampaignLevel> levelList;

		public LevelSetsPlaylistView(IEditor<CampaignFile> editor)
		{
			this.editor = editor;

			Orientation = Orientation.Vertical;
			HorizontalContentAlignment = HorizontalAlignment.Stretch;
			Spacing = 0;

			Items.Add(new StackLayoutItem(new StackLayout()
			{
				Orientation = Orientation.Horizontal,
				VerticalContentAlignment = VerticalAlignment.Stretch,
				Spacing = 0,
				Style = "no-padding",

				Items =
				{
					null,
					new Button()
					{
						Style = "icon",
						Image = Resources.GetIcon("AddGreen.ico", 16)
					},
					new Button()
					{
						Style = "icon",
						Image = Resources.GetIcon("Pencil.ico", 16),
						Enabled = false
					},
					new Button()
					{
						Style = "icon",
						Image = Resources.GetIcon("CloseRed.ico", 16),
						Enabled = false
					},
					new Button()
					{
						Style = "icon",
						Image = Resources.GetIcon("UpBlue.ico", 16),
						Enabled = false
					},
					new Button()
					{
						Style = "icon",
						Image = Resources.GetIcon("DownBlue.ico", 16),
						Enabled = false
					},
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
						HeaderText = "Level name",
						Editable = false,
						Sortable = false,
						Resizable = true,
						Width = 200,

						DataCell = new TextBoxCell()
						{
							Binding = Binding.Property<CampaignLevel, string>(lvl => $"{lvl.name}")
						}
					},
					new GridColumn()
					{
						HeaderText = "Title",
						Editable = false,
						Sortable = false,
						Resizable = true,
						Width = 200,

						DataCell = new TextBoxCell()
						{
							Binding = Binding.Property<CampaignLevel, string>(lvl => $"{lvl.title}")
						}
					}
				}
			}, true));

			Items.Add(new StackLayoutItem(new StackLayout()
			{
				Orientation = Orientation.Horizontal,
				VerticalContentAlignment = VerticalAlignment.Stretch,
				Spacing = 0,
				Style = "no-padding",

				BackgroundColor = Color.FromArgb(48, 48, 48),

				Height = THUMBNAIL_HEIGHT_SMALL,

				Items =
				{
					new StackLayoutItem(new LevelThumbnail()
					{
						//Image = Resources.GetImage("NoLevelImageFound.png"),
						Image = Resources.GetImage("sample thumbnail.bytes.png"),
						GradientDelta = 0.1f,
						Width = THUMBNAIL_WIDTH_SMALL,
						Height = THUMBNAIL_HEIGHT_SMALL
					}, false),
					new StackLayoutItem(new StackLayout()
					{
						Orientation = Orientation.Vertical,
						HorizontalContentAlignment = HorizontalAlignment.Stretch,
						Spacing = 4,
						Padding = new Padding(18),

						//BackgroundColor = Colors.RoyalBlue,

						Items =
						{
							new Label()
							{
								Text = "LEVEL NAME",
								TextColor = Colors.White,
								Font = new Font(FontFamilies.Sans, 14, FontStyle.Bold)
							},
							new Label()
							{
								Text = "Second line æ",
								TextColor = Colors.White,
								Font = new Font(FontFamilies.Sans, 12, FontStyle.None)
							}
						}
					}, true)
				}
			}, false));

			levelList.DataStore = new List<CampaignLevel>()
			{
				new CampaignLevel()
				{
					name = "Level 1",
					title = "AZERTY"
				}
			};
		}

		public void LoadData(CampaignPlaylist project, bool resetUI = true)
		{
			throw new System.NotImplementedException();
		}

		public void SaveData(CampaignPlaylist project)
		{
			throw new System.NotImplementedException();
		}
	}
}
