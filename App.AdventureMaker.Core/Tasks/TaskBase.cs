using App.AdventureMaker.Core.Interfaces;

namespace App.AdventureMaker.Core.Tasks
{
	public abstract class TaskBase
	{
		public abstract bool Execute(IProgressData progress);

		public static bool Run<PROGRESS>(TaskBase task) where PROGRESS : IProgressData, new()
		{
			PROGRESS progress = new PROGRESS();
			bool result = progress.Run(task);
			progress.NotifyCompleted();
			return result;
		}
	}
}
