using Microsoft.VisualBasic.FileIO;
using System;
using System.IO;

public static class DirectoryInfoExtensions
{
    public static void CopyTo(this DirectoryInfo source, DirectoryInfo destination, bool overwrite = false)
    {
        if (source.Exists)
        {
            FileSystem.CopyDirectory(source.FullName, destination.FullName, overwrite);
            destination.CreateIfDontExist();
        }
        else
        {
            Console.WriteLine($"Could not find directory \"{source.FullName}\"");
        }
    }

    public static void CreateIfDontExist(this DirectoryInfo directory)
    {
        if (!directory.Exists)
        {
            directory.Create();
        }
    }
}
