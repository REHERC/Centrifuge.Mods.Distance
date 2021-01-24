using App.AdventureMaker.Core.Controls;
using App.AdventureMaker.Core.Interfaces;
using Distance.AdventureMaker.Common.Models;
using Distance.AdventureMaker.Common.Models.Resources;
using Eto.Forms;

namespace App.AdventureMaker.Core.Views
{
	public class ResourcesPage : PropertiesListBase, ISaveLoad<CampaignFile>
	{

		public ResourcesPage(IEditor<CampaignFile> editor)
		{
			
		}

		void ISaveLoad<CampaignFile>.SaveData(CampaignFile project)
		{
			//project.data.resources.Add(new CampaignResource.Texture() { file = "logo.png" });
			//project.data.resources.Add(new CampaignResource.Level() { level = "file.bytes", thumbnail = "level.bytes.png" });
		}

		void ISaveLoad<CampaignFile>.LoadData(CampaignFile project)
		{

		}
	}
}