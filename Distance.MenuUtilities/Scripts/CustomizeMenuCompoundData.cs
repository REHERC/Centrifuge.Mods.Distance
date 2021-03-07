using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Distance.MenuUtilities.Scripts
{
	public class CustomizeMenuCompoundData : MonoBehaviour
	{
		public CustomizeCarColorsMenuLogic Menu { get; internal set; }

		public Action OnButtonClick { get; internal set; } = null;

		public bool IsEditing { get; internal set; }

		public ColorChanger.ColorType ColorType { get; internal set; }

		private IEnumerator CloseInput()
		{
			InputManager im = G.Sys.InputManager_;

			yield return new WaitUntil(() => im.GetKeyUp(InputAction.MenuConfirm) || im.GetKeyUp(InputAction.MenuCancel));

			IsEditing = false;
			EnableMenu(true);

			yield break;
		}

		private void EnableMenu(bool value, bool setMenuState = true)
		{
			if (setMenuState)
			{
				Menu.enabled = value;
			}

			Menu.gameObject.SetActive(value);
			Menu.colorPickerPanel_.gameObject.SetActive(value);
		}

		public void EditHexClick()
		{
			IsEditing = true;

			EnableMenu(false);

			Color color = Menu.colorPicker_.Color_;

			InputPromptPanel.Create(OnSubmit, () => Mod.Instance.StartCoroutine(CloseInput()), "HEX COLOR", $"#{ColorEx.ColorToHexUnity(color).ToUpper()}");
		}

		private bool OnSubmit(out string error, string input)
		{
			Regex hexRegex = new Regex(InternalResources.Constants.REGEX_HEXADECIMAL_COLOR);
			Match hexMatch = hexRegex.Match(input);
			if (hexMatch.Success)
			{
				Menu.modifiedColorsOrCar_ = true;
				Color color = hexMatch.Groups["color"].Captures[0].Value.ToColor();
				ColorHSB colorHSB = color.ToColorHSB();

				Menu.colorPicker_.Color_ = color;
				Menu.colorPicker_.Position_ = new Vector2(colorHSB.s, colorHSB.b);
				Menu.colorPicker_.hueSlider_.value = colorHSB.h;
				Menu.colorPicker_.NewValuesSet(true, true);

				error = "";
				return true;
			}
			else
			{
				error = "Invalid hex code";
				return false;
			}
		}

		public void Update()
		{
			if (Mod.Instance.Config.EnableHexColorInput && Menu && Menu.currentMenuLayer_ == CustomizeCarColorsMenuLogic.MenuLayer.ColorPicker && G.Sys.InputManager_.GetKeyUp(InternalResources.Constants.INPUT_EDIT_COLOR))
			{
				OnButtonClick?.Invoke();
			}
		}
	}
}
