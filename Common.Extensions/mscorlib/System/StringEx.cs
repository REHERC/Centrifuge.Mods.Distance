#pragma warning disable RCS1110

using System.IO;
using System.Text;

public static class StringEx
{
    public static Stream GetStream(this string input)
    {
        return new MemoryStream(Encoding.UTF8.GetBytes(input));
    }
}