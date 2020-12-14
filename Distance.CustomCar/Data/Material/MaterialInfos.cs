using UnityEngine;

namespace Distance.CustomCar.Data.Materials
{
	public class MaterialInfos
	{
		public readonly int diffuseID;
		public readonly int emitID;
		public readonly int normalID;
		public readonly Material material;

		public MaterialInfos(Material mat, int diffuse, int emit, int normal)
		{
			material = mat;
			diffuseID = diffuse;
			emitID = emit;
			normalID = normal;
		}
	}
}
