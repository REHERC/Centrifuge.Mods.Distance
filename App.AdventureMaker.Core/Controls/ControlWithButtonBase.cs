using Eto.Drawing;
using Eto.Forms;
using System;

namespace App.AdventureMaker.Core.Controls
{
	// TODO: move to private inner class and make current class wrap around to set proper spacing (winforms only)
	public class ControlWithButtonBase<T> : StackLayout where T : Control
	{
		protected T Control { get; }

		protected Button Button { get; }

		public event Action ButtonClick;

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

		public ControlWithButtonBase(T control, VerticalAlignment controlAlign = VerticalAlignment.Stretch)
		{
			Style = "no-padding horizontal";

			Padding = Padding.Empty;

			Control = control;

			Control.Style = "with-button";

			Button = new Button();
			Button.Click += (sender, e) =>
			{
				OnButtonClicked();
				ButtonClick?.Invoke();
			};

			Items.Add(new StackLayoutItem(Control) { Expand = true, VerticalAlignment = controlAlign });
			Items.Add(new StackLayoutItem(Button) { Expand = false });

			Spacing = 8;
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
