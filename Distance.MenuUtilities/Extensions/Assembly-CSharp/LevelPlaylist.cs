#pragma warning disable RCS1110
using Distance.MenuUtilities.Scripts;
using System;
using System.IO;
using UnityEngine;

public static class LevelPlaylistExtensions
{
	public static bool IsResourcesPlaylist(this LevelPlaylist playlist)
	{
		LevelPlaylistCompoundData data = playlist.gameObject.GetComponent<LevelPlaylistCompoundData>();

		if (!data)
		{
			return true;
		}

		string path = new FileInfo(data.FilePath).FullName.UniformPathName();
		string resourcesPath = new DirectoryInfo(Path.Combine(Application.dataPath, "Resources")).FullName.UniformPathName();

		return path.StartsWith(resourcesPath, StringComparison.InvariantCultureIgnoreCase);
	}
}