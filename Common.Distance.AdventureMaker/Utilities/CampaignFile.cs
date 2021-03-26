using Distance.AdventureMaker.Common.Enums;
using Distance.AdventureMaker.Common.Models.Resources;

namespace Distance.AdventureMaker.Common.Models
{
	public partial class CampaignFile
    {
        public CampaignResource GetResource(string guid, ResourceType type)
		{
			return Data.Resources.Find((res) => Equals(res.guid, guid) && Equals(res.resource_type, type));
		}
    }
}
