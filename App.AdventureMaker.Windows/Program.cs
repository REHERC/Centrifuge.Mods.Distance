using App.AdventureMaker.Core;
using App.AdventureMaker.Core.Forms;
using Eto;
using Eto.Forms;
using System;
using System.Globalization;
using System.Threading;

namespace App.AdventureMaker.Windows
{
	internal class Program
	{
		[STAThread]
		private static void Main()
		{
			CultureInfo english = new CultureInfo("en-US");
			Thread.CurrentThread.CurrentCulture = english;
			Thread.CurrentThread.CurrentUICulture = english;

			CultureInfo.CurrentCulture = english;
			CultureInfo.CurrentUICulture = english;

			Styles.ApplyAll();

			new Application(Platforms.WinForms).Run(new MainWindow());
		}
	}
}
