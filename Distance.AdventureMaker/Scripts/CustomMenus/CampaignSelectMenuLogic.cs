using UnityEngine;

namespace Distance.AdventureMaker.Scripts.CustomMenus
{
	public class CampaignSelectMenuLogic : MonoBehaviour
	{
		private const string MODE_NAME = "Content Packs";
		private const string HEADER_TEXT = "All Campaigns";

		private GameObject levelSelectPanel_;

		private LevelSelectMenuLogic levelSelect;

		public static CampaignSelectMenuLogic CreateFromLevelSelect(LevelSelectMenuLogic levelSelect)
		{
			if (!levelSelect) return null;

			CampaignSelectMenuLogic campaignSelect = levelSelect.gameObject.GetOrAddComponent<CampaignSelectMenuLogic>();
			campaignSelect.Setup(levelSelect);
			return campaignSelect;
		}

		protected void Setup(LevelSelectMenuLogic levelSelect)
		{
			this.levelSelect = levelSelect;

			// Steal the references from the default menu script
			levelSelectPanel_ = levelSelect.levelSelectPanel_;

			SetupGUI();
		}

		protected void SetupGUI()
		{
			// Replace the "Sprint" header text with a custom one
			GameObject modeName = transform.Find("Panel - Level Select/Anchor - Top Left/Container/UILabel - Mode Name").gameObject;
			modeName.GetComponent<SetModeNameOnUILabel>().Destroy();
			modeName.GetComponent<UILabel>().text = MODE_NAME;

			// Hide the playlist editor button as well as ghost and replays ones
			levelSelect.SetVisibleLowerButtons(false);

			// Hide the "playlist editor" menu because it is visible by default
			levelSelect.levelPlaylistSelectMenu_.gameObject.SetActive(false);

			// Hide the playlist editor menus (save/load buttons too)
			levelSelect.levelPlaylistsPanel_.SetActive(false);
			levelSelect.playlistButtonGroup_.SetActive(false);

			// Disable the category pages (horizontal "scroll" between "All levels", "Official levels", ...)
			levelSelect.levelSelectPageDotBlueprint_.transform.parent.gameObject.SetActive(false);

			// Hide "personal best" and other stats
			levelSelect.medalInfoObj_.SetActive(false);

			GameObject medalCount = transform.Find("Panel - Level Select/Anchor - Top Left/Container/Group - Medal Count").gameObject;
			medalCount.SetActive(false);
		}

		public void Start()
		{
			// Clear the available level creator types so you can't switch from "All levels" to "Official levels"
			levelSelect.availableCreatorTypes_.Clear();
			//levelSelect.SetLevelSelectHeaderText();

			// Change the category name (replace the "All levels" text)
			levelSelect.levelSelectHeader_.text = HEADER_TEXT;
		}

		public void Display()
		{
			MenuPanel menuPanel = MenuPanel.Create(levelSelectPanel_, false, true, true, disablePrevRoot: true);
			menuPanel.backgroundOpacity_ = 0.75f;
			menuPanel.onIsTopChanged_ = new MenuPanel.OnIsTopChanged(OnIsTopChanged);
			menuPanel.Push(null);
		}

		protected void OnIsTopChanged(bool isTop)
		{
			// TODO: Implement (or not)
		}
	}
}
