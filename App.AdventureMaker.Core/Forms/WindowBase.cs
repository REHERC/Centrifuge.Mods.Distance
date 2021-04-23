#pragma warning disable RCS1110

public static class WindowBase
{
	private const string BADGE_FORMAT = "[{0}]";
	private const string BADGE_SEPARATOR = " | ";

	private const string BADGE_PREVIEW = "DEVELOPMENT BUILD";

	private static readonly string[] BADGES = new string[]
	{
		#if PREVIEW
		BADGE_PREVIEW,
		#endif
	};

	public static string WindowBadges
	{
		get
		{
			return string.Format(BADGE_FORMAT, string.Join(BADGE_SEPARATOR, BADGES));
		}
	}

	public static string Name(string value)
	{
		string badges = WindowBadges;
		badges += badges.Length > 0 ? " - " : string.Empty;

		return badges + value;
	}
}

public abstract class Form : Eto.Forms.Form
{
	public new string Title
	{
		get => base.Title;
		set => base.Title = WindowBase.Name(value);
	}
}

public abstract class Dialog : Eto.Forms.Dialog
{
	public new string Title
	{
		get => base.Title;
		set => base.Title = WindowBase.Name(value);
	}
}

public abstract class Dialog<T> : Eto.Forms.Dialog<T>
{
	public new string Title
	{
		get => base.Title;
		set => base.Title = WindowBase.Name(value);
	}
}