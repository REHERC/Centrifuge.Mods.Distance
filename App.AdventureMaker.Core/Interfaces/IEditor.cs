using System;
using System.IO;

namespace App.AdventureMaker.Core.Interfaces
{
	public interface IEditor<DATA>
	{
		public event Action<IEditor<DATA>> OnLoaded;

		public event Action<IEditor<DATA>> OnModified;

		public FileInfo CurrentFile { get; set; }

		public DATA Document { get; set; }

		public bool Modified { get; set; }

		public void SaveFile();

		public void LoadFile(FileInfo file, bool resetUI = true);

		public void LoadFile(string file, bool resetUI = true);
	}
}
