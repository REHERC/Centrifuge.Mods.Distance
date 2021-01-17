using App.AdventureMaker.Core.Forms;
using App.AdventureMaker.Core.Interfaces;
using Distance.AdventureMaker.Common.Models;
using Distance.AdventureMaker.Common.Models.UI;
using Eto.Forms;
using System;
using System.IO;

namespace App.AdventureMaker.Core.Commands
{
	public class NewFileCommand : Command
	{
		private readonly IEditor<CampaignFile> editor;

		public NewFileCommand(IEditor<CampaignFile> editor_)
		{
			editor = editor_;

			MenuText = "&New";
			ToolBarText = "New";
			Image = Resources.GetIcon("New.ico");
			Shortcut = Application.Instance.CommonModifier | Keys.N;
		}

		protected override void OnExecuted(EventArgs e)
		{
			ProjectCreateData data = new NewProjectWindow().ShowModal();
			if (data != null)
			{
				Project.CreateProject(data);
				editor.LoadFile(Path.Combine(data.path, "project.json"));
			}
		}
	}
}
