#if APP
using App.AdventureMaker.Core.Interfaces;
using Eto.Drawing;
#endif
using Distance.AdventureMaker.Common.Enums;
using Newtonsoft.Json;
using System.IO;

namespace Distance.AdventureMaker.Common.Models.Resources
{
	public abstract partial class CampaignResource
	{
		public class Texture : CampaignResource
		{
			[JsonProperty("resource_type")]
			public override ResourceType resource_type => ResourceType.Texture;

			#if APP
			public override int dependencies_count => 0;

			public Image AsImage(IEditor<CampaignFile> editor)
			{
				FileInfo textureFile = editor.GetResourceFile(file);

				if (!textureFile.Exists) return null;

				return new Bitmap(textureFile.FullName);
			}
			#endif
		}
	}
}
