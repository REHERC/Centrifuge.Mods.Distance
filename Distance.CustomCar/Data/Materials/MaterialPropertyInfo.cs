namespace Distance.CustomCar.Data.Materials
{
	public class MaterialPropertyInfo
	{
		public string shaderName;
		public string name;
		public int diffuseIndex = -1;
		public int normalIndex = -1;
		public int emitIndex = -1;

		public MaterialPropertyInfo(string _shaderName, string _name, int _diffuseIndex, int _normalIndex, int _emitIndex)
		{
			shaderName = _shaderName;
			name = _name;
			diffuseIndex = _diffuseIndex;
			normalIndex = _normalIndex;
			emitIndex = _emitIndex;
		}
	}
}
