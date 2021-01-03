﻿using Distance.AdventureMaker.Common.Enums;
using Eto.Forms;

namespace App.AdventureMaker.Core.Views.Pages
{
	public class OverviewPage : PropertiesListBase
	{
		public OverviewPage()
		{
			AddRow("Campaign title", new TextBox());
			AddRow("Description", new TextBox());
			AddRow("Author", new TextBox());
			AddRow("Contact", new TextBox());
			AddRow("Difficulty rating", new EnumDropDown<Difficulty>());
			CompleteRows();
		}
	}
}
