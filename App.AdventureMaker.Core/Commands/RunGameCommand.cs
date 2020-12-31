using Eto.Forms;

namespace App.CustomDeathMessages.Core.Commands
{
	public class RunGameCommand : Command
	{
		public RunGameCommand()
		{
			MenuText = "Quit";
			ToolBarText = "Quit";
			Image = Resources.GetIcon("Delete.ico");
			Shortcut = Application.Instance.CommonModifier | Keys.Q;
		}
	}
}
