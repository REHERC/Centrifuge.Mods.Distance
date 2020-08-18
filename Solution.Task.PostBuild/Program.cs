using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Solution.Task.PostBuild
{
    static class Program
    {
        internal static string configuration;

        [STAThread]
        public static int Main(string[] args)
        {
            if (args.Length != 1)
            {
                return 1;
            }

            configuration = args[0];
            DirectoryInfo application = new DirectoryInfo(Application.StartupPath);
            DirectoryInfo directory = new DirectoryInfo(Directory.GetCurrentDirectory());

            // Create readme html files
            Tasks.MakeReadmeFiles.Run(application, directory);

            // Make an "All In One" mod package containg all mods
            Tasks.PackageAIO.Run(application, directory);

            // Include contents of Build/common into every mod build
            Tasks.IncludeDefaultContent.Run(application, directory);

            return 0;
        }

        internal static void ReadFile(DirectoryInfo application, string path, out string content)
        {
            content = string.Empty;

            FileInfo file = new FileInfo(Path.Combine(application.FullName, path));

            if (file.Exists)
            {
                content = File.ReadAllText(file.FullName, Encoding.UTF8);
            }
        }
    }
}
