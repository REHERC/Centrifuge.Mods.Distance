using Eto.Forms;
using System;
using static Utils;

namespace App.AdventureMaker.Core.Commands
{
	public class FeedbackCommand : Command
	{
		public FeedbackCommand()
		{
			MenuText = "&Bug reports and feedback";
			ToolBarText = "Bug reports and feedback";
			Image = Resources.GetIcon("Feedback.ico");
			Shortcut = Application.Instance.CommonModifier | Keys.F3;
		}

		protected override void OnExecuted(EventArgs e)
		{
			ShellOpen(Constants.GITHUB_ISSUES_PAGE);
		}
	}
}
