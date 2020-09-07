using Centrifuge.Distance.Game;
using System;
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
            MessageBox.Create("Test", "TEST").Show();
        }

        public void Update()
        {
            if (Menu && Menu.currentMenuLayer_ == CustomizeCarColorsMenuLogic.MenuLayer.ColorPicker && G.Sys.InputManager_.GetKeyUp(InternalResources.Constants.EDIT_COLOR_INPUT))
            {
                OnButtonClick?.Invoke();
            }
        }
    }
}
