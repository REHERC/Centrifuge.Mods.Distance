using Eto.Forms;
using System;
using System.IO;

namespace App.AdventureMaker.Core.Controls
{
	public class SelectFileButton : TextBoxWithButton
	{
		public event EventHandler<EventArgs> FileChanged;

		private readonly FileDialog dialog;

		public FileInfo File
		{
			get
			{
				return System.IO.File.Exists(Text) ? new FileInfo(Text) : null;
			}
			set
			{
				Text = value?.FullName;
				FileChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		public SelectFileButton(FileDialog dialog)
		{
			this.dialog = dialog;
			ControlEnabled = false;
		}

		protected override void OnButtonClicked()
		{
			base.OnButtonClicked();

			if (File?.Directory?.Exists == true)
			{
				dialog.Directory = new Uri(File.Directory.FullName, UriKind.Absolute);
			}

			if (dialog.ShowDialog(this) == DialogResult.Ok)
			{
				Text = dialog.FileName;
			}
		}
	}
}
