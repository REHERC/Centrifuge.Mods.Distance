using System;
using System.IO;
using System.Linq;

namespace Solution.Task.PostBuild.Tasks
{
	public static class PackageAIO
	{
		public static void Run(DirectoryInfo _, DirectoryInfo directory)
		{
			Console.WriteLine("Creating all-in-one build ...");

			// Get the Build directory
			DirectoryInfo build = new DirectoryInfo(Path.Combine(directory.FullName, "Build"));

			DirectoryInfo aio = new DirectoryInfo(Path.Combine(build.FullName, "Distance All In One"));

			string[] build_exclusion = new string[2]
			{
				"Common",
				aio.Name
			};

			// Get only mod folders
			DirectoryInfo[] directories = build.GetDirectories().Where(d => d.GetDirectories().Where(x => string.Equals(x.Name, "Centrifuge", StringComparison.InvariantCultureIgnoreCase)).Any()).ToArray();

			foreach (DirectoryInfo mod_directory in from x in directories where build_exclusion.All(y => !string.Equals(x.Name, y, StringComparison.InvariantCultureIgnoreCase)) select x)
			{
				Console.WriteLine($"Adding \"{mod_directory.Name}\" ...");

				mod_directory.CopyTo(aio, true);

				Console.WriteLine($"Mod added!");
			}

			Console.WriteLine($"All-in-one build complete!");
		}
	}
}
