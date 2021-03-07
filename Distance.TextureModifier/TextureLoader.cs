using Reactor.API.Storage;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Distance.TextureModifier
{
	public class TextureLoader
	{
		private readonly FileSystem fileSystem_;

		private Object[] textures;

		private Object[] materials;

		public TextureLoader(FileSystem fileSystem)
		{
			fileSystem_ = fileSystem;
			textures = new Object[0];
			materials = new Object[0];
		}

		public void Load(bool gc = false)
		{
			ClearResources(gc);
			LoadTextures("Textures");
		}

		public void ClearResources(bool garbageCollect = false)
		{
			ClearResources(ref textures);
			ClearResources(ref materials);

			if (garbageCollect)
			{
				System.GC.Collect();
			}
		}

		public void ClearResources(ref Object[] list)
		{
			for (int index = 0; index < list.Length; index++)
			{
				ref Object resource = ref list[index];

				Object.Destroy(resource);
				resource?.Destroy();

				resource = null;
			}

			System.Array.Resize(ref list, 0);
		}

		public void LoadTextures(string folder)
		{
			DirectoryInfo target = new DirectoryInfo(Path.Combine(Path.Combine(fileSystem_.RootDirectory, "Data"), folder));

			if (target.Exists)
			{
				LoadTextures(target);
			}
		}

		public void LoadTextures(DirectoryInfo directory, int stack = 10)
		{
			if (stack <= 0)
			{
				return;
			}
			else if (directory.Exists)
			{
				foreach (var file in from entry in directory.GetFiles() where Declarations.textureFileExtensions.Contains(entry.Extension.ToLowerInvariant()) select entry)
				{
					LoadTexture(file);
				}

				foreach (var subfolder in directory.GetDirectories())
				{
					LoadTextures(subfolder, stack - 1);
				}
			}
		}

		public void LoadTexture(FileInfo file)
		{
			if (file.Exists)
			{
				Texture2D texture = new Texture2D(512, 512)
				{
					anisoLevel = 1,
					filterMode = FilterMode.Trilinear,
					wrapMode = TextureWrapMode.Repeat
				};

				byte[] bitmapData = File.ReadAllBytes(file.FullName);
				texture.LoadImage(bitmapData);

				System.Array.Resize(ref textures, textures.Length + 1);
				textures[textures.Length - 1] = texture as Texture2D;

				Material material = new Material(Shader.Find(Declarations.CustomShadelessTexture));

				material.SetTexture("_MainTex", texture);
				material.SetTextureScale("_MainTex", Vector2.one * Mathf.Round(Random.Range(0, 50) / 10));
				material.SetColor("_Color", new Color(0.75f, 0.75f, 0.75f, 0.02f));

				System.Array.Resize(ref materials, materials.Length + 1);
				materials[materials.Length - 1] = material as Material;
			}
		}

		public Texture2D GetRandomTexture(int stack = 10)
		{
			if (textures.Length is 0 || stack <= 0)
			{
				return null;
			}
			else
			{
				Texture2D result = textures.RandomElement() as Texture2D;

				if (result)
				{
					return result;
				}
				else
				{
					Load(true);
					return GetRandomTexture(stack - 1);
				}
			}
		}

		public Material GetRandomMaterial(int stack = 10)
		{
			if (materials.Length is 0 || stack <= 0)
			{
				return null;
			}
			else
			{
				Material result = materials.RandomElement() as Material;

				if (result)
				{
					return result;
				}
				else
				{
					Load(true);
					return GetRandomMaterial(stack - 1);
				}
			}
		}
	}
}
