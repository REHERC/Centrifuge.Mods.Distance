using Distance.AdventureMaker.Common.Enums;

namespace Distance.AdventureMaker.Common.Validation
{
	public struct ValidationItem
	{
		public StatusLevel status;
		public string details;

		public ValidationItem(StatusLevel sl, string d)
		{
			status = sl;
			details = d;
		}

		public ValidationItem(StatusLevel sl, string d, params string[] values)
		{
			status = sl;
			details = string.Format(d, values);
		}

		public override string ToString()
		{
			return $"[{status}]\t{details}";
		}
	}
}
