using Centrifuge.Distance.Data;
using Centrifuge.Distance.Game;
using Reactor.API.Logging;
using System;
using System.Collections.Generic;

namespace Distance.CustomCar.Data.Errors
{
	public class ErrorList : List<string>
	{
		private readonly Log logger_;

		public ErrorList(Log logger)
		{
			logger_ = logger;
		}

		public new void Add(string value)
		{
			base.Add(value);
			logger_.Error(value);
		}

		public void Add(Exception value)
		{
			base.Add(value.ToString());
			logger_.Exception(value);
		}

		public void Show()
		{
			if (Count > 0)
			{
				string message = string.Join(Environment.NewLine, ToArray());

				MessageBox.Create($"Can't load the cars correctly: {Count} error(s)\n{message}", "CUSTOM CARS - ERRORS")
				.SetButtons(MessageButtons.Ok)
				.Show();
			}
		}
	}
}