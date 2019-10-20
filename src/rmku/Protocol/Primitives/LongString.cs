using System;
using System.Diagnostics;

namespace rmku.Protocol.Primitives
{
	[DebuggerDisplay("{System.Text.Encoding.ASCII.GetString(Value)}")]
	internal struct LongString
	{
		public byte[] Value { get; }

		public LongString(byte[] value)
		{
			if (value.Length > Constants.LongStringLength)
				throw new ArgumentException(nameof(value));

			Value = value;
		}
	}
}