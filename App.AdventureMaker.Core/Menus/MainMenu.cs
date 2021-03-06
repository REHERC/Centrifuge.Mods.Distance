﻿using App.AdventureMaker.Core.Commands;
using App.AdventureMaker.Core.Forms;
using App.AdventureMaker.Core.Interfaces;
using Distance.AdventureMaker.Common.Models;
using Eto.Forms;

namespace App.AdventureMaker.Core.Menus
{
	public class MainMenu : MenuBar
	{
		public MainMenu(MainWindow form, IEditor<CampaignFile> editor)
		{
			ApplicationItems.Add(new NewFileCommand(editor));
			ApplicationItems.Add(new OpenFileCommand(editor));
			ApplicationItems.Add(new SaveFileCommand(editor));
			//ApplicationItems.Add(new SeparatorMenuItem());
			ApplicationItems.Add(new ButtonMenuItem()
			{
				Text = "&Import / Export",
				Image = Resources.GetIcon("Package.ico"),
				Items =
				{
					new ImportFileCommand(editor, form),
					new ExportFileCommand(editor, form)
				}
			});
			ApplicationItems.Add(new SeparatorMenuItem());
			ApplicationItems.Add(new CloseProjectCommand(editor));
			ApplicationItems.Add(new SeparatorMenuItem());
			ApplicationItems.Add(new SettingsCommand());
			//ApplicationItems.Add(new SeparatorMenuItem());
			ApplicationItems.Add(new QuitCommand(form));

			HelpItems.Add(new FeedbackCommand());
			HelpItems.Add(new TutorialsCommand());
			HelpItems.Add(new SeparatorMenuItem());
			HelpItems.Add(new AboutCommand());

			Items.Add(new ButtonMenuItem()
			{
				Text = "&Project",
				Items =
				{
					new RunGameCommand(editor),
					new CheckFilesCommand(editor),
					new ProjectFolderCommand(editor)
				}
			});
		}
	}
}
