using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Distance.MenuUtilities.Scripts
{
    public class CustomizeMenuCompoundData : MonoBehaviour
    {
        public CustomizeCarColorsMenuLogic Menu { get; internal set; }

        public Action OnButtonClick { get; internal set; } = null;

        public ColorChanger.ColorType ColorType { get; internal set; }

        public void EditHexClick()
        {
            void enableMenu(bool value)
            {
                Menu.enabled = value;
                //Menu.gameObject.SetActive(value);
                Menu.colorPickerPanel_.gameObject.SetActive(value);
            }

            ColorChanger.ColorType colorType = Menu.pickingColorType_;

            enableMenu(false);

            InputPromptPanel.Create(OnSubmit, () => { 
                enableMenu(true);
                //Menu.PickColorForType(colorType);
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
