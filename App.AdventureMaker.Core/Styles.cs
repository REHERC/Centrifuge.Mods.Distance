using Eto;
using Eto.Drawing;
using Eto.Forms;

namespace App.AdventureMaker.Core
{
	public static class Styles
	{
		public static void ApplyAll()
		{
			Style.Add<Panel>(null, item => item.Padding = new Padding(2));
			Style.Add<TabPage>(null, item => item.Padding = new Padding(4));
			Style.Add<TableLayout>(null, item => item.Spacing = new Size(4, 8));
			Style.Add<Scrollable>(null, item => item.Border = BorderType.None);

			Style.Add<Button>("icon", item => 
			{
				Size size = new Size(32, 32);
				item.Size = size;
				item.Width = size.Width;
			});

			Style.Add<Panel>("no-padding", item => item.Padding = Padding.Empty);

			Style.Add<StackLayout>("vertical", item =>
			{
				item.Orientation = Orientation.Vertical;
				item.HorizontalContentAlignment = HorizontalAlignment.Stretch;
			});

			Style.Add<StackLayout>("horizontal", item =>
			{
				item.Orientation = Orientation.Horizontal;
				item.VerticalContentAlignment = VerticalAlignment.Stretch;
			});
		}
	}
}
