using rmku.Protocol.Primitives;

namespace rmku.Protocol.Connection
{
	internal struct Secure
	{
		public LongString Challenge { get; }

		public Secure(LongString challenge)
		{
			Challenge = challenge;
		}
	}
}
