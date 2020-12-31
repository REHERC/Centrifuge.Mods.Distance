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
		}

		public MainWindow()
		{
			InitializeComponent();
		}
	}
}
