using Distance.AdventureMaker.DataModel;
using Distance.AdventureMaker.Interfaces;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Distance.AdventureMaker.Scripts.Managers.Campaigns
{
	internal class LevelRegister : ILevelRegister
	{
		private readonly Dictionary2Way<string, string> bidirectionalAliasToPathMap = new Dictionary2Way<string, string>();
		private readonly Dictionary<string, LevelData> aliasToDataMap = new Dictionary<string, LevelData>();

		public void RegisterLevel(string alias, FileInfo bytesFile, FileInfo thumbnailTexture = null)
		{
			RegisterLevel(alias, bytesFile.FullName, thumbnailTexture.FullName);
		}

		public void RegisterLevel(string alias, string bytesFile, string thumbnailTexture = null)
		{

			return;

			//string fullBytesPath = Path.GetFullPath(bytesFile);
			//string normBytesPath = fullBytesPath.NormalizePath();
			//string fullThumbPath = Path.GetFullPath(thumbnailTexture);
			//string normThumbPath = fullThumbPath.NormalizePath();

			//Mod.Instance.Logger.Error(alias);
			//Mod.Instance.Logger.Warning(normBytesPath);

			//if (!FileEx.Exists(fullBytesPath))
			//{
			//	throw new FileNotFoundException(bytesFile);
			//}

			//bidirectionalAliasToPathMap.Add(alias, bytesFile);
			//aliasToDataMap.Add(alias, new LevelData(alias, normBytesPath, fullThumbPath));

			//// Add level to game internal registry

			//LevelSetsManager lSets = G.Sys.LevelSets_;

			//LevelSettings lSettings = LevelSettings.CreateAndLoadFromPath(Path.GetFullPath(bytesFile), out bool _);
			//LevelInfo lInfo = LevelInfo.Create(Path.GetFullPath(bytesFile), lSettings);

			//lInfo.relativePath_ = alias;
			//lInfo.levelType_ = LevelType.My;
			//lInfo.fileNameWithoutExtension_ = Path.GetFileNameWithoutExtension(bytesFile);

			//lSets.allLevelPaths_.Add(normBytesPath);
			//lSets.communityLevelFilePaths_.Add(normBytesPath);
			//lSets.communityLevelInfos_.LevelPathsToLevelInfos_.Add(normBytesPath, lInfo);
			//lSets.AddLevelNameAndPathPair(normBytesPath, lInfo, lSets.allLevelNameAndPathPairs_);
			//lSets.AddLevelNameAndPathPair(normBytesPath, lInfo, lSets.communityLevelNameAndPathPairs_);
		}

		public string GetLevelAlias(FileInfo levelFile)
		{
			return levelFile != null ? GetLevelAlias(levelFile.FullName) : string.Empty;
		}

		public string GetLevelAlias(string levelPath)
		{
			if (bidirectionalAliasToPathMap.reverse_.TryGetValue(levelPath, out string alias)) {
				return alias;
			}

			return string.Empty;
		}

		public string GetLevelPath(string alias)
		{
			if (bidirectionalAliasToPathMap.forward_.TryGetValue(alias, out string levelPath))
			{
				return levelPath;
			}

			return string.Empty;
		}

		public Texture2D GetThumbnailFromPath(FileInfo levelFile)
		{
			return GetThumbnailFromPath(levelFile.FullName);
		}

		public Texture2D GetThumbnailFromPath(string levelPath)
		{
			return GetThumbnailFromAlias(GetLevelAlias(levelPath));
		}

		public Texture2D GetThumbnailFromAlias(string alias)
		{
			return aliasToDataMap[alias]?.ThumbnailTexture;
		}
	}
}
