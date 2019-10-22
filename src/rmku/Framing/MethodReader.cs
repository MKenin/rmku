using System;
using rmku.Protocol.Connection;

namespace rmku.Framing
{
	internal static class MethodReader
	{
		internal static Start ParseStart(ReadOnlySpan<byte> body)
		{

			return new Start();
		}
	}
}
