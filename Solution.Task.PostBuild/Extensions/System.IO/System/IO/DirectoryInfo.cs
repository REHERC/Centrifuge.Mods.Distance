using Microsoft.VisualBasic.FileIO;
using System;
using System.IO;

public static class DirectoryInfoExtensions
{
    public static void CopyTo(this DirectoryInfo source, DirectoryInfo destination, bool overwrite = false)
    {
        if (source.Exists)
        {
            destination.CreateIfDontExist();
            try
            {
                FileSystem.CopyDirectory(source.FullName, destination.FullName, overwrite);
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occured while copying the contents from \"{source.FullName}\" to \"{destination.FullName}\"");
                Console.WriteLine(e);
            }
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
