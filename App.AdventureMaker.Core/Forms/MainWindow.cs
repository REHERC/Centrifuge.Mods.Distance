using App.AdventureMaker.Core.Menus;
using App.AdventureMaker.Core.Views;
using Eto.Drawing;
using Eto.Forms;

namespace App.AdventureMaker.Core.Forms
{
	public class MainWindow : Form
	{
		private const string Name = "Distance - Campaign Editor";

		private void InitializeComponent()
		{
			ClientSize = new Size(640, 480);
			Icon = Resources.GetIcon("App.ico");
			Title = Name;

			ToolBar = new MainToolbar();
			Menu = new MainMenu(this);

			Content = new MainView();
		}

		public MainWindow()
		{
			InitializeComponent();
		}
	}
}
