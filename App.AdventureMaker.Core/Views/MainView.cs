using App.AdventureMaker.Core.Interfaces;
using Distance.AdventureMaker.Common.Models;
using Eto.Forms;
using System.IO;

namespace App.AdventureMaker.Core.Views
{
	public class MainView : Panel, IEditor<CampaignFile>
	{
		private readonly EditorTabView editor;

		public FileInfo CurrentFile { get; set; } = null;

		public bool Modified { get; set; }

		public MainView()
		{
			Content = editor = new EditorTabView();
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
			if (file.Exists)
			{
				CurrentFile = file;
				CampaignFile project = Json.Load<CampaignFile>(file);
				editor.LoadData(project);

				Modified = false;
			}
		}
	}
}
