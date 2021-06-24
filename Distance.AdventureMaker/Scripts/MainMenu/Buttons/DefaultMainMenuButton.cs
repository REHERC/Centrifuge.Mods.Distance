namespace Distance.AdventureMaker.Scripts.MainMenu
{
	public class DefaultMainMenuButton : MainMenuButton
	{
		protected override string ButtonText => Button.GetComponentInChildren<UILabel>().text;

		protected override string ButtonName => Button.name;

		protected override int ItemPosition => Button.transform.GetSiblingIndex();

		public override void Setup(MainMenuLogic menuLogic)
		{
			if (menuLogic_) return;

			menuLogic_ = menuLogic;

			Table = FindTable();
			Button = gameObject;
		}

		protected override void OnButtonClick()
		{
			foreach (UIExButton button in Button.GetComponentsInChildren<UIExButton>())
			{
				button.OnClick();
			}
		}
	}
}