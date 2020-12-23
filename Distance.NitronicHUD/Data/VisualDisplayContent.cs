using UnityEngine;
using UnityEngine.UI;

namespace Distance.NitronicHUD.Data
{
	public struct VisualDisplayContent
	{
		public GameObject prefab;

		public RectTransform rectTransform;

		public Image flame;

		public Image main;

		public Image heatLow;
		public Image heatHigh;

		public Text speed;
		public Text speedLabel;

		public Text score;
		public Text scoreLabel;

		public VisualDisplayContent(GameObject obj)
		{
			prefab = obj;

			main = prefab?.GetComponent<Image>();
			rectTransform = prefab?.GetComponent<RectTransform>();

			flame = prefab?.transform.Find("Flame")?.GetComponent<Image>();

			heatLow = prefab?.transform.Find("Heat_Low")?.GetComponent<Image>();
			heatHigh = prefab?.transform.Find("Heat_High")?.GetComponent<Image>();

			speed = prefab?.transform.Find("Speed")?.GetComponent<Text>();
			speedLabel = prefab?.transform.Find("Speed_Label")?.GetComponent<Text>();

			score = prefab?.transform.Find("Score")?.GetComponent<Text>();
			scoreLabel = prefab?.transform.Find("Score_Label")?.GetComponent<Text>();
		}
	}
}
