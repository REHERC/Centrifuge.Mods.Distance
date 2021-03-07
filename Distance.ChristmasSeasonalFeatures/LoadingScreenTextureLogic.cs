#pragma warning disable RCS1085
using System;
using System.IO;
using UnityEngine;

namespace Distance.ChristmasSeasonalFeatures
{
	public class LoadingScreenTextureLogic : MonoBehaviour
	{
		private Texture[] textures_;
		public Texture[] Textures
		{
			get => textures_;
			private set => textures_ = value;
		}

		private Texture[] defaultTextures_;
		public Texture[] DefaultTextures
		{
			get => defaultTextures_;
			private set => defaultTextures_ = value;
		}

		public void Awake()
		{
			textures_ = new Texture[0];
			defaultTextures_ = new Texture[0];
		}

		public void AddTexture(Texture texture)
		{
			Array.Resize(ref textures_, textures_.Length + 1);
			textures_[textures_.Length - 1] = texture;
		}

		public void AddTexture(FileInfo texture)
		{
			AddTexture(LoadTexture(texture));
		}

		public Texture LoadTexture(FileInfo file)
		{
			Texture2D texture = null;

			if (file.Exists)
			{
				texture = new Texture2D(512, 512)
				{
					anisoLevel = 1,
					filterMode = FilterMode.Trilinear,
					wrapMode = TextureWrapMode.Repeat
				};

				byte[] bitmapData = File.ReadAllBytes(file.FullName);
				texture.LoadImage(bitmapData);
			}

			return texture;
		}

		public void SetDefaulttextures(Texture[] textures)
		{
			if (defaultTextures_.Length == 0)
			{
				defaultTextures_ = textures.Clone() as Texture[];
			}
		}

		public void OnDestroy()
		{
			for (int i = 0; i < textures_.Length; i++)
			{
				Destroy(textures_[i]);
			}

			textures_ = null;
		}
	}
}
