using System.Collections.Generic;
using System.Linq;

namespace Distance.TrackMusic.Models
{
	public class AttachedData<T>
	{
		private readonly List<object> toRemove = new List<object>();
		private readonly Dictionary<object, T> attached = new Dictionary<object, T>();

		public IEnumerable<KeyValuePair<object, T>> Pairs => attached.AsEnumerable();

		public T this[object obj]
		{
			get => Get(obj);
			set => Set(obj, value);
		}

		public delegate T DefaultValueCallback();

		public T GetOrCreate(object obj, DefaultValueCallback createDefault)
		{
			if (attached.TryGetValue(obj, out T data))
			{
				return data;
			}

			data = createDefault();
			attached[obj] = data;
			return data;
		}

		public T Get(object obj)
		{
			attached.TryGetValue(obj, out T data);
			return data;
		}

		public void Set(object obj, T data)
		{
			if (obj == null)
			{
				return;
			}

			attached[obj] = data;
		}

		public void Remove(object obj)
		{
			attached.Remove(obj);
		}

		public void Update()
		{
			foreach (var key in attached.Keys)
			{
				if (key == null)
				{
					toRemove.Add(key);
				}
			}

			foreach (var obj in toRemove)
			{
				attached.Remove(obj);
			}

			toRemove.Clear();
		}
	}
}
