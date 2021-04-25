#pragma warning disable RCS1110, IDE0063
using SharpCompress.Archives;
using System.IO;

public static class IArchiveEntryEx
{
    public static string GetText(this IArchiveEntry entry)
    {
        using (StreamReader reader = new StreamReader(entry.OpenEntryStream()))
        {
            return reader.ReadToEnd();
        }
    }
}