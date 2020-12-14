namespace Distance.CustomCar.Data.Error
{
	public struct Error
	{
		public readonly string message;
		public readonly string source;

		public Error(string errorMessage, string errorSource)
		{
			message = errorMessage;
			source = errorSource;
		}

		public override string ToString()
		{
			return message;
		}

		public static implicit operator string(Error error)
		{
			return error.ToString();
		}
	}
}
