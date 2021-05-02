using Centrifuge.Distance.Game;
using System;
using System.Collections;
using System.Text;
using UnityEngine;
using static Distance.AdventureMaker.Loader.CampaignLoaderLogic;

namespace Distance.AdventureMaker.Loader.Steps
{
	public class CampaignImporter : LoaderTask
	{
		public CampaignImporter(CampaignLoader loader) : base(loader)
		{
		}

		public override IEnumerator Run(Task.Status status)
		{
			DiscordRpc.RichPresence rpc = new DiscordRpc.RichPresence();
			rpc.details = "Waiting...";
			rpc.state = "In Main Menu";
			rpc.startTimestamp = 0L;
			rpc.endTimestamp = 0L;
			rpc.largeImageKey = "official_level";

			TimeSpan span;
			StringBuilder dots;
			double totalTime = 0;
			while (true)
			{
				dots = new StringBuilder();
				dots.Append('.', 1 + ((int)(totalTime * 2) % 3));

				totalTime += Time.deltaTime;

				span = TimeSpan.FromSeconds(totalTime);
				status.SetText($"You have been waiting for {Colors.red.ToFormattedHex()}{span.Hours:000}:{span.Minutes:00}:{span.Seconds:00}[/c][-] {dots}\n{Colors.gray.ToFormattedHex()}There is nothing of interest here (really)");
				status.SetProgress((uint)span.Milliseconds, 1000);

				DiscordRpc.UpdatePresence(ref rpc);
				yield return null;
			}
		}
	}
}
