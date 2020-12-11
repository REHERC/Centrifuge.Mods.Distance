using System.Collections.Generic;

namespace Distance.CustomCar
{
	public class ErrorList : List<string>
	{
		public override string ToString()
		{
			return string.Join("\n", ToArray());
		}

		public void Show()
		{
			MessageBox.
		}
	}
}
