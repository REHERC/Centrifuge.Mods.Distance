using System.ComponentModel;
using System.Threading.Tasks;

namespace App.AdventureMaker.Core.Tasks
{
	public sealed class Progress : INotifyPropertyChanged
	{
		public delegate Task Callback(Progress progress);

		public string Title { get; set; }

		public string Status { get; set; }

		public int Value { get; set; }

		public int Maximum { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
