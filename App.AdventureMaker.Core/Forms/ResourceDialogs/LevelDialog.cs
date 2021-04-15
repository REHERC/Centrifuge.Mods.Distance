﻿using App.AdventureMaker.Core.Controls;
using App.AdventureMaker.Core.Interfaces;
using Distance.AdventureMaker.Common.Models;
using Distance.AdventureMaker.Common.Models.Resources;

namespace App.AdventureMaker.Core.Forms.ResourceDialogs
{
	public class LevelDialog : ResourceDialogBase<CampaignResource.Level>
	{
		private TextBoxWithButton levelFile;
		private TextBoxWithButton thumbnailFile;

		public LevelDialog(CampaignResource.Level data, IEditor<CampaignFile> editor) : base(data, editor)
		{
		}

		protected override void InitializeComponent()
		{
			properties.AddRow("Level file:", levelFile = new TextBoxWithButton() { Text = Data.file });
			levelFile.TextChanged += (sender, e) => Data.file = levelFile.Text;

			properties.AddRow("Thumbnail file:", thumbnailFile = new TextBoxWithButton() { Text = Data.thumbnail });
			thumbnailFile.TextChanged += (sender, e) => Data.thumbnail = thumbnailFile.Text;
		}
	}
}
