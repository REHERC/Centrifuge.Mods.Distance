﻿using App.AdventureMaker.Core.Controls;
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
		private readonly LevelSetsPlaylistView levels;
		private readonly IEditor<CampaignFile> editor;

		public LevelSetsPage(IEditor<CampaignFile> editor)
		{
			this.editor = editor;

			tabs = new ExtendedTabControl();

			tabs.AddPage("Properties", properties = new LevelSetsPropertiesView(this.editor), scrollable: true);
			tabs.AddPage("Levels", levels = new LevelSetsPlaylistView(editor), scrollable: false);

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
			listBox.ItemsReordered += (_, __) => this.editor.Modified = true;

			listBox.RemoveItem += RemovePlaylist;
			listBox.AddItem += AddPlaylist;

			properties.OnModified += PropertiesModified;

			levels.OnModified += PlaylistLevelsModified;
		}

		private void PropertiesModified(CampaignPlaylist playlist)
		{
			properties.SaveData(listBox.SelectedValue as CampaignPlaylist);

			listBox.ListControl.UpdateBindings();
			listBox.ListControl.Invalidate();

			editor.Modified = true;
		}

		private void PlaylistLevelsModified(object sender, EventArgs e)
		{
			CampaignPlaylist playlist = listBox.SelectedValue as CampaignPlaylist;

			if (!Equals(playlist, null))
			{
				levels.SaveData(playlist);
			}

			editor.Modified = true;
		}

		private void SelectPlaylist(object sender, EventArgs e)
		{
			tabs.Enabled = listBox.SelectedIndex != -1;

			CampaignPlaylist playlist = listBox.SelectedValue as CampaignPlaylist;
			properties.LoadData(playlist, true);
			levels.LoadData(playlist);
		}

		private void RemovePlaylist(object sender, EventArgs e)
		{
			if (Messages.RemovePlaylistDialog(listBox.SelectedValue as CampaignPlaylist) == DialogResult.Yes)
			{
				listBox.Items.RemoveAt(listBox.SelectedIndex);
				editor.Modified = true;
			}
		}

		private void AddPlaylist(object sender, EventArgs e)
		{
			var playlist = new CampaignPlaylist()
			{
				Guid = Guid.NewGuid().ToString(),
				DisplayInCampaign = true
			};

			listBox.Items.Add(playlist);
			listBox.SelectedValue = playlist;

			editor.Modified = true;
		}

		public void LoadData(CampaignFile project, bool resetUI)
		{
			if (!resetUI)
			{
				listBox.SuspendLayout();
				properties.SuspendLayout();
			}

			int row = listBox.SelectedIndex;

			listBox.Items.Clear();
			listBox.UnselectItem();

			foreach (CampaignPlaylist playlist in project.Data.Playlists)
			{
				listBox.Items.Add(playlist);
			}

			if (!resetUI)
			{
				listBox.SelectedIndex = row;
				SelectPlaylist(listBox, EventArgs.Empty);

				listBox.ResumeLayout();
				properties.ResumeLayout();
			}
		}

		public void SaveData(CampaignFile project)
		{
			foreach (CampaignPlaylist playlist in listBox.Items)
			{
				project.Data.Playlists.Add(playlist);
			}
		}
	}
}