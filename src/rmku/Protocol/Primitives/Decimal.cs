namespace rmku.Protocol.Primitives
{
	internal struct Decimal
	{
		public decimal Value { get; }

		public Decimal(decimal value)
		{
			Value = value;
		}
	}
}
