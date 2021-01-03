using App.AdventureMaker.Core.Commands;
using Eto.Forms;

namespace App.AdventureMaker.Core.Menus
{
	public class MainMenu : MenuBar
	{
		public MainMenu()
		{
			ApplicationItems.Add(new NewFileCommand());
			ApplicationItems.Add(new OpenFileCommand());
			ApplicationItems.Add(new SaveFileCommand());
			ApplicationItems.Add(new SeparatorMenuItem());
			ApplicationItems.Add(new ButtonMenuItem()
			{
				Text = "&Import / Export",
				Image = Resources.GetIcon("Package.ico"),
				Items =
				{
					new ImportFileCommand(),
					new ExportFileCommand()
				}
			});
			ApplicationItems.Add(new SeparatorMenuItem());
			ApplicationItems.Add(new SettingsCommand());
			ApplicationItems.Add(new SeparatorMenuItem());
			ApplicationItems.Add(new QuitCommand());


			HelpItems.Add(new AboutCommand());


			Items.Add(new ButtonMenuItem()
			{
				Text = "&Project",
				Items =
				{
					new RunGameCommand(),
					new CheckFilesCommand(),
					new ProjectFolderCommand()
				}
			});
		}
	}
}
