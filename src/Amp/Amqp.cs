using System;
using System.Buffers;

namespace Amp
{
	internal struct ShortUint
	{
		public ushort Value { get; }
		public ShortUint(ushort value)
		{
			Value = value;
		}
	}

	internal static class Amqp
	{
		public static ShortUint ReadShortUint(ref ReadOnlySequence<byte> data)
		{
			throw new NotImplementedException();
		}
	}
}
