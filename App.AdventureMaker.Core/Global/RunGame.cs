using App.AdventureMaker.Core.Forms;
using App.AdventureMaker.Core.Interfaces;
using Distance.AdventureMaker.Common.Models;
using Eto.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using static Constants;
using static Distance.AdventureMaker.Common.CommandLine;
using static System.String;
using static Utils;

namespace App.AdventureMaker.Core
{
	public static class RunGame
	{
		public static void Run(IEditor<CampaignFile> editor)
		{
			switch (AppSettings.Instance.PreviewModeRunMethod)
			{
				case 0:
					string url = DISTANCE_STEAM_PROTOCOL_HANDLER_URL + Uri.EscapeDataString(ArgumentList(editor)).Replace("/", "%2F");

					ShellOpen(url);
					break;
				case 1:
					if (IsNullOrWhiteSpace(AppSettings.Instance.GameExe))
					{
						if (Messages.GameExeNotSetDialog() == DialogResult.Yes)
						{
							new SettingsWindow(0).ShowModal();
						}
					}
					else if (File.Exists(AppSettings.Instance.GameExe))
					{
						try
						{
							CreateSteamAppIdFile();
							Execute(AppSettings.Instance.GameExe, ArgumentList(editor));
						}
						catch (Exception)
						{
							Messages.GameExeErrorDialog();
						}
					}
					else
					{
						Messages.MissingGameExeDialog();
					}
					break;
			}
		}

		public static string ArgumentList(IEditor<CampaignFile> editor)
		{
			List<string> arguments = new List<string>();

			AddCommand(arguments, "-console");
			AddCommand(arguments, "-windowed");
			AddCommand(arguments, $"/{CLARG_FLG_PREVIEW_MODE}");
			AddKeyValue(arguments, CLARG_VAL_CAMPAIGN_PROJECT_FILE, editor.CurrentFile.FullName);

			// TODO: Send proper agrument set to the game's exe file to connect remotely to the tool
			// For remote access (see logs from the game in the tool and stuff)
			/*if (AppSettings.Instance.EnableRcon)
			{
				AddKeyValue(arguments, CLARG_VAL_REMOTE_CONSOLE_PORT, AppSettings.Instance.RconPort.ToString());
			}*/

			return Join(' ', arguments.ToArray());
		}

		private static void AddCommand(List<string> args, string command, params string[] parameters)
		{
			AddString(args, command);
			AddStrings(args, parameters);
		}

		private static void AddString(List<string> args, string value)
		{
			args.Add($"{value.Replace("\"", "\\\"")}");
		}

		private static void AddKeyValue(List<string> args, string key, object value)
		{
			char delim = StringDelimiter();

			string valueProcessed = value.ToString();

			if (value.GetType() == typeof(string))
			{
				valueProcessed = delim + valueProcessed.Replace("\"", "\\\"") + delim;
			}

			string element = $"/{key}:{valueProcessed}";

			args.Add(element);
		}

		private static void AddStrings(List<string> args, params string[] values)
		{
			char delim = StringDelimiter();

			foreach (string param in values)
			{
				args.Add($"{delim}{param}{delim}");
			}
		}

		private static char StringDelimiter()
		{
			return '"';

			/*if (EtoEnvironment.Platform.IsLinux)
			{
				return '\'';
			}
			else if (EtoEnvironment.Platform.IsWindows)
			{
				return '"';
			}

			return '\0';*/
		}

		// When starting the game from its exe file, it usually closes immediately and
		// restarts with steam... the problem is that by doing this, all startup
		// arguments are "lost"
		// However, if there's a file called "steam_appid.txt" near the game exe file
		// the game will start normally without a problem
		// This function ensures the presence of that file
		public static void CreateSteamAppIdFile()
		{
			FileInfo Executable = new FileInfo(AppSettings.Instance.GameExe);
			FileInfo Manifest = new FileInfo(Path.Combine(Executable.Directory.FullName, STEAM_APPID_FILE));

			if (Executable.Exists && !Manifest.Exists)
			{
				File.WriteAllText(Manifest.FullName, DISTANCE_STEAM_APPID);
			}
		}
	}
}
