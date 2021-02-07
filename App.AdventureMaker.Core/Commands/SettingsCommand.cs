using App.AdventureMaker.Core.Forms;
using Eto.Forms;
using System;

namespace App.AdventureMaker.Core.Commands
{
	public class SettingsCommand : Command
	{
		public SettingsCommand()
		{
			MenuText = "&Settings";
			ToolBarText = "Settings";
			Image = Resources.GetIcon("Settings.ico");
		}

		protected override void OnExecuted(EventArgs e)
		{
			new SettingsWindow().ShowModal();
		}
	}
}
