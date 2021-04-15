using Distance.AdventureMaker.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Distance.AdventureMaker.Common.Validation
{
	public abstract class Validator<T> : IValidator<T>
	{
		public Queue<ValidationItem> Messages { get; } = new Queue<ValidationItem>();

		public ValidationItem[] GetMessages(StatusLevel status)
		{
			return Messages.Where(m => m.status.HasFlag(status)).ToArray();
		}

		public void Log(StatusLevel sl, string msg) => Messages.Enqueue(new ValidationItem(sl, msg));

		public void Log(StatusLevel sl, string msg, params string[] values) => Messages.Enqueue(new ValidationItem(sl, msg, values));

		public override string ToString() => string.Join(Environment.NewLine, Messages.Select(m => m.ToString()).ToArray());

		public abstract void Validate(T item);

		public static implicit operator bool(Validator<T> x) => x?.GetMessages(StatusLevel.Error).Length == 0;
	}
}
