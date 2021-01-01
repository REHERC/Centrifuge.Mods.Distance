using Eto.Forms;

namespace App.AdventureMaker.Core.Views
{
	public class MainView : Panel
	{
		public MainView()
		{
			Content = new EditorTabView();
		}
	}
}
