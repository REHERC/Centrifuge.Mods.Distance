using Eto.Forms;
using System;
using static Utils;

namespace App.AdventureMaker.Core.Commands
{
	public class TutorialsCommand : Command
	{
		public TutorialsCommand()
		{
			MenuText = "&Online wiki";
			ToolBarText = "Online wiki";
			Image = Resources.GetIcon("Web.ico");
			Shortcut = Application.Instance.CommonModifier | Keys.F4;

			Enabled = false;
		}

		protected override void OnExecuted(EventArgs e)
		{
			ShellOpen(Constants.GITHUB_WIKI);
		}
	}
}
