using Eto.Forms;

namespace App.AdventureMaker.Core.Commands
{
	public class CheckFilesCommand : Command
	{
		public CheckFilesCommand()
		{
			MenuText = "&Check Files";
			ToolBarText = "Check Files";
			Image = Resources.GetIcon("FolderCheck.ico");
			Shortcut = Application.Instance.CommonModifier | Keys.F6;
		}
	}
}
