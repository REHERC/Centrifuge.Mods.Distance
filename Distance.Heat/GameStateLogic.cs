using UnityEngine;

namespace Distance.Heat
{
    internal class GameStateLogic : MonoBehaviour
    {
        public GeneralSettings GeneralSettings;
        public HoverScreenEmitter HoverScreenEmitter;
        public CarLogic CarLogic;
        public CarStats CarStats;

        public void Update()
        {
            var playerManager = G.Sys.PlayerManager_;
            var optionsManager = G.Sys.OptionsManager_;
            GeneralSettings = optionsManager ? optionsManager.General_ : null;
            var localPlayer = playerManager ? playerManager.Current_ : null;
            var playerDataLocal = localPlayer?.playerData_;
            var carGameObject = playerDataLocal ? playerDataLocal.Car_ : null;
            HoverScreenEmitter = carGameObject ? carGameObject.GetComponent<HoverScreenEmitter>() : null;
            CarLogic = playerDataLocal ? playerDataLocal.CarLogic_ : null;
            CarStats = CarLogic ? CarLogic.CarStats_ : null;
        }

        internal static UILabel GetAndActivateWatermark()
        {
            Transform anchorAlphaVersion = GameObject.Find("UI Root").transform.Find("Panel/Anchor : AlphaVersion");
            Transform alphaVersion = anchorAlphaVersion.Find("AlphaVersion");

            anchorAlphaVersion.gameObject.SetActive(true);
            alphaVersion.gameObject.SetActive(true);

            return alphaVersion.GetComponent<UILabel>();
        }
    }
}