using App.AdventureMaker.Core.Tasks;
using System;
using System.Threading.Tasks;

namespace App.AdventureMaker.Core.Interfaces
{
	public interface IProgressReporter
	{
		public Task RunTask(TaskBase task);

		protected void OnProgressDataChanged(object sender, EventArgs e);
	}
}
