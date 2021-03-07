using Eto.Forms;
using System;

namespace App.AdventureMaker.Core.Controls
{
	public class TextBoxWithButton : ControlWithButtonBase<TextBox>
	{
		public event EventHandler<EventArgs> TextChanged;

		public string Text
		{
			get => Control.Text;
			set
			{
				Control.Text = value;
				TextChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		public TextBoxWithButton() : base(new TextBox())
		{
			Button.Text = "...";
			Control.TextChanged += (sender, e) =>
			Text = Control.Text;
		}
	}
}
