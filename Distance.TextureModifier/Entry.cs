using Centrifuge.Distance.Game;
using Centrifuge.Distance.GUI.Data;
using Reactor.API.Attributes;
using Reactor.API.Interfaces.Systems;
using Reactor.API.Logging;
using Reactor.API.Runtime.Patching;
using Reactor.API.Storage;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Distance.TextureModifier
{
	[ModEntryPoint("com.github.reherc/Distance.TextureModifier")]
	public class Mod : MonoBehaviour
	{
		public static Mod Instance;

		public IManager Manager { get; set; }

		public FileSystem Data { get; set; }

		public TextureLoader Loader { get; set; }

		public ResourceModificator Modifier { get; set; }

		public Log Logger { get; set; }

		public void Initialize(IManager manager)
		{
			DontDestroyOnLoad(this);

			Instance = this;
			Manager = manager;

			Data = new FileSystem();

			Logger = LogManager.GetForCurrentAssembly();

			Loader = new TextureLoader(Data);

			Modifier = new ResourceModificator(Loader);

			RuntimePatcher.AutoPatch();

			Events.Level.PostLoad.Subscribe(LevelPostLoad);

			CreateSettingsMenu();
		}

		internal void LateInitialize(IManager _)
		{
			StartCoroutine(ReskinResourcePrefabs());
		}

		public void CreateSettingsMenu()
		{
			MenuTree menu = new MenuTree("texturemodifier#main.menu", "Texture Modifier");

			menu.ActionButton(
				MenuDisplayMode.Both,
				"texturemodifier#main.menu/datafolder-open",
				"OPEN DATA FOLDER",
				() => Process.Start(Path.Combine(Data.RootDirectory, "Data")),
				"Open the folder used to load textures."
			);

			Menus.AddNew(MenuDisplayMode.Both, menu, "Settings for the Texture Modifier mod");
		}

		public void Load()
		{
			Loader.Load();
		}

		internal void LevelPostLoad(Events.Level.PostLoad.Data data)
		{
			Load();

			StartCoroutine(ReskinLevelObjects(data.level_));
		}

		internal IEnumerator ReskinResourcePrefabs()
		{
			List<GameObject> objects = new List<GameObject>();

			objects.AddRange(FindObjectsOfType<GameObject>());
			objects.AddRange(Resources.FindObjectsOfTypeAll<GameObject>());

			objects.AddRange(Resource.LoadAllInFolder<GameObject>(Resource.editorPrefabsFolder_).Cast<GameObject>());
			objects.AddRange(Resource.LoadAllInFolder<GameObject>(Resource.splineRoadTemplatesFolder_).Cast<GameObject>());
			objects.AddRange(Resource.LoadAllInFolder<GameObject>(Resource.splineTunnelTemplatesFolder_).Cast<GameObject>());
			objects.AddRange(from spline in Resources.FindObjectsOfTypeAll<SplineSegment>() select spline.gameObject);

			yield return StartCoroutine(Reskin(objects));

			objects.Clear();

			yield break;
		}

		internal IEnumerator ReskinLevelObjects(Level level)
		{
			List<GameObject> objects = new List<GameObject>();

			objects.AddRange(FindObjectsOfType<GameObject>());
			objects.AddRange(Resources.FindObjectsOfTypeAll<GameObject>());
			objects.AddRange(level.AllGameObjectsInLevelIEnumerable_);

			level.Layers_.ForEach((layer) => objects.AddRange(layer.GameObjects_));

			objects.AddRange(from affector in TrackMaster.affectorNodes_ select affector.Key.Link_.Spline_.gameObject);
			objects.AddRange(from renderer in FindObjectsOfType<Renderer>() select renderer.gameObject);
			objects.AddRange(from serializable in FindObjectsOfType<SerialComponent>() select serializable.gameObject);

			yield return StartCoroutine(Reskin(objects));

			objects.Clear();

			yield break;
		}

		internal IEnumerator Reskin(List<GameObject> list, bool pause = false)
		{
			yield return null;

			GameObject[] objects = list.ToArray().RemoveDuplicates();

			for (int index = 0; index < objects.Length; index++)
			{
				try
				{
					GameObject current = objects[index];

					if (!current)
					{
						continue;
					}

					Modifier.PatchGameObject(current);
					Modifier.PatchGameObjects(current.GetChildren());
				}
				catch (System.Exception error)
				{
					Logger.Exception(error);
				}

				if (pause)
				{
					yield return null;
				}
			}

			//if (!pause)
			//{
			yield return StartCoroutine(Reskin(list, true));
			//}

			yield break;
		}
	}
}
