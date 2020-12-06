#pragma warning disable IDE0052

using App.CustomDeathMessages.Core.Forms;
using Eto.Forms;
using System;

namespace App.CustomDeathMessages.Core.Commands
{
	public class SettingsCommand : Command
	{
		readonly MainWindow form_;
		
		public SettingsCommand(MainWindow form)
		{
			form_ = form;
			MenuText = "Settings";
			ToolBarText = "Settings";
			Image = Resources.GetIcon("System.ComponentModel.Design.DefaultComponent.ico");
		}

		protected override void OnExecuted(EventArgs e)
		{
			base.OnExecuted(e);
		}
	}
}
