using Centrifuge.Distance.Game;

namespace Distance.AdventureMaker.Scripts.MainMenu
{
	public class CampaignsMenuButton : MainMenuButton
	{
		protected override string ButtonName => "Community Content Packs";

		protected override string ButtonText => "CONTENT PACKS";

		protected override int ItemPosition => 5;

		protected override void OnButtonClick()
		{
			MessageBox.Create("This feature isn't currently implemented.", ButtonText).Show();
			//SetVisible(false);
		}
	}
}
