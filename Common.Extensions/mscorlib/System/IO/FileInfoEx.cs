#pragma warning disable RCS1110
using System;
using System.IO;
using System.Linq;
using static IO;

public static class FileInfoEx
{
	// Returns the number of folders specified in the path
	// Example: C:\ABC\DEF.txt => 3 | C:\ABC\DEF\GHI\JKL.txt => 5
	public static int GetFolderLength(this FileInfo file)
	{
		return file.FullName.Split(GetSeparatorChars()).Count(item => !string.IsNullOrWhiteSpace(item));
	}

	// Returns a string representing a folder path with its beginning n folders "cut" from the string
	// Example: C:\ABC\DEF\GHI.txt;2 => DEF\GHI.txt
	public static string CutFolderStart(this FileInfo file, int cut)
	{
		return string.Join(Path.DirectorySeparatorChar, file.FullName.Split(GetSeparatorChars()).Skip(cut));
	}

	public static bool PathEquals(this FileInfo file, FileInfo other)
	{
		return Path.GetFullPath(file.FullName).Equals(Path.GetFullPath(other.FullName), StringComparison.InvariantCultureIgnoreCase);
	}
}