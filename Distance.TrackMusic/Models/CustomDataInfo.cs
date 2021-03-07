using System;
using System.Collections.Generic;

namespace Distance.TrackMusic.Models
{
	public class CustomDataInfo
	{
		public const string CustomDataPrefix = "CustomData:";

		public Type Type;
		public Type InnerType;

		public string SubPrefix { get; set; } = string.Empty;

		public string Prefix => CustomDataPrefix + SubPrefix;
		public static Dictionary<Type, CustomDataInfo> Infos = new Dictionary<Type, CustomDataInfo>();
		public static Dictionary<Type, Dictionary<Type, CustomDataInfo>> DeepInfos = new Dictionary<Type, Dictionary<Type, CustomDataInfo>>();

		public CustomDataInfo(Type type, Type innerType, string prefix)
		{
			Type = type;
			InnerType = innerType;
			SubPrefix = prefix;
		}

		public void Register()
		{
			RegisterStatic(this);
		}

		public static void RegisterStatic(CustomDataInfo info)
		{
			Mod.Instance.Logger.Info($"Registering {info.Type} {info.InnerType} {info.SubPrefix}");

			if (!Infos.ContainsKey(info.Type))
			{
				Infos.Add(info.Type, info);
			}

			if (!DeepInfos.ContainsKey(info.InnerType))
			{
				DeepInfos.Add(info.InnerType, new Dictionary<Type, CustomDataInfo>());
			}

			var innerDict = DeepInfos[info.InnerType];

			if (!innerDict.ContainsKey(info.Type))
			{
				innerDict.Add(info.Type, info);
			}
		}

		public static CustomDataInfo GetInfo(Type type)
		{
			Infos.TryGetValue(type, out CustomDataInfo info);
			return info;
		}

		public static CustomDataInfo GetInfo(Type type, Type innerType)
		{
			DeepInfos.TryGetValue(innerType, out Dictionary<Type, CustomDataInfo> dict);

			if (dict == null)
			{
				return null;
			}

			dict.TryGetValue(type, out CustomDataInfo info);
			return info;
		}

		public static string GetPrefix<T>()
		{
			return GetInfo(typeof(T)).Prefix;
		}
	}
}
