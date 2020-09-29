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

        public ColorChanger.ColorType ColorType { get; internal set; }

        public UISprite Underline { get; internal set; }

        public bool IsEditing { get; internal set; }

        private IEnumerator CloseInput()
        {
            InputManager im = G.Sys.InputManager_;

            yield return new WaitUntil(() => !im.GetKeyUp(InternalResources.Constants.INPUT_EDIT_COLOR));

            EnableMenu(true);

            yield return new WaitUntil(() => Menu.currentMenuLayer_ != CustomizeCarColorsMenuLogic.MenuLayer.ColorPicker);
            yield return new WaitForEndOfFrame();

            Menu.pickingColorType_ = ColorType;

            Menu.PickColorForType(ColorType);

            Menu.currentMenuLayer_ = CustomizeCarColorsMenuLogic.MenuLayer.ColorPicker;
            Menu.SetUnderlines();

            Underline.gameObject.SetActive(true);
            Underline.enabled = true;

            yield break;
        }

        private void EnableMenu(bool value)
        {
            Menu.enabled = value;
            Menu.gameObject.SetActive(value);
            Menu.colorPickerPanel_.gameObject.SetActive(value);
        }

        public void EditHexClick()
        {
            ColorType = Menu.pickingColorType_;

            IsEditing = true;

            for (ColorChanger.ColorType color = ColorChanger.ColorType.Primary; color < ColorChanger.ColorType.Size_; color++)
            {
                if (color == ColorType)
                {
                    Underline = Menu.colorTypeButtons_[(int)color].underline_;
                    break;
                }
            }

            Mod.Instance.Logger.Warning($"Selected: {ColorType}");

            EnableMenu(false);

            InputPromptPanel.Create(OnSubmit, () =>
            {
                Mod.Instance.StartCoroutine(CloseInput());
            }, "HEX COLOR", "");
        }

        private bool OnSubmit(out string error, string input)
        {
            Regex hexRegex = new Regex(InternalResources.Constants.REGEX_HEXADECIMAL_COLOR);
            Match hexMatch = hexRegex.Match(input);
            if (hexMatch.Success)
            {
                // TODO:Add logic
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
            if (Menu && Menu.currentMenuLayer_ == CustomizeCarColorsMenuLogic.MenuLayer.ColorPicker && G.Sys.InputManager_.GetKeyUp(InternalResources.Constants.INPUT_EDIT_COLOR))
            {
                OnButtonClick?.Invoke();
            }
        }
    }
}
