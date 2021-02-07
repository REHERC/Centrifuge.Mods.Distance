using App.AdventureMaker.Core.Forms;
using App.AdventureMaker.Core.Interfaces;
using Distance.AdventureMaker.Common.Models;
using System;
using System.Collections.Generic;
using System.IO;
using static Utils;

namespace App.AdventureMaker.Core
{
	public static class RunGame
	{
		public static void Run(IEditor<CampaignFile> editor)
		{
			switch (AppSettings.Instance.PreviewMode)
			{
				case 0:
					string url = $"{Constants.DISTANCE_STEAM_PROTOCOL_HANDLER_URL}{Uri.EscapeUriString(ArgumentList(editor))}";
					ShellOpen(url);
					break;
				case 1:
					if (String.IsNullOrWhiteSpace(AppSettings.Instance.GameExe))
					{
						if (Messages.GameExeNotSet() == Eto.Forms.DialogResult.Yes)
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
			AddCommand(arguments, "-campaign", editor.CurrentFile.FullName);

			return string.Join(' ', arguments.ToArray());
		}

		private static void AddCommand(List<string> args, string command, params string[] parameters)
		{
			AddSwitch(args, command);
			AddStrings(args, parameters);
		}

		private static void AddSwitch(List<string> args, string value)
		{
			args.Add($"{value.Replace("\"", "\\\"")}");
		}

		private static void AddStrings(List<string> args, params string[] values)
		{
			foreach (string param in values)
			{
				args.Add($"\"{param.Replace("\"", "\\\"")}\"");
			}
		}
	}
}
