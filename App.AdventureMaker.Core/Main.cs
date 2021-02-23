using App.AdventureMaker.Core.Forms;
using System.Globalization;
using System.Threading;

namespace App.AdventureMaker.Core
{
	public static class Main
	{
		public static void Start(string platformID)
		{
			CultureInfo english = new CultureInfo("en-US");

			Thread.CurrentThread.CurrentCulture = english;
			Thread.CurrentThread.CurrentUICulture = english;

			CultureInfo.CurrentCulture = english;
			CultureInfo.CurrentUICulture = english;

			Styles.ApplyAll();

			RuntimePatcher.AutoPatch();

			new Eto.Forms.Application(platformID).Run(new MainWindow());
		}
	}
}