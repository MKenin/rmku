using rmku.Protocol.Primitives;

namespace rmku.Protocol.Connection
{
	internal struct TuneOk
	{
		public ShortUInt ChannelMax { get; }
		public LongUInt FrameMax { get; }
		public ShortUInt Heartbeat { get; }

		public TuneOk(ShortUInt channelMax, LongUInt frameMax, ShortUInt heartbeat)
		{
			ChannelMax = channelMax;
			FrameMax = frameMax;
			Heartbeat = heartbeat;
		}
	}
}
