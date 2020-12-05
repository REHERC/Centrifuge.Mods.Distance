using App.CustomDeathMessages.Core.Forms;
using Eto;
using Eto.Forms;
using System;

namespace App.CustomDeathMessages.Windows
{
	class Program
	{
		[STAThread]
		static void Main()
		{
			new Application(Platforms.WinForms).Run(new MainForm());
		}
	}
}
