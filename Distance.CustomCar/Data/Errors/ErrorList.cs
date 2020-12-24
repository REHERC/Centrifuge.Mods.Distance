using Centrifuge.Distance.Data;
using Centrifuge.Distance.Game;
using Reactor.API.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

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
			if (this.Any())
			{
				string message = Count < 15 ? string.Join(Environment.NewLine, ToArray()) : "There were too many errors when loading custom cars to be displayed here, please check the logs in your mod installation directory.";

				MessageBox.Create($"Can't load the cars correctly: {Count} error(s)\n{message}", "CUSTOM CARS - ERRORS")
				.SetButtons(MessageButtons.Ok)
				.Show();
			}
		}
	}
}