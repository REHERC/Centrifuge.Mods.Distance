using App.AdventureMaker.Core.Controls;
using Distance.AdventureMaker.Common.Enums;
using Eto.Forms;
using System;

namespace App.AdventureMaker.Core.Views.Pages
{
	public class OverviewPage : PropertiesListBase
	{
		public string Value { get; set; }

		public readonly TextBox titleBox;
		public readonly TextBox descriptionBox;
		public readonly TextBox authorBox;
		public readonly TextBox contactBox;

		public OverviewPage()
		{
			titleBox = new TextBox();
			descriptionBox = new TextBox();
			authorBox = new TextBox();
			contactBox = new TextBox();

			AddRow("Campaign title", titleBox);
			AddRow("Description", descriptionBox);
			AddRow("Author", authorBox);
			AddRow("Contact", contactBox);
			AddRow("Difficulty rating", new EnumDropDown<Difficulty>());
			AddRow("Development build", new CheckBox());
			AddRow("Project unique ID", new GuidLabel() { Text = Guid.NewGuid().ToString() });
			CompleteRows();
		}
	}
}