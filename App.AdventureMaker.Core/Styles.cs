using Eto;
using Eto.Drawing;
using Eto.Forms;

namespace App.AdventureMaker.Core
{
	public static class Styles
	{
		public static void ApplyAll()
		{
			Style.Add<Panel>(null, panel => panel.Padding = new Padding(2));
			Style.Add<TabPage>(null, page => page.Padding = new Padding(4));
			Style.Add<TableLayout>(null, layout => layout.Spacing = new Size(4, 8));
		}
	}
}
