using App.AdventureMaker.Core.Controls;
using App.AdventureMaker.Core.Interfaces;
using Distance.AdventureMaker.Common.Enums;
using Distance.AdventureMaker.Common.Models;
using Eto.Forms;

namespace App.AdventureMaker.Core.Views
{
	public class OverviewPage : PropertiesListBase, ISaveLoad<CampaignFile>
	{
		public readonly TextBox titleBox;
		public readonly TextBox descriptionBox;
		public readonly TextBox authorBox;
		public readonly TextBox contactBox;
		public readonly EnumDropDown<DevelopmentStatus> devBuildBox;
		public readonly GuidLabel guidBox;

		public OverviewPage(IEditor<CampaignFile> editor)
		{
			AddRow("Campaign title", titleBox = new TextBox());
			AddRow("Description", descriptionBox = new TextBox());
			AddRow("Author", authorBox = new TextBox());
			AddRow("Contact", contactBox = new TextBox());
			//AddRow("Difficulty rating", new EnumDropDown<Difficulty>());
			//AddRow("Development build", devBuildBox = new CheckBox());
			AddRow("Development status", devBuildBox = new EnumDropDown<DevelopmentStatus>());
			AddRow("Project unique ID", guidBox = new GuidLabel() { Text = string.Empty });

			CompleteRows();

			titleBox.TextChanged += (_, __) => editor.Modified = true;
			descriptionBox.TextChanged += (_, __) => editor.Modified = true;
			authorBox.TextChanged += (_, __) => editor.Modified = true;
			contactBox.TextChanged += (_, __) => editor.Modified = true;
			devBuildBox.SelectedValueChanged += (_, __) => editor.Modified = true;
			guidBox.TextChanged += (_, __) => editor.Modified = true;
		}

		void ISaveLoad<CampaignFile>.SaveData(CampaignFile project)
		{
			project.Metadata.Title = titleBox.Text;
			project.Metadata.Description = descriptionBox.Text;
			project.Metadata.Author = authorBox.Text;
			project.Metadata.Contact = contactBox.Text;
			project.Metadata.DevelopmentStatus = devBuildBox.SelectedValue;
			project.Metadata.Guid = guidBox.Text;
		}

		void ISaveLoad<CampaignFile>.LoadData(CampaignFile project, bool resetUI)
		{
			titleBox.Text = project.Metadata.Title;
			descriptionBox.Text = project.Metadata.Description;
			authorBox.Text = project.Metadata.Author;
			contactBox.Text = project.Metadata.Contact;
			devBuildBox.SelectedValue = project.Metadata.DevelopmentStatus;
			guidBox.Text = project.Metadata.Guid;
		}
	}
}