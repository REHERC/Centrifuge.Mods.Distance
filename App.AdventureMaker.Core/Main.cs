using App.AdventureMaker.Core.Forms;
using Eto;
using System.Globalization;
using System.Threading;

namespace App.AdventureMaker.Core
{
	public static class Main
	{
		public static void Start()
		{
			CultureInfo english = new CultureInfo("en-US");

			Thread.CurrentThread.CurrentCulture = english;
			Thread.CurrentThread.CurrentUICulture = english;

			CultureInfo.CurrentCulture = english;
			CultureInfo.CurrentUICulture = english;

			Styles.ApplyAll();

			RuntimePatcher.AutoPatch();

			new Eto.Forms.Application(Platform.Detect).Run(new MainWindow());
		}

		public static string GetFullVersionString()
		{
			return $"V.{GetVersionString()} - Built on {typeof(Main).Assembly.GetBuildDate():g}";
		}

		public static string GetVersionString(int fieldCount = 4)
		{
			return typeof(Main).Assembly.GetName().Version.ToString(fieldCount);
		}
	}
}