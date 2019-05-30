
using System;
using System.Buffers;

namespace Amp
{
	internal interface IChannel
	{
		void Handle(Frame frame, Span<byte> body);
	}
}
