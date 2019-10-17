using System;

namespace rmku.Protocol.Primitives
{
	internal struct LongString
	{
		public string Value { get; }

		public LongString(string value)
		{
			if (value.Length > Constants.LongStringLength)
				throw new ArgumentException(nameof(value));

			Value = value;
		}
	}
}