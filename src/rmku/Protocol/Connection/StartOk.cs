using rmku.Protocol.Primitives;

namespace rmku.Protocol.Connection
{
	internal struct StartOk
	{
		public Table ClientProperties { get; }
		public ShortString Mechanism { get; }
		public LongString Response { get; }
		public ShortString Locale { get; }

		public StartOk(Table clientProperties, ShortString mechanism, LongString response, ShortString locale)
		{
			ClientProperties = clientProperties;
			Mechanism = mechanism;
			Response = response;
			Locale = locale;
		}
	}
}
