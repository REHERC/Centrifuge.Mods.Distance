using System.IO;
using UnityEngine;

namespace Distance.AdventureMaker.DataModel
{
	public class LevelData
	{
		public readonly string alias;
		public readonly FileInfo levelFile;
		public readonly FileInfo thumbnailFile;

		private Texture2D thumbnailTexture_;
		public Texture2D ThumbnailTexture
		{
			get
			{
				if (!thumbnailTexture_ && thumbnailFile?.Exists == true)
				{
					thumbnailTexture_ = Resource.LoadTextureFromFile(thumbnailFile.FullName, 640, 360) as Texture2D;
				}

				return thumbnailTexture_;
			}
		}

		public LevelData(string alias, string level, string thumbnail = null) : this(alias, new FileInfo(level), new FileInfo(thumbnail))
		{
		}

		public LevelData(string alias, FileInfo level, FileInfo thumbnail = null)
		{
			this.alias = alias;
			levelFile = level;
			thumbnailFile = thumbnail;
		}

		public override int GetHashCode()
		{
			return levelFile.FullName.GetHashCode();
		}
	}
}
