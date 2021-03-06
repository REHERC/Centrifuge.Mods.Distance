﻿using Eto.Forms;

namespace App.AdventureMaker.Core.Controls
{
	public class ExtendedTabControl : TabControl
	{
		public int AddPage(string title, Control content, bool scrollable = false)
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