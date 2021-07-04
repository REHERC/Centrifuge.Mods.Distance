using Centrifuge.Distance.Game;
using UnityEngine;

namespace Distance.AdventureMaker.Scripts.MainMenu
{
	public class ContentPacksButton : MainMenuButton
	{
		protected override string ButtonName => "Community Content Packs";

		protected override string ButtonText => "CONTENT PACKS";

		protected override int ItemPosition => 5;

		protected LevelSelectMenuAbstract levelSelect;

		public override void Setup(MainMenuLogic menuLogic)
		{
			base.Setup(menuLogic);

			LevelGridMenu levelGrid = menuLogic.levelSelectLogic_ as LevelGridMenu;

			GameObject levelSelectObj = Instantiate(levelGrid.advancedMenu_.gameObject);
			levelSelectObj.name = "CampaignSelectRoot";
			levelSelect = levelSelectObj.GetComponent<LevelSelectMenuLogic>();
		}

		protected override void OnButtonClick()
		{
			//MenuLogic.levelSelectLogic_
			levelSelect.Display(LevelSelectMenuAbstract.DisplayType.Arcade, new LevelSelectMenuAbstract.OnLevelSelectedDelegate((_, __) => { }), null);



			//MessageBox.Create("This feature isn't currently implemented.", ButtonText).Show();

			//return;
			
			/*var controller = MenuLogic.gameObject.GetComponent<MainMenuButtonController>();

			if (!controller) return;

			controller.Campaign.Visible = false;
			controller.Arcade.Visible = false;
			controller.Multiplayer.Visible = false;
			controller.LevelEditor.Visible = false;
			controller.Workshop.Visible = false;
			controller.ContentPacks.Visible = false;
			controller.Garage.Visible = false;
			controller.ReportBug.Visible = false;
			controller.Options.Visible = false;
			controller.Quit.Visible = false;

			var title = MenuLogic.transform.Find("UI Root/Panel - Main/DistanceTitle").GetComponent<UILabel>();
			title.text = "S A M P L E   T E X T";

			var version = MenuLogic.transform.Find("UI Root/Panel - Main/VersionNumber").GetComponent<UILabel>();
			version.text = "";*/
		}
	}
}
