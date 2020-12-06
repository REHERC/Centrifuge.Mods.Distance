using Eto.Drawing;

public static class Resources
{
	public static string ResourcesRoot => typeof(Resources).Assembly.GetName().Name;

	public static Icon GetIcon(string iconName)
	{
		return Icon.FromResource($"{ResourcesRoot}.Icons.{iconName}");
	}
}
