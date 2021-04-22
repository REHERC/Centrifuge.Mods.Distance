using App.AdventureMaker.Core.Tasks;

namespace App.AdventureMaker.Core.Interfaces
{
	public interface IProgressData
	{
		public delegate bool Callback(IProgressData progress);

		public string Title { get; set; }

		public string Status { get; set; }

		public int Value { get; set; }

		public int Maximum { get; set; }

		public bool Run(TaskBase task);

		void NotifyCompleted();
	}
}
