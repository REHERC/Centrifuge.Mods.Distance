using App.AdventureMaker.Core.Controls;
using App.AdventureMaker.Core.Interfaces;
//using Distance.AdventureMaker.Common.Enums;
using Distance.AdventureMaker.Common.Models;
using Eto.Forms;
using System;

namespace App.AdventureMaker.Core.Views
{
	public class OverviewPage : PropertiesListBase, ISaveLoad<CampaignFile>
	{
		public string Value { get; set; }

		public readonly TextBox titleBox;
		public readonly TextBox descriptionBox;
		public readonly TextBox authorBox;
		public readonly TextBox contactBox;
		public readonly CheckBox devBuildBox;
		public readonly GuidLabel guidBox;

		public OverviewPage()
		{
			AddRow("Campaign title", titleBox = new TextBox());
			AddRow("Description", descriptionBox = new TextBox());
			AddRow("Author", authorBox = new TextBox());
			AddRow("Contact", contactBox = new TextBox());
			//AddRow("Difficulty rating", new EnumDropDown<Difficulty>());
			AddRow("Development build", devBuildBox = new CheckBox());
			AddRow("Project unique ID", guidBox = new GuidLabel() { Text = string.Empty });
			
			CompleteRows();
		}

		void ISaveLoad<CampaignFile>.SaveData(CampaignFile project)
		{
			project.metadata.title = titleBox.Text;
			project.metadata.description = descriptionBox.Text;
			project.metadata.author = authorBox.Text;
			project.metadata.contact = contactBox.Text;
			project.metadata.dev_build = (bool)devBuildBox.Checked;
			project.metadata.guid = guidBox.Text;
		}

		void ISaveLoad<CampaignFile>.LoadData(CampaignFile project)
		{
			titleBox.Text = project.metadata.title;
			descriptionBox.Text = project.metadata.description;
			authorBox.Text = project.metadata.author;
			contactBox.Text = project.metadata.contact;
			devBuildBox.Checked = project.metadata.dev_build;
			guidBox.Text = project.metadata.guid;
		}
	}
}