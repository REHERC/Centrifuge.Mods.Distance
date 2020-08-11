using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            // Scan for mod folders and cache them
            GetContentDirectories(directory, out List<DirectoryInfo> content);

            // Read mod.json manifests
            GetModManifests(content, out List<AdvancedModManifest> manifests);

            // Read the readme template file and css style
            ReadFile(application, "Data/readme_template.html", out string template);
            ReadFile(application, "Data/readme_template.css", out string css);

            foreach (AdvancedModManifest manifest in manifests)
            {
                Console.WriteLine($"Generating readme file for \"{manifest.FriendlyName}\"");

                try
                {
                    string readme = template.FormatWith(manifest).FormatWith( new { DocumentStyle = css });
                    FileInfo readme_file = new FileInfo($@"{directory.FullName}\Build\{manifest.BuildPath}\Readme {manifest.FriendlyName}.html");

                    if (readme_file.Exists)
                    {
                        readme_file.Delete();
                    }

                    File.WriteAllText(readme_file.FullName, readme);

                    Console.WriteLine($"Readme file created at \"{readme_file.FullName}\"");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }

            return 0;
        }

        private static void ReadFile(DirectoryInfo application, string path, out string content)
        {
            content = string.Empty;

            FileInfo file = new FileInfo(Path.Combine(application.FullName, path));

            if (file.Exists)
            {
                content = File.ReadAllText(file.FullName, Encoding.UTF8);
            }
        }

        private static void GetModManifests(List<DirectoryInfo> content, out List<AdvancedModManifest> manifests)
        {
            manifests = new List<AdvancedModManifest>();

            foreach (DirectoryInfo directory in content)
            {
                FileInfo manifest_file = new FileInfo(Path.Combine(directory.FullName, @"Mod\mod.json"));

                if (manifest_file.Exists)
                {
                    AdvancedModManifest manifest = JsonConvert.DeserializeObject<AdvancedModManifest>(File.ReadAllText(manifest_file.FullName));

                    if (manifest != null)
                    {
                        manifest.ModDirectory = GetModDirectory(directory);
                        GetCentrifugeVersion(manifest.ModDirectory, out string version);
                        manifest.CentrifugeVersion = version;

                        manifests.Add(manifest);
                    }
                }
            }
        }

        internal static void GetCentrifugeVersion(DirectoryInfo directory, out string version)
        {
            version = "Not found";

            FileInfo reactor = new FileInfo(Path.Combine(directory.FullName, $@"bin\{configuration}\Reactor.API.dll"));

            if (reactor.Exists)
            {
                FileVersionInfo reactor_version = FileVersionInfo.GetVersionInfo(reactor.FullName);

                version = $"Centrifuge v{reactor_version.ProductVersion}";
            }
        }

        internal static DirectoryInfo GetModDirectory(DirectoryInfo directory)
        {
            if (directory.FullName.EndsWith(".Content", StringComparison.InvariantCultureIgnoreCase))
            {
                string path = directory.FullName;

                DirectoryInfo mod_directory = new DirectoryInfo(path.Substring(0, path.Length - 8));

                if (mod_directory.Exists)
                {
                    return mod_directory;
                }
            }

            return null;
        }

        internal static void GetContentDirectories(DirectoryInfo directory, out List<DirectoryInfo> directories)
        {
            directories = new List<DirectoryInfo>();

            foreach (DirectoryInfo subdirectory in directory.GetDirectories())
            {
                if (string.Equals(subdirectory.Name, "Build", StringComparison.InvariantCultureIgnoreCase) || subdirectory.Name.ToLower().EndsWith(".Content"))
                {
                    continue;
                }

                DirectoryInfo content = new DirectoryInfo($"{subdirectory.FullName}.Content");

                if (content.Exists)
                {
                    directories.Add(content);
                }
            }
        }
    }
}
