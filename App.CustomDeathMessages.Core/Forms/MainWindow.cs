using App.CustomDeathMessages.Core.Data;
using App.CustomDeathMessages.Core.Menus;
using App.CustomDeathMessages.Core.Views;
using Eto.Drawing;
using Eto.Forms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace App.CustomDeathMessages.Core.Forms
{
	public partial class MainWindow : Form
	{
		private const string Name = "Distance - Custom Death Messages Editor";

		private string filePath_ = string.Empty;
		public string FilePath 
		{ 
			get
			{
				return filePath_;
			}
			internal set
			{
				filePath_ = value;
				UpdateTitle();
			}
		}

		private bool modified_ = false;
		public bool Modified 
		{ 
			get
			{
				return modified_;
			}
			internal set
			{
				modified_ = value;
				UpdateTitle();
			}
		}

		public MainEditorView View { get; protected set; }

		public EditorData Data
		{
			get => View.Data;
			set => View.Data = value;
		}

		private void InitializeComponent()
		{
			ClientSize = new Size(640, 480);
			Icon = Resources.GetIcon("App.ico");

			Menu = new MainMenu(this);
			ToolBar = new MainToolbar(this);

			View = new MainEditorView();
			Content = View;

			View.Modified += OnDocumentModified;

			UpdateTitle();
		}

		public MainWindow()
		{
			InitializeComponent();
		}

		public void UpdateTitle()
		{
			string file = string.IsNullOrWhiteSpace(FilePath) ? "Untitled" : new FileInfo(FilePath).Name;
			string unsaved = Modified ? "*" : "";

			Title = $"{Name} - [{file}{unsaved}]";
		}

		protected void OnDocumentModified(object sender, EventArgs e)
		{
			Modified = true;
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);

			if (Modified && ShowUnsavedDialog(title: "Close application") == DialogResult.No)
			{
				e.Cancel = true;
			}
		}

		public void NewFile()
		{
			Data = new EditorData();
			FilePath = string.Empty;
			ResetView();
			Modified = false;
		}

		public void OpenFile()
		{
			using OpenFileDialog dialog = new OpenFileDialog
			{
				Title = "Open",
				MultiSelect = false,
				CheckFileExists = true
			};

			dialog.Filters.Add(Constants.DIALOG_FILTER_JSON);
			dialog.Filters.Add(Constants.DIALOG_FILTER_ANY);
			dialog.CurrentFilterIndex = 0;

			if (dialog.ShowDialog(this) == DialogResult.Ok)
			{
				OpenFile(new FileInfo(dialog.FileName));
			}
		}

		public void OpenFile(FileInfo file)
		{
			FilePath = file.FullName;
			Dictionary<string, string[]> loadData = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(File.ReadAllText(FilePath));

			Data = new EditorData();

			foreach (var item in loadData)
			{
				if (EditorData.Sections.Contains(item.Key))
				{
					Data[item.Key] = item.Value;
				}
			}

			ResetView();

			Modified = false;
		}

		public void SaveFile(bool saveAs)
		{
			FileInfo file;

			if (!saveAs && !string.IsNullOrWhiteSpace(FilePath))
			{
				file = new FileInfo(FilePath);

				if (file.Exists)
				{
					SaveFile(file);
				}
			}
			else
			{
				using SaveFileDialog dialog = new SaveFileDialog
				{
					Title = saveAs ? "Save As" : "Save",
				};

				dialog.Filters.Add(Constants.DIALOG_FILTER_JSON);
				dialog.Filters.Add(Constants.DIALOG_FILTER_ANY);
				dialog.CurrentFilterIndex = 0;

				if (dialog.ShowDialog(this) == DialogResult.Ok)
				{
					FilePath = dialog.FileName;
					file = new FileInfo(FilePath);
					
					SaveFile(file);
				}
			}
		}

		public void SaveFile(FileInfo file)
		{
			try
			{
				if (file.Exists)
				{
					file.Delete();
				}

				File.WriteAllText(file.FullName, JsonConvert.SerializeObject(Data, Formatting.Indented));

				Modified = false;
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, ex.ToString(), "An error occured");
			}
		}

		public DialogResult ShowUnsavedDialog(string message = "", string title = "")
		{
			StringBuilder builder = new StringBuilder();
			builder.AppendLine("Your changes will be lost if you don't save them!");

			if (!string.IsNullOrWhiteSpace(message))
			{
				builder.AppendLine(message);
			}

			builder.AppendLine("\nAre you sure you want to continue?");


			return MessageBox.Show(this, builder.ToString(), title, MessageBoxButtons.YesNo, MessageBoxType.Warning);
		}

		private void ResetView()
		{
			View.Sections.SelectedIndex = 0;
			View.OnSectionChanged(View, new EventArgs());
		}
	}
}