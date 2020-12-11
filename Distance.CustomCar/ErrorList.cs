using Centrifuge.Distance.Data;
using Centrifuge.Distance.Game;
using System.Collections.Generic;

namespace Distance.CustomCar
{
	public class ErrorList : List<string>
	{
		public new void Add(string val)
		{
			base.Add(val);
		}

		public override string ToString()
		{
			return string.Join("\n", ToArray());
		}
		
		public void Show()
		{
			MessageBox.Create($"An error occured while loading the cars\n{ToString()}", "Custom car - Errors")
			.SetButtons(MessageButtons.Ok)
			.Show();
		}
	}
}
