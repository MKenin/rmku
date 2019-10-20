namespace rmku.Protocol.Primitives
{
	internal struct FieldValue<T>
	{
		public T Value { get; }

		public FieldValue(T value)
		{
			Value = value;
		}
	}
}
