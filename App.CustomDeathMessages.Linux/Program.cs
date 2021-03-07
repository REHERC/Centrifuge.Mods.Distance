using App.CustomDeathMessages.Core.Forms;
using Eto;
using Eto.Forms;
using System;

namespace App.CustomDeathMessages.Linux
{
	internal static class Program
	{
		[STAThread]
		private static void Main()
		{
			new Application(Platforms.Gtk).Run(new MainWindow());
		}
	}
}
