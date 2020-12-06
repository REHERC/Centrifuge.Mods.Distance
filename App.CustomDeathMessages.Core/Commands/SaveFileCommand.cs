using App.CustomDeathMessages.Core.Forms;
using Eto.Forms;
using System;

namespace App.CustomDeathMessages.Core.Commands
{
	public class SaveFileCommand : Command
	{
		readonly MainWindow form_;
		
		public SaveFileCommand(MainWindow form)
		{
			form_ = form;
			MenuText = "&Save";
			ToolBarText = "Save";
			Image = Resources.GetIcon("System.Windows.Forms.save.ico");
			Shortcut = Application.Instance.CommonModifier | Keys.S;
		}

		protected override void OnExecuted(EventArgs e)
		{
			base.OnExecuted(e);
		}
	}
}
