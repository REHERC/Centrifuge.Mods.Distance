using App.AdventureMaker.Core.Interfaces;
using Distance.AdventureMaker.Common.Models;
using Eto.Forms;
using System;
using System.IO;

namespace App.AdventureMaker.Core.Views
{
	public class MainView : Panel, IEditor<CampaignFile>
	{
		#region Interface Events and Members
		public event Action<IEditor<CampaignFile>> OnLoaded;

		public event Action<IEditor<CampaignFile>> OnModified;

		public FileInfo CurrentFile { get; set; } = null;

		private bool modified_;
		public bool Modified
		{
			get => modified_;
			set
			{
				modified_ = value;
				OnModified?.Invoke(this);
			}
		}
		#endregion

		private readonly EditorTabView editor;

		public MainView()
		{
			Content = editor = new EditorTabView(this);

			OnLoaded += (_) =>
			{
				editor.Visible = CurrentFile != null;
			};

			LoadFile(null as FileInfo);
		}

		public void SaveFile()
		{
			if (CurrentFile != null)
			{
				CampaignFile project = new CampaignFile();
				editor.SaveData(project);
				Json.Save(CurrentFile, project, true);

				Modified = false;
			}
		}

		public void LoadFile(string file) => LoadFile(new FileInfo(file));

		public void LoadFile(FileInfo file)
		{
			if (file != null && file.Exists)
			{
				CurrentFile = file;
				CampaignFile project = Json.Load<CampaignFile>(file);
				editor.LoadData(project);
			}
			else
			{
				CurrentFile = null;
				editor.LoadData(new CampaignFile());
			}

			Modified = false;
			OnLoaded?.Invoke(this);
		}
	}
}