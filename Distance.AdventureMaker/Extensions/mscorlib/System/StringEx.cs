public static class StringExtensions
{
	public static string NormalizePath(this string input)
	{
		return input.Normalize().Replace('\\', '/');
	}
}
