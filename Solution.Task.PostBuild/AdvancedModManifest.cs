using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace Solution.Task.PostBuild
{
	[Serializable, DataContract]
	public class AdvancedModManifest : ModManifest
	{
		[JsonIgnore]
		public string BuildPath => $"Distance {FriendlyName}";

		[JsonIgnore]
		public string AuthorInfo => $"{Author} ({Contact})";

		[JsonIgnore]
		public string PorterInfo => WasPorted ? $"{Porter} ({PorterContact})" : string.Empty;

		[JsonIgnore]
		public bool WasPorted => !string.IsNullOrEmpty(Porter);

		[JsonIgnore]
		public string CreatorInfo
		{
			get
			{
				if (WasPorted)
				{
					return $"<i>{FriendlyName}</i> was originally created by <b>{AuthorInfo}</b> with the (now deperecated) <a target=\"_blank\" href=\"https://github.com/Ciastex/Spectrum\">Spectrum API</a> and was ported to Centrifuge by <b>{PorterInfo}</b>.";
				}
				else
				{
					return $"<i>{FriendlyName}</i> was created by <b>{AuthorInfo}</b>.";
				}
			}
		}

		[JsonIgnore]
		public string DependencyList => Dependencies?.Length > 0 ? string.Join(", ", Dependencies) : "<i>No dependencies</i>";

		[JsonIgnore]
		public string GameSupportList => RequiredGSLs?.Any() == true
			? string.Join(", ", RequiredGSLs)
			: "<i>No GSL required</i>";

		[JsonIgnore]
		public string CentrifugeVersion { get; set; } = "Not found";

		[JsonIgnore]
		public DirectoryInfo ModDirectory { get; set; }

		[JsonIgnore]
		public DirectoryInfo ContentDirectory => new DirectoryInfo($"{ModDirectory.FullName}.Content");
	}
}
