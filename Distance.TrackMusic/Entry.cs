using Centrifuge.Distance.Game;
using Centrifuge.Distance.GUI.Controls;
using Centrifuge.Distance.GUI.Data;
using Distance.TrackMusic.Models;
using Events;
using Reactor.API.Attributes;
using Reactor.API.Interfaces.Systems;
using Reactor.API.Logging;
using Reactor.API.Runtime.Patching;
using System.Collections.Generic;
using UnityEngine;

namespace Distance.TrackMusic
{
	[ModEntryPoint("com.github.corecii/Distance.TrackMusic")]
	public class Mod : MonoBehaviour
	{
		public static Mod Instance { get; set; }

		public IManager Manager { get; set; }

		public ConfigurationLogic Config { get; set; }

		public VariablesLogic Variables { get; set; }

		public SoundPlayerLogic SoundPlayer { get; set; }

		public LevelEditorLogic LevelEditor { get; set; }

		public Log Logger { get; set; }

		public bool Enabled { get; set; } = true;

		public void Initialize(IManager manager)
		{
			Instance = this;
			Manager = manager;

			Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);

			Config = gameObject.AddComponent<ConfigurationLogic>();
			Variables = gameObject.AddComponent<VariablesLogic>();
			SoundPlayer = gameObject.AddComponent<SoundPlayerLogic>();
			LevelEditor = gameObject.AddComponent<LevelEditorLogic>();
			Logger = LogManager.GetForCurrentAssembly();

			MusicTrack.Info.Register();
			MusicChoice.Info.Register();

			DirectoryEx.CreateIfDoesNotExist("EditorMusic/");

			CreateSettingsMenu();

			RuntimePatcher.AutoPatch();
		}

		public void LateInitialize(IManager _)
		{
			PatchPostLoad(true);
		}

		private void CreateSettingsMenu()
		{
			MenuTree settingsMenu = new MenuTree("menu.mod.trackmusic", "Track Music Settings")
			{
				new IntegerSlider(MenuDisplayMode.Both, "setting:max_music_download_size", "MAXIMUM DOWNLOAD SIZE (MB)")
				.WithGetter(() => (int)Config.MaxMusicDownloadSizeMB)
				.WithSetter((x) => Config.MaxMusicDownloadSizeMB = x)
				.LimitedByRange(10, 150)
				.WithDefaultValue(30)
				.WithDescription("Set the maximum file size to download for online music downloading."),

				new IntegerSlider(MenuDisplayMode.Both, "setting:max_music_download_time", "MAXIMUM DOWNLOAD TIME (SECS)")
				.WithGetter(() => (int)Config.MaxMusicDownloadTimeSeconds)
				.WithSetter((x) => Config.MaxMusicDownloadTimeSeconds = x)
				.LimitedByRange(5, 60)
				.WithDefaultValue(15)
				.WithDescription("Set the amount of time after which the music download times out (gets canceled)."),

				new IntegerSlider(MenuDisplayMode.Both, "setting:max_level_load_time", "MAXIMUM LOAD TIME (SECS)")
				.WithGetter(() => (int)Config.MaxMusicLevelLoadTimeSeconds)
				.WithSetter((x) => Config.MaxMusicLevelLoadTimeSeconds = x)
				.LimitedByRange(5, 60)
				.WithDefaultValue(20)
				.WithDescription("Set the amount of time after which the level loads whether the music download is complete or not."),
			};

			Menus.AddNew(MenuDisplayMode.Both, settingsMenu, "TRACK MUSIC", "Settings for the Track Music mod.");
		}

		public void PatchPostLoad(bool subscribe)
		{
			var audioManager = G.Sys.AudioManager_;

			RemoveParticularSubscriber<Events.Level.PostLoad.Data>(audioManager);

			var list = G.Sys.AudioManager_.subscriberList_;

			var item = new StaticEvent<Events.Level.PostLoad.Data>.Subscriber(new StaticEvent<Events.Level.PostLoad.Data>.Delegate(data => G.Sys.AudioManager_.OnEventPostLoad(data)));

			list.Add(item);

			if (subscribe)
			{
				(item as IEventSubscriber)?.Subscribe();
			}
		}

		public void RemoveParticularSubscriber<T>(AudioManager component)
		{
			SubscriberList subscriberList_ = component?.subscriberList_;

			if (subscriberList_ is null)
			{
				return;
			}

			StaticEvent<T>.Delegate func;

			foreach (IEventSubscriber subscriber in new List<IEventSubscriber>(subscriberList_))
			{
				if (subscriber is StaticEvent<T>.Subscriber eventSubscriber)
				{
					func = eventSubscriber?.func_;

					if (func != null)
					{
						subscriberList_.Remove(subscriber);
					}
				}
			}
		}
	}
}