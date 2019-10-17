namespace rmku.Protocol.Primitives
{
	internal struct FieldValuePair<T>
	{
		public ShortString Name { get; }
		public FieldValue<T> FieldValue { get; }

		public FieldValuePair(ShortString name, FieldValue<T> fieldValue)
		{
			Name = name;
			FieldValue = fieldValue;
		}

		public FieldValuePair(ShortString name, T value)
		{
			Name = name;
			FieldValue = new FieldValue<T>(value);
		}
	}

	internal struct FieldValue<T>
	{
		public T Value { get; }

		public FieldValue(T value)
		{
			Value = value;
		}
	}
}
