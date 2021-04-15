﻿#pragma warning disable RCS1110

using Eto.Forms;
using static Constants;

public static class Dialogs
{
	public static OpenFileDialog SelectImageDialog(string title)
	{
		return new OpenFileDialog()
		{
			Title = title,
			CheckFileExists = true,
			Filters =
			{
				DIALOG_FILTER_PNG,
				DIALOG_FILTER_JPG,
				DIALOG_FILTER_BMP,
				DIALOG_FILTER_TGA,
				DIALOG_FILTER_TIF,
				DIALOG_FILTER_ANY
			}
		};
	}
}