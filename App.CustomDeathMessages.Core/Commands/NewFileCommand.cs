#pragma warning disable IDE0052

using App.CustomDeathMessages.Core.Forms;
using Eto.Forms;
using System;

namespace App.CustomDeathMessages.Core.Commands
{
	public class NewFileCommand : Command
	{
		readonly MainWindow form_;
		
		public NewFileCommand(MainWindow form)
		{
			form_ = form;
			MenuText = "&New";
			ToolBarText = "New";
			Image = Resources.GetIcon("New.ico");
			Shortcut = Application.Instance.CommonModifier | Keys.N;
		}

		protected override void OnExecuted(EventArgs e)
		{
			base.OnExecuted(e);

			if (form_.Modified && form_.ShowUnsavedDialog(title: "Create new file") == DialogResult.No)
			{
				return;
			}

			form_.NewFile();
		}
	}
}
