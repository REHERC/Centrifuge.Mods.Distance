using Microsoft.VisualBasic.FileIO;
using System.IO;

public static class DirectoryInfoExtensions
{
    public static void CopyTo(this DirectoryInfo source, DirectoryInfo destination, bool overwrite = false)
    {
        FileSystem.CopyDirectory(source.FullName, destination.FullName, overwrite);
    }
}
