using System.Collections.Generic;

namespace App.CustomDeathMessages.Core.Data
{
	public class EditorData : Dictionary<string, string[]>
	{
		public static readonly string[] Sections;

		static EditorData()
		{
			Sections = new string[]
			{
				"AntiTunnelSquish",
				"Finished",
				"Impact",
				"LaserOverheated",
				"KickNoLevel",
				"KillGrid",
				"NotReady",
				"Overheated",
				"SelfTermination",
				"Spectate",
				"StuntCollect",
				"TagPointsLead"
			};
		}

		public EditorData()
		{
			foreach (string section in Sections)
			{
				this[section] = new string[0];
			}
		}
	}
}
