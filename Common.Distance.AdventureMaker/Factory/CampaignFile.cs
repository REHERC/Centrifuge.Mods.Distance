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
			file.Metadata.Guid = Guid.NewGuid().ToString();
			return file;
		}

		public CampaignFile WithTitle(string title)
		{
			Metadata.Title = title;
			return this;
		}

		public CampaignFile WithDescription(string description)
		{
			Metadata.Description = description;
			return this;
		}

		public CampaignFile WithAuthor(string author, string contact = "")
		{
			Metadata.Author = author;
			Metadata.Contact = contact;
			return this;
		}
		#endif
	}
}
