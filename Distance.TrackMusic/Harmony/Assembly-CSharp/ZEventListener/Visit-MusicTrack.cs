using Centrifuge.Distance.Data;
using Centrifuge.Distance.Game;
using Distance.TrackMusic.Models;
using HarmonyLib;
using System;
using System.IO;

namespace Distance.TrackMusic.Harmony
{
	[HarmonyPatch(typeof(ZEventListener), "Visit")]
	internal static class ZEventListener__Visit___MusicTrack
	{
		[HarmonyPrefix]
		internal static bool Prefix(ZEventListener __instance, IVisitor visitor)
		{
			Mod mod = Mod.Instance;
			if (!(visitor is NGUIComponentInspector))
			{
				return true;
			}

			NGUIComponentInspector inspector = visitor as NGUIComponentInspector;

			if (!__instance.eventName_.StartsWith(CustomDataInfo.GetPrefix<MusicTrack>()))
			{
				return true;
			}

			visitor.Visit("eventName_", ref __instance.eventName_, false, null);
			visitor.Visit("delay_", ref __instance.delay_, false, null);

			var isEditing = inspector.isEditing_;

			var data = mod.Variables.CachedMusicTrack.GetOrCreate(__instance, () => new MusicTrack());

			if (data.LastWrittenData != __instance.eventName_)
			{
				data.ReadObject(__instance);
				data.LastWrittenData = __instance.eventName_;
				data.EmbedFile = (data.Embedded.Length > 0 ? "Embedded" : "");
				data.LastWritten = data.Clone();
			}
			else if (!isEditing)
			{
				var anyChanges = false;
				var old = data.LastWritten;

				if (data.Name != old.Name || data.DownloadUrl != old.DownloadUrl || data.FileType != old.FileType)
				{
					anyChanges = true;
				}

				if (data.EmbedFile != old.EmbedFile)
				{
					var newRef = data.EmbedFile;
					if (newRef?.Length == 0)
					{
						data.Embedded = new byte[0];
						anyChanges = true;
					}
					else
					{
						try
						{
							newRef = newRef.Trim('"', '\'');
							var extension = Path.GetExtension(newRef);
							var file = FileEx.ReadAllBytes(newRef);
							data.Embedded = file ?? throw new Exception("Missing file");
							data.FileType = extension;
							data.DownloadUrl = "";
							anyChanges = true;
						}
						catch (Exception e)
						{
							data.Embedded = new byte[0];
							data.FileType = ".mp3";
							anyChanges = true;

							MessageBox.Create($"Failed to embed {newRef} because {e}", "TRACK MUSIC ERROR")
							   .SetButtons(MessageButtons.Ok)
							   .Show();

							Mod.Instance.Logger.Error($"Failed to embed {newRef} because {e}");
						}
					}
				}
				if (anyChanges)
				{
					data.FileLocation = null;
					data.Attempted = false;
					data.EmbedFile = (data.Embedded.Length > 0 ? "Embedded" : "");
					data.NewVersion();
					data.WriteObject(__instance);
					data.LastWrittenData = __instance.eventName_;
					data.LastWritten = data.Clone();
					var lastTrackName = mod.Variables.CurrentTrackName;

					if (lastTrackName == old.Name)
					{
						mod.SoundPlayer.StopCustomMusic();
					}

					mod.SoundPlayer.DownloadAllTracks();

					if (lastTrackName == data.Name || mod.SoundPlayer.GetMusicChoiceValue(G.Sys.LevelEditor_.WorkingSettings_.gameObject, "Level") == data.Name)
					{
						mod.SoundPlayer.PlayTrack(data.Name, 0f);
					}
				}
			}

			visitor.Visit("Name", ref data.Name, null);
			visitor.Visit("Type", ref data.FileType, null);
			visitor.Visit("Embed File", ref data.EmbedFile, mod.Variables.MusicTrackOptions);

			visitor.VisitAction("Select File", () =>
			{
				var dlgOpen = new System.Windows.Forms.OpenFileDialog
				{
					Filter = "Music file (*.mp3, *.wav, *.aiff)|*.mp3;*.wav;*.aiff|All Files (*.*)|*.*",
					SupportMultiDottedExtensions = true,
					RestoreDirectory = true,
					Title = "Select a music file",
					CheckFileExists = true,
					CheckPathExists = true
				};

				if (dlgOpen.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					data.EmbedFile = dlgOpen.FileName;
				}
			}, mod.Variables.MusicTrackButtonOptions);

			visitor.Visit("Download URL", ref data.DownloadUrl, null);

			var Error = data.GetError();

			if (Error == null)
			{
				Error = "None".Colorize(Colors.white);
			}
			else
			{
				Error = Error.Colorize(Colors.red);
			}
			//visitor.Visit("Error", ref Error, null);

			visitor.VisualLabel($"Error: {Error}".Colorize(Colors.white));

			return false;
		}

		[HarmonyPostfix]
		internal static void Postfix(ZEventListener __instance)
		{
			if (!__instance.eventName_.StartsWith(CustomDataInfo.GetPrefix<MusicTrack>()))
			{
				return;
			}

			Mod.Instance.Variables.CachedMusicTrack.GetOrCreate(__instance, () => MusicTrack.FromObject(__instance));
		}
	}
}
