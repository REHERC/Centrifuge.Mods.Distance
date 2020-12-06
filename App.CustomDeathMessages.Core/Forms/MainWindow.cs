using App.CustomDeathMessages.Core.Menus;
using App.CustomDeathMessages.Core.Views;
using Eto.Drawing;
using Eto.Forms;

namespace App.CustomDeathMessages.Core.Forms
{
	public partial class MainWindow : Form
	{
		private void InitializeComponent()
		{
			Title = "Distance - Custom Death Messages Editor";
			ClientSize = new Size(640, 480);
			Icon = Resources.GetIcon("System.Windows.Forms.wfc.ico");

			Menu = new MainMenu(this);
			Content = new MainEditorView();
		}

		public MainWindow()
		{
			InitializeComponent();
		}
	}
}