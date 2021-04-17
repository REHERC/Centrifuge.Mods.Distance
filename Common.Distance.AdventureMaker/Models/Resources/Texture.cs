#if APP
using App.AdventureMaker.Core.Interfaces;
using App.AdventureMaker.Core.Lib_Interop;
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

			public override Image AsImage(IEditor<CampaignFile> editor)
			{
				FileInfo textureFile = editor.GetResourceFile(file);

				if (!textureFile.Exists) return null;

				return PfimImage.FromFile(textureFile);
			}
			#endif
		}
	}
}
