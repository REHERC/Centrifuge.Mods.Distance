using Eto.Forms;

namespace App.AdventureMaker.Core.Commands
{
	public class ExportFileCommand : Command
	{
		public ExportFileCommand()
		{
			MenuText = "&Export";
			ToolBarText = "Export";
			Image = Resources.GetIcon("ZipFile.ico");
		}
	}
}
