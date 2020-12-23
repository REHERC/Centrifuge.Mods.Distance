using App.CustomDeathMessages.Core.Commands;
using App.CustomDeathMessages.Core.Forms;
using Eto.Forms;

namespace App.CustomDeathMessages.Core.Menus
{
	public class MainToolbar : ToolBar
	{
		private readonly MainWindow form_;

		public MainToolbar(MainWindow form)
		{
			form_ = form;

			Items.Add(new NewFileCommand(form_));
			Items.Add(new OpenFileCommand(form_));
			Items.Add(new SaveFileCommand(form_));
			Items.Add(new SaveAsFileCommand(form_));
		}
	}
}
