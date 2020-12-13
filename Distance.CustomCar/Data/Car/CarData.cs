using UnityEngine;

namespace Distance.CustomCar.Data.Car
{
	public struct CarData
	{
		public readonly GameObject carPrefab;
		public readonly CarColors carColors;

		public CarData(GameObject car, CarColors colors)
		{
			carPrefab = car;
			carColors = colors;
		}
	}
}
