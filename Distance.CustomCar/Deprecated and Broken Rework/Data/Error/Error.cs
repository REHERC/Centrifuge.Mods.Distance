namespace Distance.CustomCar.Data.Error
{
	public struct Error
	{
		public readonly string message;
		public readonly string source;
		public readonly string file;

		public Error(string errorMessage, string errorSource, string errorFile = "")
		{
			message = errorMessage;
			source = errorSource;
			file = errorFile;
		}

		public override string ToString()
		{
			return $"[{source}] \t{message}" + (string.IsNullOrEmpty(file) ? string.Empty : $" \t({file})");
		}

		public static implicit operator string(Error error)
		{
			return error.ToString();
		}
	}
}
