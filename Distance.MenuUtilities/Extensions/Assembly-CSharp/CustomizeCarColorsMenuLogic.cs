#pragma warning disable RCS1110
using Distance.MenuUtilities.Scripts;
using System;

public static class CustomizeCarColorsMenuLogicExtensions
{
	public static void SetThirdAction(this CustomizeCarColorsMenuLogic menu, string text, InputAction input, Action callback)
	{
		CustomizeMenuCompoundData data = menu.GetComponent<CustomizeMenuCompoundData>();

		if (data)
		{
			data.OnButtonClick = callback;

			menu.deleteButton_.GetComponentInChildren<UILabel>().text = text;
			ControlsBasedUITexture texture = menu.deleteButton_.GetComponent<ControlsBasedUITexture>();

			texture.inputAction_ = input;
			texture.EvaluateLatestUsedDevice();
		}
	}

	public static void SetThirdActionEnabled(this CustomizeCarColorsMenuLogic menu, bool value)
	{
		menu.deleteButton_.gameObject.SetActive(value);
		menu.deleteButton_.enabled = value;
	}
}
