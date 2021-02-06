using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;

namespace App.AdventureMaker.Core
{
	public static class RecentProjects
	{
		#region Members
		public const string RECENT_LIST_FILENAME = "recent.json";

		public static event Action OnChanged;

		private static ObservableCollection<FileInfo> FileList { get; set; }

		private static bool broadcastEvents_ = true;
		#endregion

		static RecentProjects()
		{
			FileList = new ObservableCollection<FileInfo>();

			List<string> files = Json.GetOrCreate(RECENT_LIST_FILENAME, new List<string>());

			try
			{
				broadcastEvents_ = false;

				foreach (FileInfo file in files.Select(item => new FileInfo(item)).Where(item => item.Exists))
				{
					FileList.Add(file);
				}
			}
			finally
			{
				broadcastEvents_ = true;
			}

			FileList.CollectionChanged += OnCollectionChanged;
		}

		public static FileInfo[] Get() => FileList.ToArray();

		private static void Save()
		{
			Json.Save(RECENT_LIST_FILENAME, FileList.Select(file => file.FullName).ToList(), true);
		}

		public static void Update(FileInfo file)
		{
			broadcastEvents_ = false;
			if (Equals(file, null) || !file.Exists) return;

			if (FileList.Contains(file))
			{
				FileList.Remove(file);
			}

			broadcastEvents_ = true;
			FileList.Insert(0, file);
		}

		private static void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (broadcastEvents_)
			{
				broadcastEvents_ = false;
				OnChanged?.Invoke();
				Save();
				broadcastEvents_ = true;
			}
		}
	}
}
