using Centrifuge.Distance.Data;
using Centrifuge.Distance.Game;
using Reactor.API.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Distance.CustomCar.Data.Error
{
	public class ErrorList : List<Error>
	{
		readonly Log logger_;

		public bool HasAny => Count > 0;

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
			logger_.Error(val);
		}

		public override string ToString()
		{
			return string.Join("\n", this.Select(x => x.ToString()).ToArray());
		}

		public void Show(bool clearAfter = false)
		{
			MessageBox.Create($"An error occured while loading the cars\n{this}", "Custom Car")
			.SetButtons(MessageButtons.Ok)
			.OnConfirm(
				() =>
				{
					if (clearAfter)
					{
						Clear();
					}
				}
			)
			.Show();
		}
	}
}
