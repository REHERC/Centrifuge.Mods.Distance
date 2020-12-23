using Distance.CustomCar.Data.Car;
using Distance.CustomCar.Data.Errors;
using Reactor.API.Attributes;
using Reactor.API.Interfaces.Systems;
using Reactor.API.Logging;
using Reactor.API.Runtime.Patching;
using UnityEngine;

namespace Distance.CustomCar
{
	[ModEntryPoint("com.github.larnin/CustomCar")]
	public class Mod : MonoBehaviour
	{
		public static Mod Instance;

		public IManager Manager { get; set; }

		public Log Logger { get; set; }

		public ErrorList Errors { get; set; }

		public void Initialize(IManager manager)
		{
			Instance = this;
			Manager = manager;

			Logger = LogManager.GetForCurrentAssembly();

			Errors = new ErrorList(Logger);

			RuntimePatcher.AutoPatch();
		}

		// Equivalent of Spectrum's Initialize call (which happens after GameManager initialization)
		public void LateInitialize(IManager _)
		{
			CarInfos carInfos = new CarInfos();
			carInfos.CollectInfos();
			CarBuilder carBuilder = new CarBuilder();
			carBuilder.CreateCars(carInfos);
		}
	}
}
