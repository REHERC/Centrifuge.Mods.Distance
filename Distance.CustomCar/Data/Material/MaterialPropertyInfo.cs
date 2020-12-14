namespace Distance.CustomCar.Data.Materials
{
	public struct MaterialPropertyInfo
	{
		public static readonly MaterialPropertyInfo[] CommonCarMaterials;

		public readonly int diffuseID;
		public readonly int emitID;
		public readonly int normalID;
		public readonly string materialName;
		public readonly string shaderName;

		static MaterialPropertyInfo()
		{
			CommonCarMaterials = new MaterialPropertyInfo[]
			{
				new MaterialPropertyInfo("Custom/LaserCut/CarPaint", "carpaint", 5, -1, -1),
				new MaterialPropertyInfo("Custom/LaserCut/CarWindow", "carwindow", -1, 218, 219),
				new MaterialPropertyInfo("Custom/Reflective/Bump Glow LaserCut", "wheel", 5, 218, 255),
				new MaterialPropertyInfo("Custom/LaserCut/CarPaintBump", "carpaintbump", 5, 218, -1),
				new MaterialPropertyInfo("Custom/Reflective/Bump Glow Interceptor Special", "interceptor", 5, 218, 255),
				new MaterialPropertyInfo("Custom/LaserCut/CarWindowTrans2Sided", "transparentglow", -1, 218, 219)
			};
		}

		public MaterialPropertyInfo(string shader, string material, int diffuse, int normal, int emit)
		{
			shaderName = shader;
			materialName = material;
			diffuseID = diffuse;
			normalID = normal;
			emitID = emit;
		}
	}
}
