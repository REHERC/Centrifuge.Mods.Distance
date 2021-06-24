using Centrifuge.Distance.Game;

namespace Distance.AdventureMaker.Scripts.MainMenu
{
	public class ContentPacksButton : MainMenuButton
	{
		protected override string ButtonName => "Community Content Packs";

		protected override string ButtonText => "CONTENT PACKS";

		protected override int ItemPosition => 5;

		protected override void OnButtonClick()
		{
			MessageBox.Create("This feature isn't currently implemented.", ButtonText).Show();
			//SetVisible(false);

			return;
			/*
			var controller = MenuLogic.gameObject.GetComponent<MainMenuButtonController>();

			if (!controller) return;

			controller.Campaign.Visible = false;
			controller.Arcade.Visible = false;
			controller.Multiplayer.Visible = false;
			controller.LevelEditor.Visible = false;
			controller.Workshop.Visible = false;
			controller.ContentPacks.Visible = false;
			controller.Garage.Visible = false;
			controller.ReportBug.Visible = true;
			controller.Options.Visible = false;
			controller.Quit.Visible = true;


			var title = MenuLogic.transform.Find("UI Root/Panel - Main/DistanceTitle").GetComponent<UILabel>();
			title.text = "N O";

			var version = MenuLogic.transform.Find("UI Root/Panel - Main/VersionNumber").GetComponent<UILabel>();
			version.text = "";
			*/
		}
	}
}
