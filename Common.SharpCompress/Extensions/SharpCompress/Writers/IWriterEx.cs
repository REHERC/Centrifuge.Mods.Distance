#pragma warning disable RCS1110, IDE0063

using SharpCompress.Writers;
using System.IO;

public static class IWriterEx
{
	public static void CreateEntry(this IWriter writer, string filename, string data)
	{
		using (Stream stream = data.GetStream())
		{
			writer.Write(filename, stream);
		}
	}
}