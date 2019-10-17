namespace rmku.Protocol.Primitives
{
	internal struct Boolean
	{
		public bool Value { get; }

		public Boolean(byte value)
		{
			Value = value > 0;
		}
	}
}
