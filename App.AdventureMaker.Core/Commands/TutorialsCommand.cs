using App.AdventureMaker.Core.Interfaces;
using Distance.AdventureMaker.Common.Models;
using Eto.Forms;
using System;
using System.Diagnostics;

namespace App.AdventureMaker.Core.Commands
{
	public class TutorialsCommand : Command
	{
		private readonly IEditor<CampaignFile> editor;

		public TutorialsCommand()
		{
			MenuText = "&Online wiki";
			ToolBarText = "Online wiki";
			Image = Resources.GetIcon("Web.ico");
			Shortcut = Keys.F4;

			Enabled = false;
		}

		protected override void OnExecuted(EventArgs e)
		{
			new Process()
			{
				StartInfo = new ProcessStartInfo(Constants.GITHUB_WIKI)
				{
					UseShellExecute = true
				}
			}.Start();
		}
	}
}
