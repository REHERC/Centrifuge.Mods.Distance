using App.AdventureMaker.Core.Controls;
using App.AdventureMaker.Core.Interfaces;
using Distance.AdventureMaker.Common.Enums;
using Distance.AdventureMaker.Common.Models;
using Eto.Forms;
using System;
using System.Linq;

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
		private readonly ResourceSelector iconBox;
		private readonly GuidLabel guidBox;
		private readonly BooleanSelector campaignDisplayBox;
		private readonly BooleanSelector sprintDisplayBox;

		public LevelSetsPropertiesView(IEditor<CampaignFile> editor_)
		{
			editor = editor_;

			//Style = "no-padding";
			Orientation = Orientation.Vertical;
			HorizontalContentAlignment = HorizontalAlignment.Stretch;

			properties = new PropertiesListBase();

			properties.AddRow("Name", nameBox = new TextBox());
			properties.AddRow("Description", descriptionBox = new TextBox());
			properties.AddRow("Icon", iconBox = new ResourceSelector(editor, ResourceType.Texture));

			properties.AddRow("Campaign playlist", campaignDisplayBox = new BooleanSelector());
			properties.AddRow("Sprint playlist", sprintDisplayBox = new BooleanSelector());

			properties.AddRow("Unique ID", guidBox = new GuidLabel());
			//properties.AddRow("Playlist locking", new EnumDropDown<PlaylistUnlock>());
			//properties.AddRow("Individual level locking", new EnumDropDown<LevelUnlock>());
			//properties.AddRow("Difficulty rating", new EnumDropDown<Difficulty>());

			properties.CompleteRows();

			Items.Add(new StackLayoutItem(properties) { Expand = false });

			nameBox.TextChanged += NotifyModified;
			descriptionBox.TextChanged += NotifyModified;
			iconBox.ResourceSelected += NotifyModified;
			guidBox.TextChanged += NotifyModified;
			sprintDisplayBox.ValueChanged += NotifyModified;
			campaignDisplayBox.ValueChanged += NotifyModified;
		}

		private void NotifyModified(object sender, EventArgs e)
		{
			if (!raiseEvents) return;

			var playlist = new CampaignPlaylist();
			SaveData(playlist);
			OnModified?.Invoke(playlist);
		}

		public void LoadData(CampaignPlaylist playlist, bool resetUI)
		{
			if (playlist != null)
			{
				raiseEvents = false;

				nameBox.Text = playlist.Name;
				descriptionBox.Text = playlist.Description;

				//var resourceQuery = editor.Document.Data.Resources.Where(res => Equals(res.guid, playlist.Icon));
				//iconBox.Resource = resourceQuery.FirstOrDefault();

				iconBox.Resource = editor.Document.GetResource(playlist.Icon, ResourceType.Texture);

				guidBox.Text = playlist.Guid;
				sprintDisplayBox.Value = playlist.DisplayInSprint;
				campaignDisplayBox.Value = playlist.DisplayInCampaign;

				raiseEvents = true;
			}
		}

		public void SaveData(CampaignPlaylist playlist)
		{
			//raiseEvents = false;

			playlist.Name = nameBox.Text;
			playlist.Description = descriptionBox.Text;
			playlist.Icon = iconBox.Resource?.guid;
			playlist.Guid = guidBox.Text;
			playlist.DisplayInSprint = sprintDisplayBox.Value;
			playlist.DisplayInCampaign = campaignDisplayBox.Value;
			//raiseEvents = true;

			//NotifyModified(null, null);
		}
	}
}
