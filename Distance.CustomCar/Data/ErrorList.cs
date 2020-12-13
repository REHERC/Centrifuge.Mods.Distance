using Centrifuge.Distance.Data;
using Centrifuge.Distance.Game;
using Reactor.API.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Distance.CustomCar.Data
{
	public class ErrorList : List<Error>
	{
		readonly Log logger_;

		public ErrorList(Log logger)
		{
			logger_ = logger;
		}

		public void Add(string message, string source = "")
		{
			Add(new Error(message, source));
		}

		public new void Add(Error val)
		{
			base.Add(val);
			logger_.Error($"{val.message}\t({val.source})");
		}

		public override string ToString()
		{
			return string.Join("\n", this.Select(x => x.message).ToArray());
		}
		
		public void Show()
		{
			MessageBox.Create($"An error occured while loading the cars\n{ToString()}", "Custom Car")
			.SetButtons(MessageButtons.Ok)
			.Show();
		}
	}
}
