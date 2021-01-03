using Eto.Forms;

namespace App.AdventureMaker.Core.Commands
{
	public class ImportFileCommand : Command
	{
		public ImportFileCommand()
		{
			MenuText = "&Import";
			ToolBarText = "Import";
			Shortcut = Application.Instance.CommonModifier | Keys.Shift | Keys.I;
		}
	}
}
