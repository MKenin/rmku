namespace Amp.Framing
{
    public class Protocol
    {
        public const byte FrameEnd = 206;
        public const byte HeaderSize = 7;
        public const byte EndFrameSize = 1;
    }

    internal static class Method
    {
        internal static class Connection
        {
            public const uint Start = 655370;
            public const uint StartOk = 655371;
            public const uint Secure = 655380;
            public const uint SecureOk = 655381;
            public const uint Tune = 655390;
            public const uint TuneOk = 655391;
            public const uint Open = 655400;
            public const uint OpenOk = 655401;
            public const uint Close = 655410;
            public const uint CloseOk = 655411;
            public const uint Blocked = 655420;
            public const uint Unblocked = 655421;
        }

        internal static class Channel
        {
            public const uint Open = 1310730;
            public const uint OpenOk = 1310731;
            public const uint Flow = 1310740;
            public const uint FlowOk = 1310741;
            public const uint Close = 1310760;
            public const uint CloseOk = 1310761;
        }

        internal static class Basic
        {
            public const uint Qos = 3932170;
            public const uint QosOk = 3932171;
            public const uint Consume = 3932180;
            public const uint ConsumeOk = 3932181;
            public const uint Cancel = 3932190;
            public const uint CancelOk = 3932191;
            public const uint Publish = 3932200;
            public const uint Return = 3932210;
        }
    }
}
