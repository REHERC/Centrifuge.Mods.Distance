using Eto.Forms;
using System;
using System.Diagnostics;

namespace App.AdventureMaker.Core.Commands
{
	public class FeedbackCommand : Command
	{
		public FeedbackCommand()
		{
			MenuText = "&Bug reports and feedback";
			ToolBarText = "Bug reports and feedback";
			Image = Resources.GetIcon("Feedback.ico");
			Shortcut = Keys.F3;
		}

		protected override void OnExecuted(EventArgs e)
		{
			new Process()
			{
				StartInfo = new ProcessStartInfo(Constants.GITHUB_ISSUES_PAGE)
				{
					UseShellExecute = true
				}
			}.Start();
		}
	}
}
