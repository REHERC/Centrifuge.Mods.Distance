using App.AdventureMaker.Core.Menus;
using App.AdventureMaker.Core.Views;
using Eto.Drawing;
using Eto.Forms;

namespace App.AdventureMaker.Core.Forms
{
	public class MainWindow : Form
	{
		private const string Name = "Distance - Campaign Editor";

		private MainView mainView;

		private void InitializeComponent()
		{
			ClientSize = new Size(640, 480);
			Icon = Resources.GetIcon("App.ico");
			Title = Name;

			Content = mainView = new MainView();
			ToolBar = new MainToolbar(mainView);
			Menu = new MainMenu(this, mainView);
		}

		public MainWindow()
		{
			InitializeComponent();
		}
	}
}
