namespace rmku.Protocol.Primitives
{
	internal struct Timestamp
	{
		public ulong Value { get; }

		public Timestamp(ulong value)
		{
			Value = value;
		}
	}
}