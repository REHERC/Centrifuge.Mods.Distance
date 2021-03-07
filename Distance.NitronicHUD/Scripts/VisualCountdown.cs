using Distance.NitronicHUD.Data;
using Reactor.API.Storage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Distance.NitronicHUD.Scripts
{
	public class VisualCountdown : MonoBehaviour
	{
		public const string AssetName = "Assets/Prefabs/NitronicCountdownHUD.prefab";

		public Assets Assets { get; internal set; }

		public AssetBundle Bundle => Assets.Bundle as AssetBundle;

		private GameObject Prefab { get; set; }

		private KeyValuePair<Image, VisualCountdownDigit>[] digits_;

		public void Awake()
		{
			CreatePrefab();
			InitializeDigits();
			SubscribeEvents();
		}

		#region Initialize
		private void CreatePrefab(bool loadBundle = true)
		{
			if (loadBundle)
			{
				Assets = new Assets("countdown.assets");
			}

			if (!Bundle)
			{
				Mod.Instance.Logger.Error("The following assets file could not be loaded: countdown.assets");

				DestroyImmediate(this);
				return;
			}

			Prefab = Instantiate(Bundle.LoadAsset<GameObject>(AssetName), transform);

			if (!Prefab)
			{
				Mod.Instance.Logger.Error($"The following asset from the countdown.assets could not be loaded: \"{AssetName}\"");

				DestroyImmediate(this);
				return;
			}

			Prefab.name = "Visual Countdown";
		}

		private void InitializeDigits()
		{
			digits_ = new KeyValuePair<Image, VisualCountdownDigit>[4];

			foreach (GameObject digitObject in Prefab.GetChildren())
			{
				// Child object names are "NR-3" "NR-2" "NR-1" and "NR-Rush"

				Image image = digitObject.GetComponent<Image>();
				VisualCountdownDigit digit;

				int number = 0;

				if (digitObject.name.Length >= 4 && int.TryParse(digitObject.name.Substring(3), out number))
				{
					digit = new VisualCountdownDigit(-number, 0.75f);
				}
				else
				{
					digit = new VisualCountdownDigit(0, 1.0f);
				}

				digits_[number] = new KeyValuePair<Image, VisualCountdownDigit>(image, digit);
			}
		}

		private void SubscribeEvents()
		{
			Events.Game.PauseToggled.Subscribe(OnPauseToggled);
			Events.Game.ModeInitialized.Subscribe(OnModeInitialized);
		}
		#endregion

		#region Events
		private void OnPauseToggled(Events.Game.PauseToggled.Data data)
		{
			Prefab.SetActive(!data.paused_);
		}

		private void OnModeInitialized(Events.Game.ModeInitialized.Data data)
		{
			Prefab.SetActive(true);
		}
		#endregion

		public void Update()
		{
			if (digits_.Length == 0)
			{
				return;
			}

			ConfigurationLogic config = Mod.Instance.Config;

			float time = (float)Timex.ModeTime_;

			// Make visible only in game mode
			if (!config.DisplayCountdown || !Flags.CanDisplayHudElements)
			{
				time = -10;
			}

			foreach (KeyValuePair<Image, VisualCountdownDigit> digit in digits_)
			{
				digit.Key.material.SetFloat("_Progress", GetInterpolation(time, digit.Value));
			}
		}

		public float GetInterpolation(float time, VisualCountdownDigit data)
		{
			float peak = data.start + (data.duration / 2);
			float diff = peak - data.start;
			float curve = (1 / diff * (-Mathf.Abs(time - data.start - diff))) + 1;
			return Mathf.Max(0, curve);
		}
	}
}
