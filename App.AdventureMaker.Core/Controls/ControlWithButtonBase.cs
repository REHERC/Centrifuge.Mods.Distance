using Eto.Drawing;
using Eto.Forms;
using System;

namespace App.AdventureMaker.Core.Controls
{
	// TODO: move to private inner class and make current class wrap around to set proper spacing (winforms only)
	public class ControlWithButtonBase<T> : StackLayout where T : Control
	{
		protected T Control { get; private set; }

		protected Button Button { get; private set; }

		public bool ControlEnabled
		{
			get => Control.Enabled;
			set => Control.Enabled = value;
		}

		public bool ButtonEnabled
		{
			get => Button.Enabled;
			set => Button.Enabled = value;
		}

		public ControlWithButtonBase(T control)
		{
			Style = "no-padding";

			Padding = Padding.Empty;

			Control = control;
			
			Button = new Button();
			Button.Click += (sender, e) => OnButtonClicked();

			Orientation = Orientation.Horizontal;
			VerticalContentAlignment = VerticalAlignment.Top;

			Items.Add(new StackLayoutItem(Control) { Expand = true });
			Items.Add(new StackLayoutItem(Button) { Expand = false });

			Spacing = 0;
		}

		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			Button.Focus();
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			Height = Math.Max(Control.Height, Button.Height);
			Button.Height = Math.Min(Button.Height, Control.Height);
		}

		protected virtual void OnButtonClicked() { }
	}
}
