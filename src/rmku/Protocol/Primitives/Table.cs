namespace rmku.Protocol.Primitives
{
	internal struct Table
	{
		public object[] FieldValuePairs { get; }

		public Table(object[] fieldValuePairs)
		{
			FieldValuePairs = fieldValuePairs;
		}
	}
}
