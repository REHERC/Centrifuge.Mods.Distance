using App.AdventureMaker.Core.Controls;
using App.AdventureMaker.Core.Views.Pages;

namespace App.AdventureMaker.Core.Views
{
	public class EditorTabView : ExtendedTabControl
	{
		public EditorTabView()
		{
			AddPage("Overview", new OverviewPage(), scrollable: true);
			AddPage("Level Sets", new LevelSetsPage(), scrollable: true);
			AddPage("Achievements", null, scrollable: true);
			AddPage("Editor Prefabs", null, scrollable: true);
			AddPage("Audio Engine", null, scrollable: true);
			AddPage("Resources", null, scrollable: true);
		}
	}
}
