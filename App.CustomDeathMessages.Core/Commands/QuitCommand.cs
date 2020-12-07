#pragma warning disable IDE0052

using App.CustomDeathMessages.Core.Forms;
using Eto.Forms;
using System;

namespace App.CustomDeathMessages.Core.Commands
{
	public class QuitCommand : Command
	{
		readonly MainWindow form_;
		
		public QuitCommand(MainWindow form)
		{
			form_ = form;
			MenuText = "Quit";
			ToolBarText = "Quit";
			Image = Resources.GetIcon("Delete.ico");
			Shortcut = Application.Instance.CommonModifier | Keys.Q;
		}

		protected override void OnExecuted(EventArgs e)
		{
			base.OnExecuted(e);

			form_.Close();
		}
	}
}
