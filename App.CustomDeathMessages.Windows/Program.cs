using App.CustomDeathMessages.Core.Forms;
using Eto;
using Eto.Forms;
using System;
using System.Globalization;
using System.Threading;

namespace App.CustomDeathMessages.Windows
{
	class Program
	{
		[STAThread]
		static void Main()
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
