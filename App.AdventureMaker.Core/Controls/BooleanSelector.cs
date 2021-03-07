using Eto.Forms;
using System;
using Eto.Drawing;

namespace App.AdventureMaker.Core.Controls
{
	public class BooleanSelector : RadioButtonList
	{
		private static readonly Size DefaultSpacing = new Size(8, 8);

		public event EventHandler<EventArgs> ValueChanged;

		private bool value_;
		public bool Value
		{
			get => value_;
			set
			{
				if (value_ != value)
				{
					value_ = value;
					ValueChanged?.Invoke(this, EventArgs.Empty);

					SelectedIndex = value ? 0 : 1;
				}
			}
		}


		public BooleanSelector(string trueText = "Yes", string falseText = "No", Orientation orientation = Orientation.Horizontal)
		{
			Style = "no-padding";

			Items.Add(trueText);
			Items.Add(falseText);

			SelectedIndex = 1;

			Orientation = orientation;
			Spacing = DefaultSpacing;
		}

		protected override void OnSelectedIndexChanged(EventArgs e)
		{
			Value = SelectedIndex == 0;
		}
	}
}
