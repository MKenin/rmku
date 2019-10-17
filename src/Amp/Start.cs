namespace Amp
{
	internal struct Start
	{
		public ushort VersionMajor { get; }
		public ushort VersionMinor { get; }
		public Table ServerProperties { get; }
		public string Mechanisms { get; }
		public string Locales { get; }
	}
}
