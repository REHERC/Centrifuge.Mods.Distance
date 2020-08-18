using Distance.NitronicHUD.Storage;
using System.Collections;
using UnityEngine;

namespace Distance.NitronicHUD.Scripts
{
    public class VisualCountdown : MonoBehaviour
    {
        public const string AssetName = "Assets/Prefabs/NitronicCountdownHUD.prefab";

        public AssetBundle Bundle { get; set; }

        private GameObject Prefab { get; set; }

        public void Start()
        {
            Mod.Instance.Logger.Warning("Start");

            Bundle = new Assets("countdown").Bundle;

            foreach (var thing in Bundle.LoadAllAssets())
            {
                Mod.Instance.Logger.Warning(thing.name);
            }

            //yield break;
        }
    }
}
