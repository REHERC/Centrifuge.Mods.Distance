#pragma warning disable RCS1110, IDE0063
using SharpCompress.Archives.Zip;
using System.IO;

public static class ZipArchiveEntryEx
{
    public static string GetText(this ZipArchiveEntry entry)
    {
        using (StreamReader reader = new StreamReader(entry.OpenEntryStream()))
        {
            return reader.ReadToEnd();
        }
    }
}