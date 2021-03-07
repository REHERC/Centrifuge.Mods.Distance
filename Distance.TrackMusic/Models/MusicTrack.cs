using Distance.TrackMusic.Util;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Distance.TrackMusic.Models
{
	public class MusicTrack : CustomData<ZEventListener>
	{
		public static CustomDataInfo Info = new CustomDataInfo(typeof(MusicTrack), typeof(ZEventListener), "MusicTrack:");

		public static readonly string[] AllowedExtensions = new string[]
		{
			".mp3",
			".wav",
			".aiff"
		};

		public static readonly string[] AllowedDownloadSchemes = new string[]
		{
			"http",
			"https"
		};

		public override string StringFromObject(ZEventListener obj)
		{
			return obj.eventName_;
		}

		public override void StringToObject(ZEventListener obj, string str)
		{
			obj.eventName_ = str;
		}

		public string Name = "Unknown";
		public string FileType = ".mp3";
		public string DownloadUrl;
		public string Version;

		[NonSerialized]
		public byte[] Embedded = new byte[0];

		[NonSerialized]
		public string EmbedFile;

		[NonSerialized]
		public string FileLocation;

		[NonSerialized]
		public bool Attempted;

		[NonSerialized]
		public string LastEmbeddedFile;

		[NonSerialized]
		public string LastWrittenData;

		[NonSerialized]
		public MusicTrack LastWritten;

		public string FileName => Regex.Replace(Name, "[^A-Za-z0-9]", "");

		public override bool ReadDataString(string data)
		{
			var separatorLoc = data.IndexOf(':');
			if (separatorLoc == -1)
			{
				return false;
			}
			var numSub = data.Substring(0, separatorLoc);
			var success = int.TryParse(numSub, out int num);
			if (!success || data.Length < separatorLoc + 1 + num)
			{
				return false;
			}
			var jsonStr = data.Substring(separatorLoc + 1, num);
			var embedStr = data.Substring(separatorLoc + 1 + num);
			var embedBytes = Base16k.FromBase16kString(embedStr);
			JsonConvert.PopulateObject(jsonStr, this);
			Embedded = embedBytes;
			return true;
		}
		public override string WriteDataString()
		{
			var asJson = JsonConvert.SerializeObject(this);
			var embedStr = Base16k.ToBase16kString(Embedded);
			return asJson.Length.ToString() + ':' + asJson + embedStr;
		}

		public void NewVersion()
		{
			Version = Guid.NewGuid().ToString();
		}

		public MusicTrack Clone()
		{
			return new MusicTrack()
			{
				Name = Name,
				DownloadUrl = DownloadUrl,
				FileType = FileType,
				Version = Version,
				Embedded = (byte[])Embedded.Clone(),
				LastEmbeddedFile = LastEmbeddedFile,
				EmbedFile = EmbedFile,
			};
		}

		public string GetError()
		{
			string Error = null;

			if (string.IsNullOrEmpty(FileName))
			{
				Error = "FileName is empty (Name must have 1 character in A-Za-z0-9)";
			}
			else if (!AllowedExtensions.Contains(FileType))
			{
				Error = $"Bad file type {FileType}: should be .mp3, .wav, or .aiff";
			}
			else if (!string.IsNullOrEmpty(DownloadUrl))
			{
				var success = Uri.TryCreate(DownloadUrl, UriKind.Absolute, out Uri downloadUri);

				if (!success || !AllowedDownloadSchemes.Contains(downloadUri.Scheme))
				{
					Error = "Bad URL: should be http:// or https://";
				}
			}
			else if (string.IsNullOrEmpty(DownloadUrl) && Embedded.Length == 0)
			{
				Error = "Missing data";
			}
			return Error;
		}

		public static MusicTrack FromObject(ZEventListener obj)
		{
			var newThis = new MusicTrack();
			var success = newThis.ReadObject(obj);
			return success ? newThis : null;
		}
	}
}
