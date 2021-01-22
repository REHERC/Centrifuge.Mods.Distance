using App.AdventureMaker.Core.Controls;
using App.AdventureMaker.Core.Interfaces;
using Distance.AdventureMaker.Common.Models;
using Eto.Forms;
using System;

namespace App.AdventureMaker.Core.Views
{
	public class LevelSetsPage : TableLayout, ISaveLoad<CampaignFile>
	{
		private readonly ExtendedTabControl tabs;
		private readonly ReorderableListBox listBox;
		private readonly LevelSetsPropertiesView properties;
		private readonly IEditor<CampaignFile> editor;

		public LevelSetsPage(IEditor<CampaignFile> editor_)
		{
			editor = editor_;

			tabs = new ExtendedTabControl();
			tabs.AddPage("Properties", properties = new LevelSetsPropertiesView(editor), scrollable: true);
			tabs.AddPage("Levels", new Panel(), scrollable: true);

			TableRow row = new TableRow()
			{
				Cells =
				{
					new TableCell(listBox = new ReorderableListBox()),
					new TableCell(tabs),
				}
			};
			Rows.Add(row);

			listBox.SelectedKeyChanged += SelectPlaylist;
			listBox.ItemsReordered += (_, __) => editor.Modified = true;

			listBox.RemoveItem += RemovePlaylist;
			listBox.AddItem += AddPlaylist;

			properties.OnModified += (playlist) =>
			{
				properties.SaveData(listBox.SelectedValue as CampaignPlaylist);

				listBox.ListControl.UpdateBindings();
				listBox.ListControl.Invalidate();

				editor.Modified = true;
			};
		}

		private void SelectPlaylist(object sender, EventArgs e)
		{
			tabs.Enabled = listBox.SelectedIndex != -1;
			properties.LoadData(listBox.SelectedValue as CampaignPlaylist);
		}

		private void RemovePlaylist(object sender, EventArgs e)
		{
			if (Messages.RemovePlaylist(listBox.SelectedValue as CampaignPlaylist) == DialogResult.Yes)
			{
				listBox.Items.RemoveAt(listBox.SelectedIndex);
				editor.Modified = true;
			}
		}

		private void AddPlaylist(object sender, EventArgs e)
		{
			// TODO: Implement
			var playlist = new CampaignPlaylist()
			{
				guid = Guid.NewGuid().ToString()
			};

			listBox.Items.Add(playlist);
			listBox.SelectedValue = playlist;
		}

		public void LoadData(CampaignFile project)
		{
			listBox.Items.Clear();
			listBox.UnselectItem();

			foreach (CampaignPlaylist playlist in project.data.playlists)
			{
				listBox.Items.Add(playlist);
			}
		}

		public void SaveData(CampaignFile project)
		{
			foreach (CampaignPlaylist playlist in listBox.Items)
			{
				project.data.playlists.Add(playlist);
			}
		}
	}
}