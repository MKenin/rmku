using System;
using System.Buffers;
using System.Buffers.Binary;
using rmku.Protocol;
using rmku.Framing;

namespace rmku.Connectivity
{
	internal static class NetHelper
	{
		public static bool TryReadFrame(ref this ReadOnlySequence<byte> buffer, out Frame frame)
		{
			frame = default;

			if (buffer.Length < 7)
				return false;

			var reader = new SequenceReader<byte>(buffer);
			reader.TryRead(out byte type);

			var spanToRead = reader.UnreadSpan;
			ushort channel = BinaryPrimitives.ReadUInt16BigEndian(reader.UnreadSpan);
			reader.Advance(2);

			uint size = BinaryPrimitives.ReadUInt32BigEndian(reader.UnreadSpan);
			reader.Advance(size + 4);
			reader.TryRead(out byte frameEnd);
			if (frameEnd != Constants.FrameEnd)
				//TODO: proper error handling
				throw new Exception("Protocol exception");

			frame = new Frame(type, channel, size);
			return true;
		}
	}
}
