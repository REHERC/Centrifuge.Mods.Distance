using App.AdventureMaker.Core.Interfaces;
using Distance.AdventureMaker.Common.Models;
using Eto.Forms;
using System;
using System.Diagnostics;

namespace App.AdventureMaker.Core.Commands
{
	public class ProjectFolderCommand : Command
	{
		private readonly IEditor<CampaignFile> editor;

		public ProjectFolderCommand(IEditor<CampaignFile> editor_)
		{
			editor = editor_;

			MenuText = "Open Project &Folder";
			ToolBarText = "Open Project Folder";
			Image = Resources.GetIcon("FolderOpened.ico");
			Shortcut = Keys.F10;

			Enabled = false;

			editor.OnLoaded += (_) =>
			{
				Enabled = editor.CurrentFile != null;
			};
		}

		protected override void OnExecuted(EventArgs e)
		{
			if (editor.CurrentFile != null)
			{
				new Process()
				{
					StartInfo = new ProcessStartInfo(editor.CurrentFile.Directory.FullName)
					{
						UseShellExecute = true
					}
				}.Start();
			}
		}
	}
}
