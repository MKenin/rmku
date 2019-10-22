using rmku.Protocol.Primitives;

namespace rmku.Protocol.Connection
{
	internal struct Open
	{
		public ShortString VirtualHost { get; }
		public ShortString Reserved1 { get; }
		public ShortString Reserved2 { get; }

		public Open(ShortString virtualHost, ShortString reserved1, ShortString reserved2)
		{
			VirtualHost = virtualHost;
			Reserved1 = reserved1;
			Reserved2 = reserved2;
		}
	}
}
