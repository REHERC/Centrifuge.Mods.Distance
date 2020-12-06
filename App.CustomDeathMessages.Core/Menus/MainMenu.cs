using App.CustomDeathMessages.Core.Commands;
using App.CustomDeathMessages.Core.Forms;
using Eto.Forms;

namespace App.CustomDeathMessages.Core.Menus
{
	public class MainMenu : MenuBar
	{
		readonly MainWindow form_;

		public MainMenu(MainWindow form)
		{
			form_ = form;

			ApplicationItems.Add(new NewFileCommand(form_));
			ApplicationItems.Add(new OpenFileCommand(form_));
			ApplicationItems.Add(new SaveFileCommand(form_));
			ApplicationItems.Add(new SaveAsFileCommand(form_));
			ApplicationItems.Add(new SeparatorMenuItem());
			ApplicationItems.Add(new QuitCommand(form_));


			HelpItems.Add(new FormattingHelp(form_));
			HelpItems.Add(new SeparatorMenuItem());
			HelpItems.Add(new AboutCommand(form_));
		}
	}
}
