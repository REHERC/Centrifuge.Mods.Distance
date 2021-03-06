﻿using App.AdventureMaker.Core.Controls;
using App.AdventureMaker.Core.Interfaces;
using Distance.AdventureMaker.Common.Models;
using Eto.Forms;
using System.Collections.Generic;

namespace App.AdventureMaker.Core.Views
{
	public class EditorTabView : ExtendedTabControl, ISaveLoad<CampaignFile>
	{
		private readonly List<ISaveLoad<CampaignFile>> pages = new List<ISaveLoad<CampaignFile>>();

		public EditorTabView(MainView editor)
		{
			AddPage("Overview", new OverviewPage(editor), scrollable: true);
			AddPage("Level Sets", new LevelSetsPage(editor), scrollable: true);
			//AddPage("Achievements", null, scrollable: true);
			//AddPage("Editor Prefabs", null, scrollable: true);
			//AddPage("Audio Engine", null, scrollable: true);
			AddPage("Resources", new ResourcesPage(editor), scrollable: false);

			editor.OnLoaded += OnFileLoad;
		}

		private void OnFileLoad(IEditor<CampaignFile> editor)
		{
			if (editor.CurrentFile != null)
			{
				SelectedIndex = 0;
			}
		}

		public new int AddPage(string title, Control content, bool scrollable = false)
		{
			if (content is ISaveLoad<CampaignFile> page)
			{
				pages.Add(page);
			}

			return base.AddPage(title, content, scrollable);
		}

		public void LoadData(CampaignFile project, bool resetUI)
		{
			pages.ForEach(page => page.LoadData(project, resetUI));
		}

		public void SaveData(CampaignFile project)
		{
			pages.ForEach(page => page.SaveData(project));
		}
	}
}
