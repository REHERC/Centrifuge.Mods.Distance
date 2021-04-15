#pragma warning disable RCS1110
using System;
using System.IO;
using System.Linq;
using static IO;

public static class DirectoryInfoEx
{
	public static bool FileExistsInside(this DirectoryInfo directory, FileInfo file, SearchOption searchOption = SearchOption.AllDirectories)
	{
		return directory.GetFiles(file.Name, searchOption).Any(item => string.Equals(item.FullName, file.FullName, StringComparison.InvariantCultureIgnoreCase));
	}

	// Returns the number of folders specified in the path
	// Example: C:\ABC\DEF => 3 | C:\ABC\DEF\GHI\JKL => 5
	public static int GetFolderLength(this DirectoryInfo directory)
	{
		return directory.FullName.Split(GetSeparatorChars()).Count(item => !string.IsNullOrWhiteSpace(item));
	}

	// Returns a string representing a folder path with its beginning n folders "cut" from the string
	// Example: C:\ABC\DEF\GHI;2 => DEF\GHI
	public static string CutFolderStart(this DirectoryInfo directory, int cut)
	{
		return string.Join(Path.DirectorySeparatorChar, directory.FullName.Split(GetSeparatorChars()).Skip(cut));
	}

	public static void CreateIfDoesntExist(this DirectoryInfo directory)
	{
		if (!directory.Exists)
		{
			directory.Create();
		}
	}

	public static bool PathEquals(this DirectoryInfo file, DirectoryInfo other)
	{
		return Path.GetFullPath(file.FullName).Equals(Path.GetFullPath(other.FullName), StringComparison.InvariantCultureIgnoreCase);
	}
}