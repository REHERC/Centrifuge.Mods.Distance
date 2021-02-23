using static App.AdventureMaker.Core.Main;
using Eto;
using System;

namespace App.AdventureMaker.Windows
{
	internal class Program
	{
		[STAThread]
		private static void Main()
		{
			Start(Platforms.WinForms);
		}
	}
}
