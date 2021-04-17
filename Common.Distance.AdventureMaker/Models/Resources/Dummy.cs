#if APP
#pragma warning disable RCS1079
using App.AdventureMaker.Core.Interfaces;
using Eto.Drawing;
#endif
using Distance.AdventureMaker.Common.Enums;
using Newtonsoft.Json;
using System;

namespace Distance.AdventureMaker.Common.Models.Resources
{
	public abstract partial class CampaignResource
	{
		public class Dummy : CampaignResource
		{
			[JsonProperty("resource_type")]
			public override ResourceType resource_type => ResourceType.None;

			#if APP
			[JsonIgnore]
			public override int dependencies_count => 0;

			public override Image AsImage(IEditor<CampaignFile> editor)
			{
				throw new NotImplementedException();
			}
#endif

			public Dummy()
			{
				file = "<None>";
			}
		}
	}
}
