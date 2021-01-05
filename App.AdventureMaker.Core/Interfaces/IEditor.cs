namespace App.AdventureMaker.Core.Interfaces
{
	public interface IEditor<DATA, ROOT>
	{
		protected ROOT DataRoot { get; set; }

		protected DATA Data { get; set; }

		public void Save(ref DATA data);

		public void Load(DATA data);
	}
}
