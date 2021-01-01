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
			ApplicationItems.Add(new SaveAsFileCommand());
			ApplicationItems.Add(new SeparatorMenuItem());
			ApplicationItems.Add(new RunGameCommand());
			ApplicationItems.Add(new ButtonMenuItem()
			{
				Text = "Packaging",
				Image = Resources.GetIcon("Package.ico"),
				Items =
				{
					new ImportFileCommand(),
					new ExportFileCommand()
				}
			});
			ApplicationItems.Add(new SettingsCommand());
			ApplicationItems.Add(new SeparatorMenuItem());
			ApplicationItems.Add(new QuitCommand());

			HelpItems.Add(new AboutCommand());
		}
	}
}
