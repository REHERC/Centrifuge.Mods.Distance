#pragma warning disable RCS1110, IDE0063
using SharpCompress.Archives;
using System.Collections.Generic;
using System.Linq;

public static class IArchiveEx
{
    public static Dictionary<string, IArchiveEntry> GetFileEntries(this IArchive archive)
    {
        return archive.Entries
        .Where(entry => !entry.IsDirectory)
        .ToDictionary(entry => entry.Key);
    }
}