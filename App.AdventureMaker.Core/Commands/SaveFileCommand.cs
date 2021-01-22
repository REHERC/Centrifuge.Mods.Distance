using App.AdventureMaker.Core.Interfaces;
using Distance.AdventureMaker.Common.Models;
using Eto.Forms;
using System;

namespace App.AdventureMaker.Core.Commands
{
	public class SaveFileCommand : Command
	{
		private readonly IEditor<CampaignFile> editor;

		public SaveFileCommand(IEditor<CampaignFile> editor_)
		{
			editor = editor_;

			MenuText = "&Save";
			ToolBarText = "Save";
			Image = Resources.GetIcon("Save.ico");
			Shortcut = Application.Instance.CommonModifier | Keys.S;

			Enabled = false;

			editor.OnLoaded += (_) =>
			{
				Enabled = editor.CurrentFile != null;
			};
		}

		protected override void OnExecuted(EventArgs e)
		{
			editor.SaveFile();
		}
	}
}
