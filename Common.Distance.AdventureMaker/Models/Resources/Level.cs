#if APP
using App.AdventureMaker.Core.Interfaces;
using App.AdventureMaker.Core.Lib_Interop;
using Eto.Drawing;
#endif
using Distance.AdventureMaker.Common.Enums;
using Newtonsoft.Json;
using System.IO;
using System;

namespace Distance.AdventureMaker.Common.Models.Resources
{
	public abstract partial class CampaignResource
	{
		public class Level : CampaignResource
		{
			[JsonProperty("resource_type")]
			public override ResourceType resource_type => ResourceType.Level;

			[JsonProperty("thumbnail")]
			public string thumbnail;

			#if APP
			public override int required_files => 1;

			public override Image AsImage(IEditor<CampaignFile> editor)
			{
				FileInfo textureFile = editor.GetResourceFile(thumbnail);

				if (textureFile.Exists)
				{
					try
					{
						return PfimImage.FromFile(textureFile);
					}
					catch (Exception)
					{
						return null;
					}
				}

				return null;
			}
			#endif
		}
	}
}
