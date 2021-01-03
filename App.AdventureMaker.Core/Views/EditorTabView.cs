using App.AdventureMaker.Core.Views.Pages;
using Eto.Forms;

namespace App.AdventureMaker.Core.Views
{
	public class EditorTabView : TabControl
	{
		public EditorTabView()
		{
			AddPage("Overview", new OverviewPage());
		}


		protected int AddPage(string title, Control content)
		{
			Pages.Add(new TabPage()
			{
				Text = title,
				Content = content
			});

			return Pages.Count - 1;
		}
	}
}
