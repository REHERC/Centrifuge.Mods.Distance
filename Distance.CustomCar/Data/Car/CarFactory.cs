using Reactor.API;

namespace Distance.CustomCar.Data.Car
{
	public class CarFactory
	{
		public ErrorList Errors { get; internal set; }

		public CarPrefabDatabase Prefabs { get; internal set; }

		public CarInfos Infos { get; internal set; }

		public CarBuilder Builder { get; internal set; }

		public CarFactory(ErrorList errors)
		{
			Errors = errors;

			Prefabs = new CarPrefabDatabase(Mod.Instance.GetLocalFolder(Defaults.PrivateAssetsDirectory), Errors);
			Infos = new CarInfos();
			Builder = new CarBuilder(this);
		}

		public void MakeCars()
		{
			if (Infos.CollectInformations())
			{
				Builder.CreateCars();
			}
		}

		public void RegisterCars()
		{

		}
	}
}