using App.AdventureMaker.Core.Controls;
using Distance.AdventureMaker.Common.Models;
using Eto.Drawing;
using Eto.Forms;
using static Constants;

namespace App.AdventureMaker.Core.Views
{
	public class LevelSetsPlaylistView : StackLayout
	{

		public LevelSetsPlaylistView()
		{
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
						Image = Resources.GetIcon("Pencil.ico", 16)
					},
					new Button()
					{
						Style = "icon",
						Image = Resources.GetIcon("CloseRed.ico", 16)
					},
					new Button()
					{
						Style = "icon",
						Image = Resources.GetIcon("UpBlue.ico", 16)
					},
					new Button()
					{
						Style = "icon",
						Image = Resources.GetIcon("DownBlue.ico", 16)
					},
					null
				}
			}, false));

			Items.Add(new StackLayoutItem(new GridView()
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

						DataCell = new TextBoxCell()
						{
							Binding = Binding.Property<CampaignLevel, string>(lvl => lvl.name)
						}
					},
					new GridColumn()
					{
						HeaderText = "Intro title",
						Editable = false,
						Sortable = false,
						Resizable = true,

						DataCell = new TextBoxCell()
						{
							Binding = Binding.Property<CampaignLevel, string>(lvl => lvl.title)
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
		}
	}
}
