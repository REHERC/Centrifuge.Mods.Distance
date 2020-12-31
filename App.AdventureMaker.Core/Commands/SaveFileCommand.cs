using Eto.Forms;

namespace App.AdventureMaker.Core.Commands
{
	public class SaveFileCommand : Command
	{
		public SaveFileCommand()
		{
			MenuText = "&Save";
			ToolBarText = "Save";
			Image = Resources.GetIcon("Save.ico");
			Shortcut = Application.Instance.CommonModifier | Keys.S;
		}
	}
}
