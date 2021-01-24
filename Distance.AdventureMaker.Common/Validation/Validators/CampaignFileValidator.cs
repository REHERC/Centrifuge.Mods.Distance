using Distance.AdventureMaker.Common.Enums;
using Distance.AdventureMaker.Common.Models;
using Distance.AdventureMaker.Common.Models.Resources;
using System;
using System.IO;

namespace Distance.AdventureMaker.Common.Validation.Validators
{
	public class CampaignFileValidator
		: Validator<CampaignFile>
		, IValidator<CampaignMetadata>
		, IValidator<CampaignData>
		, IValidator<CampaignPlaylist>
		, IValidator<CampaignLevel>
		, IValidator<CampaignResource>
		, IValidator<CampaignResource.Level>
		, IValidator<CampaignResource.Texture>
	{
		#region Members / Properties and Constructor
		public DirectoryInfo Directory { get; private set; }

		private CampaignFile @base;

		public CampaignFileValidator(DirectoryInfo dir)
		{
			Directory = dir;
		}
		#endregion

		public override void Validate(CampaignFile item)
		{
			@base = item;
			
			if (item.metadata is null)
			{
				Log(StatusLevel.ERR, "The metadata section was null when validating!");
			}
			else
			{
				Validate(item.metadata);
			}

			if (item.data is null)
			{
				Log(StatusLevel.ERR, "The data section was null when validating!");
			}
			else
			{
				Validate(item.data);
			}
		}

		public void Validate(CampaignMetadata data)
		{
			throw new NotImplementedException();
		}

		public void Validate(CampaignData data)
		{
			throw new NotImplementedException();
		}

		public void Validate(CampaignPlaylist data)
		{
			throw new NotImplementedException();
		}

		public void Validate(CampaignLevel data)
		{
			throw new NotImplementedException();
		}

		public void Validate(CampaignResource data)
		{
			throw new NotImplementedException();
		}

		public void Validate(CampaignResource.Level data)
		{
			throw new NotImplementedException();
		}

		public void Validate(CampaignResource.Texture data)
		{
			throw new NotImplementedException();
		}
	}
}
