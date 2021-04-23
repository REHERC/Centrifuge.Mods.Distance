using App.AdventureMaker.Core.Interfaces;
using System.Threading.Tasks;

namespace App.AdventureMaker.Core.Tasks
{
	public abstract class TaskBase
	{
		public abstract Task Execute(Progress progress);

		public static async Task Run<T>(TaskBase task) where T : IProgressReporter, new()
		{
			await Run(new T(), task).ConfigureAwait(false);
		}

		public static async Task Run(IProgressReporter reporter, TaskBase task)
		{
			await reporter.RunTask(task).ConfigureAwait(false);
		}
	}
}
