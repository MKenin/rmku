using rmku.Protocol;

namespace rmku.Framing
{
	internal struct Frame
	{
		public FrameType Type { get; }
		public ushort Channel { get; }
		public long ContentLength { get; }
		public long TotalLength { get { return ContentLength + Constants.HeaderSize + Constants.EndFrameSize; } }
		public Frame(byte type, ushort channel, long length)
		{
			Type = (FrameType)type;
			Channel = channel;
			ContentLength = length;
		}
	}
}
