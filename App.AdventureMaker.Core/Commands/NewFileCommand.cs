using Eto.Forms;

namespace App.AdventureMaker.Core.Commands
{
	public class NewFileCommand : Command
	{
		public NewFileCommand()
		{
			MenuText = "&New";
			ToolBarText = "New";
			Image = Resources.GetIcon("New.ico");
			Shortcut = Application.Instance.CommonModifier | Keys.N;
		}
	}
}
