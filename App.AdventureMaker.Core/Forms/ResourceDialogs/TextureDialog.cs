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
	public class TextureDialog : ResourceDialogBase<CampaignResource.Texture>
	{
		private SelectFileButton textureFile;

		public TextureDialog(CampaignResource.Texture data, IEditor<CampaignFile> editor) : base(data, editor)
		{
		}

		protected override void InitializeComponent()
		{
			properties.AddRow("Image file:", textureFile = new SelectFileButton(SelectImageDialog("Select an image file to import")));

			if (!string.IsNullOrWhiteSpace(Data.file))
			{
				textureFile.File = editor.GetResourceFile(Data.file);
			}
		}

		protected override void Confirm(object sender, EventArgs e)
		{
			FileInfo file = textureFile.File;

			if (file?.Exists != true)
			{
				MessageBox.Show("The specified texture file could not be found!", DIALOG_CAPTION_MISSING_FILE);
				return;
			}

			ResourceImporter.ImportFile(editor, file, "textures", out string filePath);

			Data.file = filePath;

			base.Confirm(sender, e);
		}
	}
}
