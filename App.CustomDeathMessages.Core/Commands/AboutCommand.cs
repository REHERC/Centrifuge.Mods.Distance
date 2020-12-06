using App.CustomDeathMessages.Core.Forms;
using Eto.Forms;
using System;

namespace App.CustomDeathMessages.Core.Commands
{
	public class AboutCommand : Command
	{
		readonly MainWindow form_;
		
		public AboutCommand(MainWindow form)
		{
			form_ = form;
			MenuText = "&About";
			ToolBarText = "About";
			Image = Resources.GetIcon("System.Windows.Forms.help.ico");
		}

		protected override void OnExecuted(EventArgs e)
		{
			base.OnExecuted(e);

			using var dialog = new Forms.AboutWindow();
			dialog.ShowModal(form_);
		}
	}
}
