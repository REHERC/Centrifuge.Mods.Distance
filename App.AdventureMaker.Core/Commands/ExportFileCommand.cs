using App.AdventureMaker.Core.Interfaces;
using Distance.AdventureMaker.Common.Models;
using Eto.Forms;
using System;
using static Dialogs;

namespace App.AdventureMaker.Core.Commands
{
	public class ExportFileCommand : Command
	{
		private readonly IEditor<CampaignFile> editor;
		private readonly SaveFileDialog dialog;

		public ExportFileCommand(IEditor<CampaignFile> editor)
		{
			this.editor = editor;

			MenuText = "&Export";
			ToolBarText = "Export";
			Shortcut = Application.Instance.CommonModifier | Keys.Shift | Keys.E;

			dialog = ExportCampaignDialog("Export campaign as...");

			Enabled = false;
			editor.OnLoaded += (_) => Enabled = editor.CurrentFile != null;
		}

		protected override void OnExecuted(EventArgs e)
		{
			if (dialog.ShowDialog(null) == DialogResult.Ok)
			{
				MessageBox.Show("Unimplemented :(");
			}
		}
	}
}
