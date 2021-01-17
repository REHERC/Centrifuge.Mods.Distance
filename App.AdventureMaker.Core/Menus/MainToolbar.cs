using App.AdventureMaker.Core.Commands;
using App.AdventureMaker.Core.Interfaces;
using Distance.AdventureMaker.Common.Models;
using Eto.Forms;

namespace App.AdventureMaker.Core.Menus
{
	public class MainToolbar : ToolBar
	{
		public MainToolbar(IEditor<CampaignFile> editor)
		{
			Items.Add(new NewFileCommand(editor));
			Items.Add(new OpenFileCommand(editor));
			Items.Add(new SaveFileCommand(editor));
			Items.Add(new SeparatorToolItem());
			Items.Add(new RunGameCommand());
		}
	}
}
