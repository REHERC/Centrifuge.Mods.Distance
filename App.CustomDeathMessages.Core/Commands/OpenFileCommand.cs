#pragma warning disable IDE0052

using App.CustomDeathMessages.Core.Forms;
using Eto.Forms;
using System;

namespace App.CustomDeathMessages.Core.Commands
{
	public class OpenFileCommand : Command
	{
		readonly MainWindow form_;
		
		public OpenFileCommand(MainWindow form)
		{
			form_ = form;
			MenuText = "&Open";
			ToolBarText = "Open";
			Image = Resources.GetIcon("Open.ico");
			Shortcut = Application.Instance.CommonModifier | Keys.O;
		}

		protected override void OnExecuted(EventArgs e)
		{
			base.OnExecuted(e);

			form_.OpenFile();
		}
	}
}
