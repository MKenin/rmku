namespace rmku.Protocol
{
	public class Constants
	{
		public const byte FrameEnd = 206;
		public const byte HeaderSize = 7;
		public const byte EndFrameSize = 1;
		public const byte ShortStringLength = 255;
		public static uint LongStringLength = uint.MaxValue;
	}
}
