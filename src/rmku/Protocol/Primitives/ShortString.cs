using System;
using System.Diagnostics;

namespace rmku.Protocol.Primitives
{
	[DebuggerDisplay("{System.Text.Encoding.ASCII.GetString(Value)}")]
	internal struct ShortString
	{
		public byte[] Value { get; }

		public ShortString(byte[] value)
		{
			if(value.Length > Constants.ShortStringLength)
				throw new ArgumentException(nameof(value));
				
			Value = value;			
		}
	}
}