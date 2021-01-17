using App.AdventureMaker.Core.Interfaces;
using Distance.AdventureMaker.Common.Models;
using Eto.Forms;
using System;

namespace App.AdventureMaker.Core.Commands
{
	public class OpenFileCommand : Command
	{
		private readonly IEditor<CampaignFile> editor;

		private readonly OpenFileDialog dialog;

		public OpenFileCommand(IEditor<CampaignFile> editor_)
		{
			editor = editor_;

			MenuText = "&Open";
			ToolBarText = "Open";
			Image = Resources.GetIcon("Open.ico");
			Shortcut = Application.Instance.CommonModifier | Keys.O;

			dialog = new OpenFileDialog()
			{
				Title = "Open project",
				MultiSelect = false,
				CheckFileExists = true
			};

			dialog.Filters.Add(Constants.DIALOG_FILTER_PROJECT);
			dialog.Filters.Add(Constants.DIALOG_FILTER_ANY);
		}

		protected override void OnExecuted(EventArgs e)
		{
			dialog.CurrentFilterIndex = 0;

			if (dialog.ShowDialog(null) == DialogResult.Ok)
			{
				editor.LoadFile(dialog.FileName);
			}
		}
	}
}
