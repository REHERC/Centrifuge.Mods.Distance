using App.AdventureMaker.Core.Controls;
using App.AdventureMaker.Core.Interfaces;
using Distance.AdventureMaker.Common.Models;
using Distance.AdventureMaker.Common.Models.Resources;
using Eto.Forms;
using System;
using System.IO;
using static Constants;
using static Dialogs;

namespace App.AdventureMaker.Core.Forms.ResourceDialogs
{
	public class LevelDialog : ResourceDialogBase<CampaignResource.Level>
	{
		private SelectFileButton levelFile;
		private SelectFileButton thumbnailFile;

		public LevelDialog(CampaignResource.Level data, IEditor<CampaignFile> editor) : base(data, editor)
		{
		}

		protected override void InitializeComponent()
		{
			properties.AddRow("Level file:", levelFile = new SelectFileButton(SelectBytesFileDialog("Select a level file to import")));

			if (!string.IsNullOrWhiteSpace(Data.file))
			{
				levelFile.File = editor.GetResourceFile(Data.file);
			}

			properties.AddRow("Thumbnail file:", thumbnailFile = new SelectFileButton(SelectImageDialog("Select a thumbnail image to import")));

			if (!string.IsNullOrWhiteSpace(Data.thumbnail))
			{
				thumbnailFile.File = editor.GetResourceFile(Data.thumbnail);
			}
		}

		protected override void Confirm(object sender, EventArgs e)
		{
			FileInfo level = levelFile.File;
			FileInfo thumbnail = thumbnailFile.File;

			if (level?.Exists != true)
			{
				MessageBox.Show("The specified level file could not be found!", DIALOG_CAPTION_MISSING_FILE);
				return;
			}

			if (thumbnail?.Exists != true)
			{
				MessageBox.Show("The specified thumbnail file could not be found!", DIALOG_CAPTION_MISSING_FILE);
				return;
			}

			ResourceImporter.ImportFile(editor, level, "levels", out string levelPath);
			Data.file = levelPath;

			ResourceImporter.ImportFile(editor, thumbnail, "textures", out string thumbnailPath);
			Data.thumbnail = thumbnailPath;

			base.Confirm(sender, e);
		}
	}
}
