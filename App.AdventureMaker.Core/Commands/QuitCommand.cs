using Eto.Forms;

namespace App.CustomDeathMessages.Core.Commands
{
	public class RunGameCommand : Command
	{
		public RunGameCommand()
		{
			MenuText = "&Run";
			ToolBarText = "Run";
			Image = Resources.GetIcon("Run.ico");
			Shortcut = Application.Instance.CommonModifier | Keys.F5;
		}
	}
}
