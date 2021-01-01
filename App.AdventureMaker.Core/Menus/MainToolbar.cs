using App.AdventureMaker.Core.Commands;
using Eto.Forms;

namespace App.AdventureMaker.Core.Menus
{
	public class MainToolbar : ToolBar
	{
		public MainToolbar()
		{
			Items.Add(new NewFileCommand());
			Items.Add(new OpenFileCommand());
			Items.Add(new SaveFileCommand());
			Items.Add(new SaveAsFileCommand());
			Items.Add(new SeparatorToolItem());
			Items.Add(new RunGameCommand());
		}
	}
}
