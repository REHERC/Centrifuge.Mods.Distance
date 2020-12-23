using Newtonsoft.Json.Linq;
using Reactor.API.Configuration;
using System;
using UnityEngine;

namespace Distance.CustomDeathMessages
{
	public class ConfigurationLogic : MonoBehaviour
	{
		#region Properties
		public string[] KillGrid
		{
			get => Get<JArray>("KillGrid").ToObject<string[]>();
			set => Set("KillGrid", new JArray(value));
		}

		public string[] SelfTermination
		{
			get => Get<JArray>("SelfTermination").ToObject<string[]>();
			set => Set("SelfTermination", new JArray(value));
		}

		public string[] LaserOverheated
		{
			get => Get<JArray>("LaserOverheated").ToObject<string[]>();
			set => Set("LaserOverheated", new JArray(value));
		}

		public string[] Impact
		{
			get => Get<JArray>("Impact").ToObject<string[]>();
			set => Set("Impact", new JArray(value));
		}

		public string[] Overheated
		{
			get => Get<JArray>("Overheated").ToObject<string[]>();
			set => Set("Overheated", new JArray(value));
		}

		public string[] AntiTunnelSquish
		{
			get => Get<JArray>("AntiTunnelSquish").ToObject<string[]>();
			set => Set("AntiTunnelSquish", new JArray(value));
		}

		public string[] StuntCollect
		{
			get => Get<JArray>("StuntCollect").ToObject<string[]>();
			set => Set("StuntCollect", new JArray(value));
		}

		public string[] KickNoLevel
		{
			get => Get<JArray>("KickNoLevel").ToObject<string[]>();
			set => Set("KickNoLevel", new JArray(value));
		}

		public string[] Finished
		{
			get => Get<JArray>("Finished").ToObject<string[]>();
			set => Set("Finished", new JArray(value));
		}

		public string[] NotReady
		{
			get => Get<JArray>("NotReady").ToObject<string[]>();
			set => Set("NotReady", new JArray(value));
		}

		public string[] Spectate
		{
			get => Get<JArray>("Spectate").ToObject<string[]>();
			set => Set("Spectate", new JArray(value));
		}

		public string[] TagPointsLead
		{
			get => Get<JArray>("TagPointsLead").ToObject<string[]>();
			set => Set("TagPointsLead", new JArray(value));
		}
		#endregion

		internal Settings Config;

		public event Action<ConfigurationLogic> OnChanged;

		private void Load()
		{
			Config = new Settings("Config");
		}

		public void Awake()
		{
			Load();

			CreateDefault(section: "KillGrid",
				"The laser grid was not cool with {0}",
				"{0} have touched the forbiden grid"
			);

			CreateDefault(section: "SelfTermination",
				"{0} pressed the reset button",
				"{0} commited sudoku"
			);

			CreateDefault(section: "LaserOverheated",
				"{0} don't know how to drive without wheels",
				"{0} was too hot"
			);

			CreateDefault(section: "Impact",
				"{0} kissed a wall",
				"The ground facepalmed {0}"
			);

			CreateDefault(section: "Overheated",
				"{0} needs to stop boosting sometimes"
			);

			CreateDefault(section: "AntiTunnelSquish",
				"{0} got unitied"
			);

			CreateDefault(section: "StuntCollect",
				"{0} looted a x{1} multiplier!"
			);

			CreateDefault(section: "KickNoLevel",
				"{0} is too poor to have this level",
				"[FF0000]{0} is sad because he can't load the level[-]"
			);

			CreateDefault(section: "Finished",
				"[FFFFFF]{0}[-] [00FF00]f[-][00FFFF]i[-][0000FF]n[-][FF00FF]i[-][FF0000]s[-][FFFF00]h[-][00FF00]e[-][00FFFF]d[-]"
			);

			CreateDefault(section: "NotReady",
				"{0} is a little busy, try agains later"
			);

			CreateDefault(section: "Spectate",
				"[-]This map is too hard, {0} gave up"
			);

			CreateDefault(section: "TagPointsLead",
				"[FFFFFF]{0}[-] is [00FF00]f[-][00FFFF]a[-][0000FF]b[-][FF00FF]u[-][FF0000]l[-][FFFF00]o[-][00FF00]u[-][00FFFF]s[-]!"
			);

			Save();
		}

		public void CreateDefault(string section, params string[] lines)
		{
			Get(section, new JArray(lines));
		}

		public T Get<T>(string key, T @default = default)
		{
			return Config.GetOrCreate(key, @default);
		}

		public void Set<T>(string key, T value)
		{
			Config[key] = value;
			Save();
		}

		public void Save()
		{
			Config.Save();
			OnChanged(this);
		}
	}
}
