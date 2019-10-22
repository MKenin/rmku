using rmku.Protocol.Primitives;

namespace rmku.Protocol.Connection
{
	internal struct OpenOk
	{
		public ShortString Reserved1 { get; }

		public OpenOk(ShortString reserved1)
		{
			Reserved1 = reserved1;
		}
	}
}
