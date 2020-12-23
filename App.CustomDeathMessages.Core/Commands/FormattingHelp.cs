#pragma warning disable IDE0052

using App.CustomDeathMessages.Core.Forms;
using Eto.Forms;
using System;

namespace App.CustomDeathMessages.Core.Commands
{
	public class FormattingHelp : Command
	{
		private readonly MainWindow form_;

		public FormattingHelp(MainWindow form)
		{
			form_ = form;
			MenuText = "&Formatting";
			ToolBarText = "Formatting";
			Image = Resources.GetIcon("Help.ico");
			Shortcut = Keys.F1;
		}

		protected override void OnExecuted(EventArgs e)
		{
			base.OnExecuted(e);

			using var dialog = new FormattingHelpWindow();
			dialog.ShowModal(form_);
		}
	}
}
