using UnityEngine;

namespace Distance.AdventureMaker.Scripts.MainMenu
{
	public class MainMenuButtonController : MonoBehaviour
	{
		private MainMenuLogic menuLogic_;

		public MainMenuLogic MenuLogic => menuLogic_;

		public MainMenuButton Campaign { get; protected set; }

		public MainMenuButton Arcade { get; protected set; }

		public MainMenuButton Multiplayer { get; protected set; }

		public MainMenuButton LevelEditor { get; protected set; }

		public MainMenuButton Workshop { get; protected set; }

		public MainMenuButton ContentPacks { get; protected set; }

		public MainMenuButton Garage { get; protected set; }

		public MainMenuButton ReportBug { get; protected set; }

		public MainMenuButton Options { get; protected set; }

		public MainMenuButton Quit { get; protected set; }

		public void Awake()
		{
			menuLogic_ = GetComponent<MainMenuLogic>();
			if (!menuLogic_)
			{
				DestroyImmediate(this);
				return;
			}

			Campaign = GetOrAddButtonScript("Campaign");
			Arcade = GetOrAddButtonScript("Arcade");
			Multiplayer = GetOrAddButtonScript("Multiplayer");
			LevelEditor = GetOrAddButtonScript("Level Editor");
			Workshop = GetOrAddButtonScript("Steam Workshop");

			ContentPacks = GetOrAddButtonScript<ContentPacksButton>();

			Garage = GetOrAddButtonScript("Garage");
			ReportBug = GetOrAddButtonScript("Report Bug");
			Options = GetOrAddButtonScript("Options");
			Quit = GetOrAddButtonScript("Quit");
		}

		private MainMenuButton GetOrAddButtonScript(string buttonName)
		{
			GameObject button = menuLogic_.mainButtons_.transform.Find($"MainButtons/{buttonName}").gameObject;
			var script = button.GetOrAddComponent<DefaultMainMenuButton>();
			script.Setup(menuLogic_);
			return script;
		}

		private MainMenuButton GetOrAddButtonScript<T>() where T : MainMenuButton
		{
			return menuLogic_.GetOrAddComponent<T>();
		}
	}
}
