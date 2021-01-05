using Eto.Forms;

namespace App.AdventureMaker.Core.Controls
{
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

			Control = control;

			Button = new Button();
			Button.Click += (sender, e) => OnButtonClicked();

			Orientation = Orientation.Horizontal;
			VerticalContentAlignment = VerticalAlignment.Center;

			Items.Add(new StackLayoutItem(Control) { Expand = true });
			Items.Add(new StackLayoutItem(Button) { Expand = false });

			this.GotFocus += (sender, e) => Button.Focus();
		}

		protected virtual void OnButtonClicked() { }
	}
}
