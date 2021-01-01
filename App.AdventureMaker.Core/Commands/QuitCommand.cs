using Eto.Forms;

namespace App.AdventureMaker.Core.Commands
{
	public class QuitCommand : Command
	{
		public QuitCommand()
		{
			MenuText = "&Quit";
			ToolBarText = "Quit";
			Image = Resources.GetIcon("Delete.ico");
			Shortcut = Application.Instance.CommonModifier | Keys.Q;
		}
	}
}
