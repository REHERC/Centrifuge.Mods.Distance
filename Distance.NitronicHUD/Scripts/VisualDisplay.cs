using Reactor.API.Storage;
using UnityEngine;

namespace Distance.NitronicHUD.Scripts
{
    public class VisualDisplay : MonoBehaviour
    {
        public const string AssetName = "assets/nr hud/nr_hud.prefab";

        public Assets Assets { get; internal set; }

        public AssetBundle Bundle => Assets.Bundle as AssetBundle;

        private GameObject Prefab { get; set; }

        public void Awake()
        {
            CreatePrefab();
        }

        #region Initialize
        private void CreatePrefab()
        {
            Assets = new Assets("hud.assets");

            if (!Bundle)
            {
                Mod.Instance.Logger.Error($"The following assets file could not be loaded: hud.assets");

                DestroyImmediate(this);
                return;
            }

            Prefab = Instantiate(Bundle.LoadAsset<GameObject>(AssetName), transform);

            if (!Prefab)
            {
                Mod.Instance.Logger.Error($"The following asset from the hud.assets could not be loaded: \"{AssetName}\"");

                DestroyImmediate(this);
                return;
            }

            Prefab.name = "Visual Display";

            // The original prefab from larnin/NitronicHUD has misspelled names...
            Prefab.transform.Find("Hud_Left/Heat_Hight").name = "Heat_High";
            Prefab.transform.Find("Hud_Right/Heat_Hight").name = "Heat_High";

            // NOTE: Create subclass to handle logic
        }
        #endregion
    }
}
