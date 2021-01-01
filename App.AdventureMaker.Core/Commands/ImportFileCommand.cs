using Eto.Forms;

namespace App.AdventureMaker.Core.Commands
{
	public class ImportFileCommand : Command
	{
		public ImportFileCommand()
		{
			MenuText = "&Import";
			ToolBarText = "Import";
			Image = Resources.GetIcon("LinkedFolder.ico");
		}
	}
}
