using System;

namespace App.CustomDeathMessages.Core.Forms
{
	public partial class MainForm
	{
		private void OnExecuteHelpAboutCommand(object o, EventArgs e)
		{
			using AboutDialog dialog = new AboutDialog();
			dialog.ShowModal(this);
		}
	}
}
