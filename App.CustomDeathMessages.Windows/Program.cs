using App.CustomDeathMessages.Core.Forms;
using Eto;
using Eto.Forms;
using System;
using System.Globalization;
using System.Threading;

namespace App.CustomDeathMessages.Windows
{
	internal static class Program
	{
		[STAThread]
		private static void Main()
		{
			CultureInfo english = new CultureInfo("en-US");
			Thread.CurrentThread.CurrentCulture = english;
			Thread.CurrentThread.CurrentUICulture = english;

			CultureInfo.CurrentCulture = english;
			CultureInfo.CurrentUICulture = english;

			new Application(Platforms.WinForms).Run(new MainWindow());
		}
	}
}
