using App.AdventureMaker.Core.Menus;
using App.AdventureMaker.Core.Views;
using Eto.Drawing;
using Eto.Forms;
using System;
using System.ComponentModel;

namespace App.AdventureMaker.Core.Forms
{
	public class MainWindow : Form
	{
		private const string Name = "Distance - Campaign Editor";

		private MainView mainView;

		private void InitializeComponent()
		{
			ClientSize = new Size(720, 540);
			MinimumSize = ClientSize;

			Icon = Resources.GetIcon("App.ico");
			
			Content = mainView = new MainView();
			ToolBar = new MainToolbar(mainView);
			Menu = new MainMenu(this, mainView);

			mainView.OnModified += (editor) =>
			{
				UpdateTitle();
			};

			//WindowState = WindowState.Maximized;

			UpdateTitle();
		}

		private void UpdateTitle()
		{

			if (mainView.CurrentFile != null)
			{
				Title = $"{Name} - [{mainView.CurrentFile.FullName}{(mainView.Modified ? "*" : string.Empty)}]";
			}
			else
			{
				Title = Name;
			}
		}

		public MainWindow()
		{
			InitializeComponent();
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			if (AppSettings.Instance.OpenLastProject && RecentProjects.Get().Length > 0)
			{
				mainView.LoadFile(RecentProjects.Get()[0]);
			}

			Messages.PreviewMessage();
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			if (mainView.Modified && Messages.UnsavedChangesDialog(Constants.DIALOG_CAPTION_APP_CLOSE) == DialogResult.No)
			{
				e.Cancel = true;
			}
		}
	}
}
