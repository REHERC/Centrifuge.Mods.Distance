using System;
using System.IO;

namespace App.AdventureMaker.Core.Interfaces
{
	public interface IEditor<DATA>
	{
		public event Action<IEditor<DATA>> OnLoaded;

		public event Action<IEditor<DATA>> OnModified;

		public FileInfo CurrentFile { get; set; }

		public DATA Data { get; set; }

		public bool Modified { get; set; }

		public void SaveFile();

		public void LoadFile(FileInfo file);

		public void LoadFile(string file);
	}
}
