using UnityEngine;

namespace Distance.AdventureMaker.Scripts.MainMenu
{
	public abstract class MainMenuButton : MonoBehaviour
	{
		protected virtual string ButtonName => "MENU BUTTON";

		protected virtual string ButtonText => "Text not set.";

		protected virtual int ItemPosition => 0;

		private MainMenuLogic menuLogic_;

		protected MainMenuLogic MenuLogic => menuLogic_;

		protected GameObject Button { get; private set; }

		protected UITable Table { get; private set; }

		public void Awake()
		{
			MainMenuLogic menuLogic = gameObject.GetComponent<MainMenuLogic>();
			if (menuLogic)
			{
				Setup(menuLogic);
			}
		}

		public void Setup(MainMenuLogic menuLogic)
		{
			// Do not create the button if it has already been created
			if (menuLogic_) return;

			menuLogic_ = menuLogic;

			Table = FindTable();
			Button = CreateButtonItem();

			// Rename/reposition the button
			Button.name = ButtonName;
			Button.transform.SetSiblingIndex(ItemPosition);

			// Set the button's label (text)
			Button.GetComponentInChildren<UILabel>().text = ButtonText;


			UnclickedMenuStatusIconLogic unclickedIcon = Button.GetComponentInChildren<UnclickedMenuStatusIconLogic>();
			if (unclickedIcon)
			{
				unclickedIcon.isDisabled_ = true;
				unclickedIcon.image_.enabled = true;
			}

			// There may be multiple UIExButton scripts, we only wanna set the
			// event handler on one of them. Otherwise the click event will 
			// trigger as many times as there are UIExButton/UIButton scripts
			// present on the object

			bool handlerSet = false;
			foreach (UIExButton button in Button.GetComponentsInChildren<UIExButton>())
			{
				button.onClick.Clear();
				if (!handlerSet)
				{
					button.onClick.Add(new EventDelegate(OnButtonClick));
					handlerSet = true;
				}
			}
		}

		private UITable FindTable()
		{
			return MenuLogic.mainButtons_.GetComponentInChildren<UITable>();
		}

		private GameObject CreateButtonItem()
		{
			// Get the table object (responsible for positioning the buttons vertically)
			Transform buttonTable = Table.transform;

			// Find one of the already existing buttons
			Transform buttonTemplate = buttonTable.Find("Arcade");

			// Return a copy of the previously found button
			return Instantiate(buttonTemplate.gameObject, buttonTable);
		}

		public void SetVisible(bool value)
		{
			Button.SetActive(value);
			Table.Reposition();
		}

		protected abstract void OnButtonClick();
	}
}
