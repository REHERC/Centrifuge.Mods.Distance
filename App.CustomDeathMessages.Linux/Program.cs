using App.CustomDeathMessages.Core.Forms;
using Eto;
using Eto.Forms;
using System;

namespace App.CustomDeathMessages.Linux
{
	class Program
	{
		[STAThread]
		static void Main()
		{
			new Application(Platforms.Gtk).Run(new MainWindow());
		}
	}
}
