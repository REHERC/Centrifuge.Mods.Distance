using System.IO;

namespace Solution.Task.PostBuild.Tasks
{
    public static class IncludeDefaultContent
    {
        public static void Run(DirectoryInfo application, DirectoryInfo directory)
        {
            // Get the Build directory
            DirectoryInfo build = new DirectoryInfo(Path.Combine(directory.FullName, "Build"));

            // Get the Build directory
            DirectoryInfo common = new DirectoryInfo(Path.Combine(build.FullName, "Common"));


        }
    }
}
