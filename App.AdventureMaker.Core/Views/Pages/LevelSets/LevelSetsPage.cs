using App.AdventureMaker.Core.Controls;
using App.AdventureMaker.Core.Interfaces;
using Distance.AdventureMaker.Common.Models;
using Eto.Forms;
using System;

namespace App.AdventureMaker.Core.Views
{
	public class LevelSetsPage : TableLayout, ISaveLoad<CampaignFile>
	{
		public readonly ExtendedTabControl tabs;
		public readonly ReorderableListBox listBox;
		public readonly LevelSetsPropertiesView properties;
		private readonly IEditor<CampaignFile> editor;

		public LevelSetsPage(IEditor<CampaignFile> editor_)
		{
			editor = editor_;

			tabs = new ExtendedTabControl();
			tabs.AddPage("Properties", properties = new LevelSetsPropertiesView(), scrollable: true);
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
		}

		private void SelectPlaylist(object sender, EventArgs e)
		{
			tabs.Enabled = listBox.SelectedIndex != -1;
		}

		private void RemovePlaylist(object sender, EventArgs e)
		{
			if (Messages.RemovePlaylist(listBox.SelectedValue as CampaignPlaylist) == DialogResult.Yes)
			{
				listBox.Items.RemoveAt(listBox.SelectedIndex);
				editor.Modified = true;
			}
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