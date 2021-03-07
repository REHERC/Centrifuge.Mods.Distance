using System;

namespace App.AdventureMaker.Core.Interfaces
{
	public interface IModifiedNotifier<DATA>
	{
		public event Action<DATA> OnModified;
	}
}
