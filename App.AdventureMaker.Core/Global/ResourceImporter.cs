#pragma warning disable RCS1110

using App.AdventureMaker.Core.Interfaces;
using Distance.AdventureMaker.Common.Models;
using System;
using System.IO;

public static class ResourceImporter
{
	/* Imports the specified file into the editor project workspace
	 * Returns a file info pointing to the imported file 
	 */
	public static FileInfo ImportFile(IEditor<CampaignFile> editor, FileInfo file, string importDirectory, out string relativePath)
	{
		DirectoryInfo resources = editor.ResourcesPath();

		DirectoryInfo dir = GetPath(resources, file, importDirectory);

		relativePath = Path.Combine(dir.CutFolderStart(resources.GetFolderLength()), file.Name);
		FileInfo importPath = new FileInfo(Path.Combine(resources.FullName, relativePath));

		if (importPath.PathEquals(file))
		{
			return file;
		}

		importPath.Directory.CreateIfDoesntExist();

		file.CopyTo(importPath.FullName);

		return importPath;
	}

	/* This function receives a project directory and a file path
	 * If the file path is already located in the project directory,
	 * return the directory where that file is
	 * Otherwise generate a new directory path (random one using GUID or something random enough)
	 * The importDirectory value specifies the first part of the generated path
	 */
	public static DirectoryInfo GetPath(DirectoryInfo projectPath, FileInfo file, string importDirectory)
	{
		if (projectPath.FileExistsInside(file))
		{
			return file.Directory;
		}

		return new DirectoryInfo(Path.Combine(projectPath.FullName, GenerateDirectoryName(importDirectory)));
	}

	public static string GenerateDirectoryName(string path)
	{
		string guid = Guid.NewGuid().ToString("D");
		string date = DateTime.Now.Ticks.ToString();
		string root = !string.IsNullOrWhiteSpace(path) ? $"{path}/" : "";

		return $"{root}imported/{guid}_{date}";
	}
}