using App.AdventureMaker.Core.Interfaces;
using Distance.AdventureMaker.Common.Models;
using Eto.Forms;
using System;

namespace App.AdventureMaker.Core.Commands
{
	public class RunGameCommand : Command
	{
		private readonly IEditor<CampaignFile> editor;
		public RunGameCommand(IEditor<CampaignFile> editor_)
		{
			editor = editor_;

			MenuText = "&Run Preview";
			ToolBarText = "Run Preview";
			Image = Resources.GetIcon("Run.ico");
			Shortcut = Application.Instance.CommonModifier | Keys.F5;

			Enabled = false;

			editor.OnLoaded += (_) => Enabled = editor.CurrentFile != null;
		}

		protected override void OnExecuted(EventArgs e)
		{
			base.OnExecuted(e);

			if (editor.Modified && Messages.SaveBeforeContinue() == DialogResult.No)
			{
				return;
			}

			editor.SaveFile();
			RunGame.Run(editor);
		}
	}
}
