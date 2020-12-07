using Eto.Drawing;
using System.IO;

public static class Resources
{
	public static string ResourcesRoot => typeof(Resources).Assembly.GetName().Name;

	public static Stream GetStream(string name)
	{
		return typeof(Resources).Assembly.GetManifestResourceStream(name);
	}
	public static Icon GetIcon(string iconName)
	{
		return Icon.FromResource($"{ResourcesRoot}.Resources.Icons.{iconName}");
	}

	public static Bitmap GetImage(string iconName)
	{
		return Bitmap.FromResource($"{ResourcesRoot}.Resources.Bitmaps.{iconName}");
	}

	public static string GetText(string name)
	{
		string resourceName = $"{ResourcesRoot}.Resources.Text.{name}";

		using Stream memory = GetStream(resourceName);
		using StreamReader reader = new StreamReader(memory);
		return reader.ReadToEnd();
	}
}
