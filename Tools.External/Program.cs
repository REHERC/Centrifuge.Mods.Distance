using System;
using System.Windows.Forms;

namespace Tools.External
{
	internal static class Program
	{
		internal static ToolManager ToolManager { get; set; }

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		internal static void Main()
		{
			Control.CheckForIllegalCrossThreadCalls = false;

			ToolManager = new ToolManager();
			ToolManager.LoadTools();

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Forms.MainWindow());
		}
	}
}
