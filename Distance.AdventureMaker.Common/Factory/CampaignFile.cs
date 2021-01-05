using System;
using System.IO;

namespace Distance.AdventureMaker.Common.Models
{
	public partial class CampaignFile
	{
		public static CampaignFile LoadFrom(FileInfo file)
		{
			if (!file.Exists)
			{
				throw new FileNotFoundException("The file does not exist", file.FullName);
			}

			return Json.Load<CampaignFile>(file);
		}

		#if APP
		public static CampaignFile CreateNew()
		{
			CampaignFile file = new CampaignFile();
			file.metadata.guid = Guid.NewGuid().ToString();
			return file;
		}

		public CampaignFile WithTitle(string title)
		{
			metadata.title = title;
			return this;
		}

		public CampaignFile WithDescription(string description)
		{
			metadata.description = description;
			return this;
		}

		public CampaignFile WithAuthor(string author, string contact = "")
		{
			metadata.author = author;
			metadata.contact = contact;
			return this;
		}
		#endif
	}
}
