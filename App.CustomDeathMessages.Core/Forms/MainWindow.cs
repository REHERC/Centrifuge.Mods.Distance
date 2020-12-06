using App.CustomDeathMessages.Core.Menus;
using App.CustomDeathMessages.Core.Views;
using Eto.Drawing;
using Eto.Forms;
using System;

namespace App.CustomDeathMessages.Core.Forms
{
	public partial class MainWindow : Form
	{
		private void InitializeComponent()
		{
			Title = "Distance - Custom Death Messages Editor";
			ClientSize = new Size(640, 480);

			Menu = new MainMenu(this);
			Content = new MainEditorView();
		}

		public MainWindow()
		{
			InitializeComponent();
		}
	}
}