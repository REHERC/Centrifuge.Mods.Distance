using App.AdventureMaker.Core.Controls;
using Distance.AdventureMaker.Common.Models.Resources;

namespace App.AdventureMaker.Core.Forms.Dialog
{
	public class LevelDialog : ResourceDialogBase<CampaignResource.Level>
	{
		public LevelDialog(CampaignResource.Level data) : base(data)
		{
		}

		protected override void InitializeComponent()
		{
			properties.AddRow("Level file:", new TextBoxWithButton());
			properties.AddRow("Thumbnail file:", new TextBoxWithButton());
		}
	}
}
