namespace App.AdventureMaker.Core.Interfaces
{
	public interface ISaveLoad<DATA>
	{
		public void SaveData(DATA project);

		public void LoadData(DATA project);
	}
}
