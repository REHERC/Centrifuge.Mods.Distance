#if APP
#pragma warning disable IDE0063
#endif
#pragma warning disable RCS1110
using Newtonsoft.Json;
using System;
using System.IO;

public static class Json
{
	#region Save
	public static void Save<TYPE>(string file, TYPE data, bool overwrite = true)
		=> Save(new FileInfo(file), data, overwrite);

	public static void Save<TYPE>(FileInfo file, TYPE data, bool overwrite = true)
	{
		if (file.Exists)
		{
			if (overwrite)
			{
				file.Delete();
			}
			else
			{
				throw new Exception($"File already exists: {file.FullName}");
			}
		}

		JsonSerializer serializer = new JsonSerializer()
		{
			NullValueHandling = NullValueHandling.Include
		};

		using (StreamWriter streamWriter = new StreamWriter(file.FullName))
		{
			using (JsonWriter jsonWriter = new JsonTextWriter(streamWriter)
			{
				Formatting = Formatting.Indented
			})
			{
				serializer.Serialize(jsonWriter, data);
			}
		}
	}
	#endregion
	#region Load
	public static TYPE Load<TYPE>(string file) where TYPE : new()
		=> Load<TYPE>(new FileInfo(file));

	public static TYPE Load<TYPE>(string file, TYPE @default) where TYPE : new()
		=> Load(new FileInfo(file), @default);

	public static TYPE Load<TYPE>(FileInfo file) where TYPE : new()
		=> Load(file, new TYPE());

	public static TYPE Load<TYPE>(FileInfo file, TYPE @default) where TYPE : new()
	{
		if (file.Exists)
		{
			try
			{
				using (StreamReader streamReader = new StreamReader(file.FullName))
				{
					return JsonConvert.DeserializeObject<TYPE>(streamReader.ReadToEnd());
				}
			}
			catch (Exception)
			{
				return @default;
			}
		}

		return @default;
		//else
		//{
		//	throw new FileNotFoundException("The file does not exist", file.FullName);
		//}
	}
	#endregion
	#region Get or Create
	public static TYPE GetOrCreate<TYPE>(string file, TYPE @default) where TYPE : new()
		=> GetOrCreate(new FileInfo(file), @default);

	public static TYPE GetOrCreate<TYPE>(FileInfo file, TYPE @default) where TYPE : new()
	{
		if (!file.Exists)
		{
			Save(file, @default);
		}

		return Load<TYPE>(file);
	}
	#endregion
}
