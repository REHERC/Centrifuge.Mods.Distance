﻿using System.IO;

public static class Json<TYPE> where TYPE : new()
{
	public static void Save(FileInfo file, TYPE data, bool overwrite)
		=> Json.Save(file, data, overwrite);

	public static void Save(string file, TYPE data, bool overwrite)
		=> Json.Save(file, data, overwrite);

	public static TYPE Load(FileInfo file)
		=> Json.Load<TYPE>(file);

	public static TYPE Load(string file)
		=> Json.Load<TYPE>(file);
}