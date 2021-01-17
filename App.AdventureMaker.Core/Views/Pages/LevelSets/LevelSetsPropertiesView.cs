using App.AdventureMaker.Core.Controls;
using Distance.AdventureMaker.Common.Enums;
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
			properties.AddRow("Playlist locking", new EnumDropDown<PlaylistUnlock>());
			properties.AddRow("Individual level locking", new EnumDropDown<LevelUnlock>());
			properties.AddRow("Difficulty rating", new EnumDropDown<Difficulty>());

			properties.CompleteRows();

			Items.Add(new StackLayoutItem(properties) { Expand = false });
		}
	}
}
