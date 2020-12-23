namespace Distance.TrackMusic.Models
{
	public abstract class CustomData<T>
	{
		public CustomDataInfo GetInfo()
		{
			return CustomDataInfo.GetInfo(GetType());
		}

		public string GetPrefix()
		{
			return GetInfo().Prefix;
		}

		public abstract string StringFromObject(T obj);

		public abstract void StringToObject(T obj, string str);

		public string DataStringFromObject(T obj)
		{
			var prefix = GetPrefix();
			var distanceStr = StringFromObject(obj);

			if (!distanceStr.StartsWith(prefix))
			{
				return null;
			}

			return distanceStr.Substring(prefix.Length);
		}
		public void DataStringToObject(T obj, string str)
		{
			var prefix = GetPrefix();
			StringToObject(obj, prefix + str);
		}

		public abstract bool ReadDataString(string data);

		public abstract string WriteDataString();

		public bool ReadObject(T obj)
		{
			var str = DataStringFromObject(obj);

			if (str == null)
			{
				return false;
			}

			return ReadDataString(str);
		}

		public void WriteObject(T obj)
		{
			var str = WriteDataString();
			DataStringToObject(obj, str);
		}
	}
}
