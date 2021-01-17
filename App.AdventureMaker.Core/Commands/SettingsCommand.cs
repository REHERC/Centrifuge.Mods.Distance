using Eto.Forms;

namespace App.AdventureMaker.Core.Commands
{
	public class SettingsCommand : Command
	{
		public SettingsCommand()
		{
			MenuText = "&Settings";
			ToolBarText = "Settings";
			Image = Resources.GetIcon("Settings.ico");

			Enabled = false;
		}
	}
}
