using Eto.Forms;

namespace App.AdventureMaker.Core.Commands
{
	public class SaveAsFileCommand : Command
	{
		public SaveAsFileCommand()
		{
			MenuText = "Save &As";
			ToolBarText = "Save As";
			Image = Resources.GetIcon("SaveAs.ico");
			Shortcut = Application.Instance.CommonModifier | Keys.Shift | Keys.S;
		}
	}
}
