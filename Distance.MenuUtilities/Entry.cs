using Reactor.API.Attributes;
using Reactor.API.Interfaces.Systems;
using Reactor.API.Logging;
using Reactor.API.Runtime.Patching;
using UnityEngine;

namespace Distance.MenuUtilities
{
    [ModEntryPoint("com.github.reherc/Distance.MenuUtilities")]
    public class Mod : MonoBehaviour
    {
        public static Mod Instance;

        public IManager Manager { get; set; }

        public Log Logger { get; set; }

        //public ConfigurationLogic Config { get; private set; }

        public MonoBehaviour[] Scripts { get; set; }

        public void Initialize(IManager manager)
        {
            DontDestroyOnLoad(this);

            Instance = this;
            Manager = manager;
            Logger = LogManager.GetForCurrentAssembly();
            //Config = gameObject.AddComponent<ConfigurationLogic>();

            RuntimePatcher.AutoPatch();

            //CreateSettingsMenu();
        }
    }
}