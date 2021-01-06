﻿using Eto.Forms;
using System;

namespace App.AdventureMaker.Core.Controls
{
	public class TextBoxWithButton : ControlWithButtonBase<TextBox>
	{
		public string Text
		{
			get => Control.Text;
			set => Control.Text = value;
		}

		public TextBoxWithButton() : base(new TextBox())
		{
			Button.Text = "...";
		}

		protected override void OnButtonClicked()
		{
			Text = Guid.NewGuid().ToString();
		}
	}
}