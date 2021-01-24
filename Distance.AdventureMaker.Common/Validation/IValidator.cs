namespace Distance.AdventureMaker.Common.Validation
{
	public interface IValidator<T>
	{
		void Validate(T item);
	}
}
