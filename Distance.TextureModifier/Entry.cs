using Reactor.API.Attributes;
using Reactor.API.Interfaces.Systems;
using Reactor.API.Logging;
using Reactor.API.Storage;
using System.Linq;
using UnityEngine;

namespace Distance.TextureModifier
{
    [ModEntryPoint("com.distance.reherc/texturemodifier")]
    public class Entry
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

        public void Load()
        {
            Loader.ClearResources();
            Loader.LoadTextures("Textures");
        }

        internal void LevelPostLoad(Events.Level.PostLoad.Data data)
        {
            Load();

            GameObject[] objects = data.level_.AllGameObjectsInLevelIEnumerable_.ToArray().Concat(Resources.FindObjectsOfTypeAll(typeof(GameObject))).Cast<GameObject>().ToArray();


            foreach (var obj in objects)
            {
                Modifier.PatchGameObject(obj);
            }

            foreach (var renderers in from obj in objects select obj.GetComponents<Renderer>().Concat(obj.GetComponentsInChildren<Renderer>(true)))
            {
                Modifier.PatchRenderers(renderers);
            }
        }
        
        internal void LateInitialize(Events.Managers.AwakeGameManager.Data data)
        {
            Events.Managers.AwakeGameManager.Unsubscribe(LateInitialize);
            
            Modifier.PatchCollection(Resource.LoadAllInFolder<Material>(Resource.materialsFolder_));
            Modifier.PatchCollection(Resources.FindObjectsOfTypeAll<Material>());

            Modifier.PatchCollection(Resource.LoadAllInFolder<GameObject>(Resource.prefabsFolder_));
            Modifier.PatchCollection(Resource.LoadAllInFolder<GameObject>(Resource.editorPrefabsFolder_));
            Modifier.PatchCollection(Resource.LoadAllInFolder<GameObject>(Resource.splineRoadTemplatesFolder_));
            Modifier.PatchCollection(Resource.LoadAllInFolder<GameObject>(Resource.splineTunnelTemplatesFolder_));
        }
    }
}
