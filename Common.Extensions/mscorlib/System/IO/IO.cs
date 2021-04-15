#pragma warning disable RCS1110
using System.IO;

public static class IO
{
	// Return an array of path delimiter characters
	public static char[] GetSeparatorChars()
	{
		return new char[3] { '/', Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar };
	}
}