using rmku.Protocol.Primitives;

namespace rmku.Protocol
{
	internal struct Start
	{
		public byte VersionMajor { get; }
		public byte VersionMinor { get; }
		public Table ServerProperties { get; }
		public LongString Mechanisms { get; }
		public LongString Locales { get; }

		public Start(byte versionMajor, byte versionMinor, Table serverProperties, LongString mechanisms, LongString locales)
		{
			VersionMajor = versionMajor;
			VersionMinor = versionMinor;
			ServerProperties = serverProperties;
			Mechanisms = mechanisms;
			Locales = locales;
		}
	}
}
