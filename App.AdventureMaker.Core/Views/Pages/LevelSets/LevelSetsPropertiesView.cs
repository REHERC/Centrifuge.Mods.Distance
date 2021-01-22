using App.AdventureMaker.Core.Controls;
using App.AdventureMaker.Core.Interfaces;
using Distance.AdventureMaker.Common.Models;
using Eto.Forms;
using System;

namespace App.AdventureMaker.Core.Views
{
	public class LevelSetsPropertiesView : StackLayout, ISaveLoad<CampaignPlaylist>, IModifiedNotifier<CampaignPlaylist>
	{
		public event Action<CampaignPlaylist> OnModified;

		private bool raiseEvents = true;

		private readonly PropertiesListBase properties;
		private readonly IEditor<CampaignFile> editor;

		private readonly TextBox nameBox;
		private readonly TextBox descriptionBox;
		private readonly TextBoxWithButton iconBox;
		private readonly GuidLabel guidBox;

		public LevelSetsPropertiesView(IEditor<CampaignFile> editor_)
		{
			editor = editor_;

			//Style = "no-padding";
			Orientation = Orientation.Vertical;
			HorizontalContentAlignment = HorizontalAlignment.Stretch;

			properties = new PropertiesListBase();

			properties.AddRow("Name", nameBox = new TextBox());
			properties.AddRow("Description", descriptionBox = new TextBox());
			properties.AddRow("Icon", iconBox = new TextBoxWithButton());
			properties.AddRow("Unique ID", guidBox = new GuidLabel());
			//properties.AddRow("Playlist locking", new EnumDropDown<PlaylistUnlock>());
			//properties.AddRow("Individual level locking", new EnumDropDown<LevelUnlock>());
			//properties.AddRow("Difficulty rating", new EnumDropDown<Difficulty>());

			properties.CompleteRows();

			Items.Add(new StackLayoutItem(properties) { Expand = false });

			nameBox.TextChanged += NotifyModified;
			descriptionBox.TextChanged += NotifyModified;
			iconBox.TextChanged += NotifyModified;
			guidBox.TextChanged += NotifyModified;
		}

		private void NotifyModified(object sender, EventArgs e)
		{
			if (!raiseEvents) return;

			var playlist = new CampaignPlaylist();
			SaveData(playlist);
			OnModified?.Invoke(playlist);
		}

		public void LoadData(CampaignPlaylist playlist)
		{
			if (playlist != null)
			{
				raiseEvents = false;

				nameBox.Text = playlist.name;
				descriptionBox.Text = playlist.description;
				iconBox.Text = playlist.icon;
				guidBox.Text = playlist.guid;

				raiseEvents = true;
			}
		}

		public void SaveData(CampaignPlaylist playlist)
		{
			//raiseEvents = false;

			playlist.name = nameBox.Text;
			playlist.description = descriptionBox.Text;
			playlist.icon = iconBox.Text;
			playlist.guid = guidBox.Text;

			//raiseEvents = true;

			//NotifyModified(null, null);
		}
	}
}
