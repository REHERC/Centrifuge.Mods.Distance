using Reactor.API.Attributes;
using Reactor.API.Interfaces.Systems;
using Reactor.API.Logging;
using Reactor.API.Storage;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Distance.TextureModifier
{
    [ModEntryPoint("com.distance.reherc/texturemodifier")]
    public class Entry : MonoBehaviour
    {
        public static Entry Instance;

        public IManager Manager { get; set; }

        public FileSystem Data { get; set; }

        public TextureLoader Loader { get; set; }

        public ResourceModificator Modifier { get; set; }

        public Log Logger { get; set; }

        public void Initialize(IManager manager)
        {
            Instance = this;
            Manager = manager;
            Data = new FileSystem();

            Logger = LogManager.GetForCurrentAssembly();

            Loader = new TextureLoader(Data);

            Modifier = new ResourceModificator(Loader);

            Reactor.API.Runtime.Patching.RuntimePatcher.AutoPatch();

            Events.Managers.AwakeGameManager.Subscribe(LateInitialize);
            Events.Level.PostLoad.Subscribe(LevelPostLoad);
        }

        internal void LateInitialize(Events.Managers.AwakeGameManager.Data data)
        {
            Events.Managers.AwakeGameManager.Unsubscribe(LateInitialize);

            StartCoroutine(ReskinResourcePrefabs());
        }

        public void Load()
        {
            Loader.ClearResources(true);
            Loader.LoadTextures("Textures");
        }

        internal void LevelPostLoad(Events.Level.PostLoad.Data data)
        {
            Load();

            StartCoroutine(ReskinLevelObjects(data.level_));
        }


        internal IEnumerator ReskinResourcePrefabs()
        {
            List<GameObject> objects = new List<GameObject>();

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

            objects.AddRange(Resources.FindObjectsOfTypeAll<GameObject>());
            objects.AddRange(level.AllGameObjectsInLevelIEnumerable_);

            level.Layers_.ForEach((layer) =>
            {
                objects.AddRange(layer.GameObjects_);
            });

            objects.AddRange(from affector in TrackMaster.affectorNodes_ select affector.Key.Link_.Spline_.gameObject);
            objects.AddRange(from renderer in FindObjectsOfType<Renderer>() select renderer.gameObject);
            objects.AddRange(from serializable in FindObjectsOfType<SerialComponent>() select serializable.gameObject);

            yield return StartCoroutine(Reskin(objects));

            objects.Clear();

            yield break;
        }

        internal IEnumerator Reskin(List<GameObject> list)
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
                yield return null;
            }

            yield break;
        }
    }
}
