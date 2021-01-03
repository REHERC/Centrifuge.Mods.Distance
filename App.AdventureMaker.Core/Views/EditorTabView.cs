﻿using App.AdventureMaker.Core.Views.Pages;
using Eto.Forms;

namespace App.AdventureMaker.Core.Views
{
	public class EditorTabView : TabControl
	{
		public EditorTabView()
		{
			AddPage("Overview", new OverviewPage(), scrollable: true);
			AddPage("Level Sets", new LevelSetsPage(), scrollable: false);
		}

		protected int AddPage(string title, Control content, bool scrollable = false)
		{

			Panel host = scrollable ? new Scrollable() : new Panel();
			host.Content = content;

			Pages.Add(new TabPage()
			{
				Text = title,
				Content = host
			});

			return Pages.Count - 1;
		}
	}
}
