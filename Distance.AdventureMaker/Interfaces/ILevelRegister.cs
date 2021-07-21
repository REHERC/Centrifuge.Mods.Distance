using System.IO;
using UnityEngine;

namespace Distance.AdventureMaker.Interfaces
{
	public interface ILevelRegister
	{
		void RegisterLevel(string alias, FileInfo bytesFile, FileInfo thumbnailTexture = null);

		void RegisterLevel(string alias, string bytesFile, string thumbnailTexture = null);

		string GetLevelAlias(string levelPath);

		string GetLevelPath(string alias);

		Texture2D GetThumbnailFromPath(FileInfo levelFile);

		Texture2D GetThumbnailFromPath(string levelPath);

		Texture2D GetThumbnailFromAlias(string alias);
	}
}
