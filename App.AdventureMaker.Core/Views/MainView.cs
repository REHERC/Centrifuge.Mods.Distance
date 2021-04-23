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

		public CampaignFile Document
		{
			get
			{
				CampaignFile data = new CampaignFile();
				editorView.SaveData(data);
				return data;
			}
			set
			{
				editorView.LoadData(value, false);
				Modified = true;
			}
		}
		#endregion

		private readonly EditorTabView editorView;
		private readonly EditorStartView startView;

		public MainView()
		{
			editorView = new EditorTabView(this);
			startView = new EditorStartView(this);

			OnLoaded += (_) =>
			{
				if (CurrentFile != null)
				{
					Content = editorView;
				}
				else
				{
					Content = startView;
				}
			};

			LoadFile(null as FileInfo);
		}

		public void SaveFile()
		{
			if (CurrentFile != null)
			{
				CampaignFile project = new CampaignFile();
				editorView.SaveData(project);
				project.Metadata.Version = DateTime.Now.TimeOfDay.Ticks;

				Json.Save(CurrentFile, project, true);

				Modified = false;
			}
		}

		public void LoadFile(string file, bool resetUI = true) => LoadFile(new FileInfo(file), resetUI);

		public void LoadFile(FileInfo file, bool resetUI = true)
		{
			if (file?.Exists == true)
			{
				if (Project.IsValidProjectManifest(file))
				{
					CurrentFile = file;
					CampaignFile project = Json.Load<CampaignFile>(file);
					editorView.LoadData(project, resetUI);
				}
			}
			else
			{
				CurrentFile = null;
				editorView.LoadData(new CampaignFile(), resetUI);
			}

			Modified = false;
			OnLoaded?.Invoke(this);
		}
	}
}