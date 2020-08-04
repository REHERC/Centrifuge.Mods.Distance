using Reactor.API.Attributes;
using Reactor.API.Interfaces.Systems;
using Reactor.API.Logging;
using Reactor.API.Runtime.Patching;
using UnityEngine;

namespace Distance.CustomDeathMessages
{
    [ModEntryPoint("com.github.larnin/Distance.CustomDeathMessages")]
    public class Mod : MonoBehaviour
    {
        public static Mod Instance { get; set; }

        public IManager Manager { get; set; }
        
        public ConfigurationLogic Config { get; set; }

        public Log Logger { get; set; }

        public void Initialize(IManager manager)
        {
            Instance = this;
            Manager = manager;

            Logger = LogManager.GetForCurrentAssembly();

            Config = gameObject.AddComponent<ConfigurationLogic>();

            RuntimePatcher.AutoPatch();
        }
    }
}