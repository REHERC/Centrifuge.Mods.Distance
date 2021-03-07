#if APP
using System.IO;
using System.Linq;
using System.Text;

namespace Distance.AdventureMaker.Common.Models.UI
{
	public class ProjectCreateData
	{
		public readonly string name;
		public readonly string description;
		public readonly string path;

		public ProjectCreateData(string name_, string description_, string path_)
		{
			name = name_;
			description = description_;
			path = path_;
		}

		public bool Validate(out string message)
		{
			StringBuilder sb = new StringBuilder();
			bool flag = true;

			if (string.IsNullOrWhiteSpace(name))
			{
				sb.AppendLine("You must specify a campaign name.");
				flag = false;
			}

			if (string.IsNullOrWhiteSpace(path))
			{
				sb.AppendLine("You must specify a path to create the project.");
				flag = false;
			}

			if (Directory.Exists(path) && Directory.GetFileSystemEntries(path).Length > 0)
			{
				sb.AppendLine("The selected project folder must be empty.");
				flag = false;
			}

			message = sb.ToString();
			return flag;
		}

		public CampaignFile ToProject()
		{
			CampaignFile project = new CampaignFile();

			project.metadata.title = name;
			project.metadata.description = description;

			return project;
		}
	}
}
#endif