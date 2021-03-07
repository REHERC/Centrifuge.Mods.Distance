#pragma warning disable RCS1110
using System.IO;

public static class Json<TYPE> where TYPE : new()
{
	public static void Save(FileInfo file, TYPE data, bool overwrite = true)
		=> Json.Save(file, data, overwrite);

	public static void Save(string file, TYPE data, bool overwrite = true)
		=> Json.Save(file, data, overwrite);

	public static TYPE Load(FileInfo file)
		=> Json.Load<TYPE>(file);

	public static TYPE Load(FileInfo file, TYPE @default)
		=> Json.Load(file, @default);

	public static TYPE Load(string file)
		=> Json.Load<TYPE>(file);

	public static TYPE Load(string file, TYPE @default)
		=> Json.Load(file, @default);

	public static TYPE GetOrCreate(string file, TYPE @default)
		=> Json.GetOrCreate(file, @default);

	public static TYPE GetOrCreate(FileInfo file, TYPE @default)
		=> Json.GetOrCreate(file, @default);
}