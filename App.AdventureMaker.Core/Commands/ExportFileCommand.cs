using Eto.Forms;

namespace App.AdventureMaker.Core.Commands
{
	public class ExportFileCommand : Command
	{
		public ExportFileCommand()
		{
			MenuText = "&Export";
			ToolBarText = "Export";
			Shortcut = Application.Instance.CommonModifier | Keys.Shift | Keys.E;

			Enabled = false;
		}
	}
}
