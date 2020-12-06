using App.CustomDeathMessages.Core.Forms;
using Eto.Forms;
using System;

namespace App.CustomDeathMessages.Core.Commands
{
	public class SaveAsFileCommand : Command
	{
		readonly MainWindow form_;
		
		public SaveAsFileCommand(MainWindow form)
		{
			form_ = form;
			MenuText = "Save &As";
			ToolBarText = "Save As";
			Image = Resources.GetIcon("System.Windows.Forms.save.ico");
			Shortcut = Application.Instance.CommonModifier | Keys.Shift | Keys.S;
		}

		protected override void OnExecuted(EventArgs e)
		{
			base.OnExecuted(e);
		}
	}
}
