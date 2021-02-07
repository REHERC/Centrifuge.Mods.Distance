using System.Diagnostics;

public static class Utils
{
	public static Process ShellOpen(string url)
	{
		var process = new Process()
		{
			StartInfo = new ProcessStartInfo(url)
			{
				UseShellExecute = true
			},
		};

		process.Start();

		return process;
	}

	public static Process Execute(string command, string arguments)
	{
		var process = new Process()
		{
			StartInfo = new ProcessStartInfo(command, arguments)
			{
				UseShellExecute = false
			},
		};

		process.Start();

		return process;
	}
}
