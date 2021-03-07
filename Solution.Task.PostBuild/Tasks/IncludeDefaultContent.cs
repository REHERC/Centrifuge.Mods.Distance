using System;
using System.IO;
using System.Linq;

namespace Solution.Task.PostBuild.Tasks
{
	public static class IncludeDefaultContent
	{
		public static void Run(DirectoryInfo _, DirectoryInfo directory)
		{
			Console.WriteLine("Including common content from \"Build/Common\" ...");

			// Get the Build directory
			DirectoryInfo build = new DirectoryInfo(Path.Combine(directory.FullName, "Build"));

			// Get the Build directory
			DirectoryInfo common = new DirectoryInfo(Path.Combine(build.FullName, "Common"));

			string[] build_exclusion = new string[1]
			{
				common.Name
			};

			// Get only mod folders
			DirectoryInfo[] directories = build.GetDirectories().Where(d => d.GetDirectories().Any(x => string.Equals(x.Name, "Centrifuge", StringComparison.InvariantCultureIgnoreCase))).ToArray();

			foreach (DirectoryInfo mod_directory in from x in directories where build_exclusion.All(y => !string.Equals(x.Name, y, StringComparison.InvariantCultureIgnoreCase)) select x)
			{
				Console.WriteLine($"Copying content to \"Build/{mod_directory.Name}\" ...");

				common.CopyTo(mod_directory, true);

				Console.WriteLine("Content copied successfully!");
			}
		}
	}
}
