#if APP
#pragma warning disable IDE0063
#endif

using Newtonsoft.Json;
using System;
using System.IO;

public static class Json
{
	#region Save
	public static void Save<TYPE>(string file, TYPE data, bool overwrite = false)
		=> Save(new FileInfo(file), data, overwrite);

	public static void Save<TYPE>(FileInfo file, TYPE data, bool overwrite = false)
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

	public static TYPE Load<TYPE>(FileInfo file) where TYPE : new()
	{

		if (file.Exists)
		{
			TYPE result = new TYPE();

			using (StreamReader streamReader = new StreamReader(file.FullName))
			{
				result = JsonConvert.DeserializeObject<TYPE>(streamReader.ReadToEnd());
			}

			return result;
		}
		else
		{
			throw new FileNotFoundException("The file does not exist", file.FullName);
		}
	}
	#endregion
}
