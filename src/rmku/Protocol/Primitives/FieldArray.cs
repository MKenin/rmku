namespace rmku.Protocol.Primitives
{
	internal struct FieldArray<T>
	{
		public FieldValue<T>[] Value { get; }

		public FieldArray(FieldValue<T>[] value)
		{
			Value = value;
		}
	}
}
