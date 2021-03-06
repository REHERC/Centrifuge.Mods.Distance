﻿using Eto.Forms;
using System;

namespace App.AdventureMaker.Core.Controls
{
	public class GuidLabel : ControlWithButtonBase<Label>
	{
		public const string MESSAGE_TITLE = "Regenerate GUID";
		public const string MESSAGE_TEXT = "Are you sure you want to generate a new identifier?\nThis identifier is used for linking save data, generating a new one will make old saves incompatible!";

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

		public GuidLabel() : base(new Label(), VerticalAlignment.Center)
		{
			Control.VerticalAlignment = VerticalAlignment.Center;
			Button.Text = "Renew";
		}

		protected override void OnButtonClicked()
		{
			if (MessageBox.Show(MESSAGE_TEXT, MESSAGE_TITLE, MessageBoxButtons.YesNo, MessageBoxType.Warning) == DialogResult.Yes)
			{
				Text = Guid.NewGuid().ToString();
			}
		}
	}
}
