#pragma warning disable RCS1110
using Eto.Drawing;
using System.IO;
using System.Linq;

public static class Resources
{
	public static string ResourcesRoot => typeof(Resources).Assembly.GetName().Name;

	public static Stream GetStream(string name)
	{
		return typeof(Resources).Assembly.GetManifestResourceStream(name);
	}

	public static Icon GetIcon(string iconName, int size = -1)
	{
		Icon ico = Icon.FromResource($"{ResourcesRoot}.Resources.Icons.{iconName}");

		if (size == -1)
		{
			return ico;
		}
		else
		{
			Bitmap bmp = (from frame in ico.Frames where Equals(frame.PixelSize, new Size(size, size)) select frame.Bitmap).FirstOrDefault();

			return new Icon(1, bmp ?? ico.Frames.ToArray()[0].Bitmap);
		}
	}

	public static Bitmap GetImage(string imageName)
	{
		return Bitmap.FromResource($"{ResourcesRoot}.Resources.Images.{imageName}");
	}

	public static string GetText(string name)
	{
		string resourceName = $"{ResourcesRoot}.Resources.Text.{name}";

		using Stream memory = GetStream(resourceName);
		using StreamReader reader = new StreamReader(memory);
		return reader.ReadToEnd();
	}
}