using App.AdventureMaker.Core.Controls;
using Eto.Drawing;
using Eto.Forms;

namespace App.AdventureMaker.Core.Views
{
	public class LevelSetsPropertiesView : StackLayout
	{
		private readonly PropertiesListBase properties;

		public LevelSetsPropertiesView()
		{
			//Style = "no-padding";
			Orientation = Orientation.Vertical;
			HorizontalContentAlignment = HorizontalAlignment.Stretch;

			properties = new PropertiesListBase();

			properties.AddRow("Name", new TextBox());
			properties.AddRow("Description", new TextBox());
			properties.AddRow("Icon", new TextBoxWithButton());

			properties.CompleteRows(new Size(8, 8));

			Items.Add(new StackLayoutItem(properties) { Expand = false });
		}
	}
}
