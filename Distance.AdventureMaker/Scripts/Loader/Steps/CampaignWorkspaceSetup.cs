﻿using Centrifuge.Distance.Game;
using Reactor.API.Storage;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using static Distance.AdventureMaker.Loader.CampaignLoaderLogic;

namespace Distance.AdventureMaker.Loader.Steps
{
	public class CampaignWorkspaceSetup : LoaderTask, IEnumerable<DirectoryInfo>
	{
		private DirectoryInfo LocalCampaignsFolder;
		private DirectoryInfo DocumentsCampaignsFolder;

		public CampaignWorkspaceSetup(CampaignLoader loader) : base(loader)
		{
		}

		public IEnumerator<DirectoryInfo> GetEnumerator()
		{
			yield return LocalCampaignsFolder;
			yield return DocumentsCampaignsFolder;
		}

		public override IEnumerator Run(Task.Status status)
		{
			status.SetText("Creating directory structure...");

			FileSystem fs = new FileSystem();

			LocalCampaignsFolder = new DirectoryInfo(Path.Combine(fs.RootDirectory, "Campaigns"));
			LocalCampaignsFolder.CreateIfDoesntExist();

			DocumentsCampaignsFolder = new DirectoryInfo(Path.Combine(Resource.personalDistanceDirPath_, "Campaigns"));
			DocumentsCampaignsFolder.CreateIfDoesntExist();

			yield break;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			yield return GetEnumerator();
		}
	}
}
