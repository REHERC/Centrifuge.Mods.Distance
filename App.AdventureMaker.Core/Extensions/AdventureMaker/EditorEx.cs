#pragma warning disable RCS1110

using App.AdventureMaker.Core.Interfaces;
using Distance.AdventureMaker.Common.Models;
using System.IO;

public static class EditorEx
{
	public static DirectoryInfo ResourcesPath(this IEditor<CampaignFile> editor)
	{
		return new DirectoryInfo(Path.Combine(editor.CurrentFile.Directory.FullName, "resources"));
	}

	public static FileInfo GetResourceFile(this IEditor<CampaignFile> editor, string path)
	{
		return new FileInfo(Path.Combine(editor.ResourcesPath().FullName, path));
	}
}