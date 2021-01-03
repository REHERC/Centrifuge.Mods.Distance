using Eto.Forms;

namespace App.AdventureMaker.Core.Commands
{
	public class ProjectFolderCommand : Command
	{
		public ProjectFolderCommand()
		{
			MenuText = "Open Project &Folder";
			ToolBarText = "Open Project Folder";
			Image = Resources.GetIcon("FolderOpened.ico");
			Shortcut = Keys.F10;
		}
	}
}
