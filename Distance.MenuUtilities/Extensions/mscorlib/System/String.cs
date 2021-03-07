#pragma warning disable RCS1110
using System.IO;

public static class StringExtensions
{
	public static string UniformPathName(this string source)
	{
		string result = source.ToLowerInvariant();

		result = result.Replace(Path.DirectorySeparatorChar, '/');
		result = result.Replace(Path.AltDirectorySeparatorChar, '/');
		result = result.Replace('\\', '/');

		return result;
	}
}