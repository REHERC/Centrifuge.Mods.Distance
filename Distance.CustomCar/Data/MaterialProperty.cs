namespace Distance.CustomCar.Data
{
	public class MaterialProperty
	{
		public PropertyType type;
		public string fromName;
		public string toName;
		public int fromID;
		public int toID;

		public MaterialProperty(PropertyType propertyType, string fName, int fID, string tName, int tID)
		{
			type = propertyType;
			fromName = fName;
			fromID = fID;
			toName = tName;
			toID = tID;
		}

		public MaterialProperty()
		{
			type = 0;
			fromName = string.Empty;
			fromID = -1;
			toName = string.Empty;
			toID = -1;
		}
	}
}
