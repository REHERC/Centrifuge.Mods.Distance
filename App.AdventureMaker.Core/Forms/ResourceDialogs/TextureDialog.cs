using App.AdventureMaker.Core.Controls;
using Distance.AdventureMaker.Common.Models.Resources;

namespace App.AdventureMaker.Core.Forms.Dialog
{
	public class TextureDialog : ResourceDialogBase<CampaignResource.Texture>
	{
		private TextBoxWithButton textureFile;

		public TextureDialog(CampaignResource.Texture data) : base(data)
		{
		}

		protected override void InitializeComponent()
		{
			properties.AddRow("Image file:", textureFile = new TextBoxWithButton() { Text = Data.file });
			textureFile.TextChanged += (sender, e) => Data.file = textureFile.Text;
		}
	}
}
