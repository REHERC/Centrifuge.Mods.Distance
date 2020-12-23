using Distance.DecorativeLamp.Enums;

namespace Events.DecorativeLamp
{
	public class ChangeLampModel : StaticEvent<ChangeLampModel.Data>
	{
		public struct Data
		{
			public readonly LampModel model;

			public Data(LampModel m)
			{
				model = m;
			}
		}
	}
}
