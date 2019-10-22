using rmku.Protocol.Primitives;

namespace rmku.Protocol.Connection
{
	internal struct SecureOk
	{
		public LongString Response { get; }

		public SecureOk(LongString response)
		{
			Response = response;
		}
	}
}
