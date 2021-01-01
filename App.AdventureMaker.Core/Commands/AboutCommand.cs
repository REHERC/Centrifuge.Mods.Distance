using App.AdventureMaker.Core.Forms;
using Eto.Forms;
using System;

namespace App.AdventureMaker.Core.Commands
{
	public class AboutCommand : Command
	{
		public AboutCommand()
		{
			MenuText = "&About";
			ToolBarText = "About";
		}

		protected override void OnExecuted(EventArgs e)
		{
			base.OnExecuted(e);

			using AboutWindow dialog = new AboutWindow();
			dialog.ShowModal();
		}
	}
}
