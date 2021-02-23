using App.AdventureMaker.Core.Forms;
using App.AdventureMaker.Core.Interfaces;
using Distance.AdventureMaker.Common.Models;
using Eto;
using Eto.Forms;
using System;
using System.Collections.Generic;
using System.IO;
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
					string url = $"{Constants.DISTANCE_STEAM_PROTOCOL_HANDLER_URL}{Uri.EscapeUriString(ArgumentList(editor)).Replace("/", "%2F")}";

					ShellOpen(url);
					break;
				case 1:
					if (IsNullOrWhiteSpace(AppSettings.Instance.GameExe))
					{
						if (Messages.GameExeNotSet() == DialogResult.Yes)
						{
							new SettingsWindow(0).ShowModal();
						}
					}
					else if (File.Exists(AppSettings.Instance.GameExe))
					{
						try
						{
							Execute(AppSettings.Instance.GameExe, ArgumentList(editor));
						}
						catch (Exception)
						{
							Messages.GameExeError();
						}
					}
					else
					{
						Messages.MissingGameExe();
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

			if (AppSettings.Instance.EnableRcon)
			{
				AddKeyValue(arguments, CLARG_VAL_REMOTE_CONSOLE_PORT, AppSettings.Instance.RconPort.ToString());
			}

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
	}
}
