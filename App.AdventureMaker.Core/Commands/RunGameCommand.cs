using Eto.Forms;

namespace App.AdventureMaker.Core.Commands
{
	public class RunGameCommand : Command
	{
		public RunGameCommand()
		{
			MenuText = "&Run Preview";
			ToolBarText = "Run Preview";
			Image = Resources.GetIcon("Run.ico");
			Shortcut = Application.Instance.CommonModifier | Keys.F5;

			Enabled = false;
		}
	}
}
