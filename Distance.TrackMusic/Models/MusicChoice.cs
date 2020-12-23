using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Distance.TrackMusic.Models
{
	public class MusicChoice : CustomData<ZEventListener>
	{
		public static CustomDataInfo Info = new CustomDataInfo(typeof(MusicChoice), typeof(ZEventListener), "MusicChoice:");

		public override string StringFromObject(ZEventListener obj)
		{
			return obj.eventName_;
		}

		public override void StringToObject(ZEventListener obj, string str)
		{
			obj.eventName_ = str;
		}

		public Dictionary<string, MusicChoiceEntry> Choices = new Dictionary<string, MusicChoiceEntry>();

		[NonSerialized]
		public string LastWrittenData = null;

		[NonSerialized]
		public MusicChoice LastWritten = null;

		public override bool ReadDataString(string data)
		{
			JsonConvert.PopulateObject(data, this);
			return true;
		}
		public override string WriteDataString()
		{
			return JsonConvert.SerializeObject(this);
		}

		public MusicChoice Clone()
		{
			var dict = new Dictionary<string, MusicChoiceEntry>();
			dict.AddRange(Choices.Select(entry => new KeyValuePair<string, MusicChoiceEntry>(entry.Key, new MusicChoiceEntry(entry.Value.Track))));

			return new MusicChoice()
			{
				Choices = dict,
			};
		}

		public static MusicChoice FromObject(ZEventListener obj)
		{
			var newThis = new MusicChoice();
			var success = newThis.ReadObject(obj);
			return success ? newThis : null;
		}
	}
}
