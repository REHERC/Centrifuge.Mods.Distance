using Reactor.API;

namespace Distance.CustomCar.Data
{
	public class CarFactory
	{
		private readonly ErrorList errors_;

		public CarPrefabDatabase Prefabs { get; internal set; }

		public CarInfos Infos { get; internal set; }

		//public CarBuilder Builder { get; internal set; }

		public CarFactory(ErrorList errors)
		{
			errors_ = errors;

			Prefabs = new CarPrefabDatabase(Mod.Instance.GetLocalFolder(Defaults.PrivateAssetsDirectory), errors_);
			Infos = new CarInfos();
		}


		public void MakeCars()
		{
			if (Infos.CollectInformations())
			{

			}
		}
	}
}