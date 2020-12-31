using Eto.Forms;

namespace App.AdventureMaker.Core.Commands
{
	public class OpenFileCommand : Command
	{
		public OpenFileCommand()
		{
			MenuText = "&Open";
			ToolBarText = "Open";
			Image = Resources.GetIcon("Open.ico");
			Shortcut = Application.Instance.CommonModifier | Keys.O;
		}
	}
}
