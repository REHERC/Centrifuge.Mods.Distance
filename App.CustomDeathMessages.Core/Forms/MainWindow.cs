using App.CustomDeathMessages.Core.Data;
using App.CustomDeathMessages.Core.Menus;
using App.CustomDeathMessages.Core.Views;
using Eto.Drawing;
using Eto.Forms;
using System;

namespace App.CustomDeathMessages.Core.Forms
{
	public partial class MainWindow : Form
	{
		public string FilePath { get; internal set; }
		
		public bool Modified { get; internal set; }

		public MainEditorView View { get; protected set; }

		public EditorData Data
		{
			get => View.Data;
			set => View.Data = value;
		}

		private void InitializeComponent()
		{
			Title = "Distance - Custom Death Messages Editor";
			ClientSize = new Size(640, 480);
			Icon = Resources.GetIcon("System.Windows.Forms.wfc.ico");

			Menu = new MainMenu(this);

			View = new MainEditorView();
			Content = View;

			View.Modified += OnDocumentModified;
		}

		public MainWindow()
		{
			InitializeComponent();
		}

		protected void OnDocumentModified(object sender, EventArgs e)
		{
			Modified = true;
		}
	}
}