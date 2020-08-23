namespace Distance.Heat
{
    internal class GameState
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
    }
}