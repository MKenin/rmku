using System;
using System.Buffers;
using System.IO.Pipelines;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Amp
{
    public class Connection
    {
        private readonly string _server;
        private readonly ushort _port;

        public Connection(string server, ushort port = 5672)
        {
            _server = server;
            _port = port;
        }

        public async Task Open()
        {
            // open socket
            //TODO: proper disposable handling
            var socket = await Connect();
            var process = ProcessLinesAsync(socket);
            byte[] head =
            {
                (byte)'A',
                (byte)'M',
                (byte)'Q',
                (byte)'P',
                0,
                0,
                9,
                1
            };
            socket.Send(head);
            await process;
        }

        static async Task ProcessLinesAsync(Socket socket)
        {
            var pipe = new Pipe();
            Task writing = FillPipeAsync(socket, pipe.Writer);
            Task reading = ReadPipeAsync(pipe.Reader);

            await Task.WhenAll(reading, writing);
        }

        static async Task FillPipeAsync(Socket socket, PipeWriter writer)
        {
            while (true)
            {
                try
                {
                    Memory<byte> memory = writer.GetMemory();
                    int bytesRead = await socket.ReceiveAsync(memory, SocketFlags.None);
                    if (bytesRead == 0)
                        break;

                    writer.Advance(bytesRead);
                }
                catch (Exception)
                {
                    break;
                }

                FlushResult result = await writer.FlushAsync();

                if (result.IsCompleted)
                    break;
            }

            writer.Complete();
        }

        static async Task ReadPipeAsync(PipeReader reader)
        {
            while (true)
            {
                ReadResult result = await reader.ReadAsync();
                ReadOnlySequence<byte> buffer = result.Buffer;

                while (buffer.TryReadFrame(out Frame frame))
                {
                    HandleFrame(frame);
                    var nextPart = buffer.GetPosition(frame.Length + 8);
                    buffer = buffer.Slice(nextPart);
                };

                reader.AdvanceTo(buffer.Start, buffer.End);

                if (result.IsCompleted)
                    break;
            }
            reader.Complete();
        }

        private static void HandleFrame(Frame frame)
        {
            Console.WriteLine($"Got frame {frame.Type} with length {frame.Length}");
        }

        private static void ProcessFrame(Byte type, Int16 channel, Int32 size, ReadOnlySpan<byte> readOnlySequence)
        {
            StringBuilder sb = new StringBuilder(size + 7);

            sb.AppendFormat("Recieved frame type {0}", type);
            sb.AppendFormat(" on channel {0}", channel);
            sb.AppendFormat(" of size {0}.", size);

            sb.AppendFormat(" with content: ");
            var bytes = readOnlySequence.ToArray();
            sb.Append(BitConverter.ToString(bytes));

            Console.WriteLine(sb.ToString());
        }

        private async Task<Socket> Connect()
        {
            IPAddress[] addresses = await Dns.GetHostAddressesAsync(_server);
            if (addresses.Length == 0)
                throw new ArgumentException();

            IPAddress serverAddress;
            if (addresses.Length == 1)
                serverAddress = addresses[0];
            else
                serverAddress = addresses[new Random().Next(0, addresses.Length)];

            IPEndPoint address = new IPEndPoint(serverAddress, _port);

            Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp);

            await socket.ConnectAsync(address);
            return socket;
        }
    }

    internal static class NetHelper
    {
        public static bool TryReadFrame(ref this ReadOnlySequence<byte> buffer, out Frame frame)
        {
            frame = default;
            if (buffer.Length < 7)
                return false;

            SequenceReader<byte> reader = new SequenceReader<byte>(buffer);
            var span = reader.UnreadSpan;

            // all those are uint
            reader.TryRead(out Byte type);
            reader.TryReadBigEndian(out Int16 channel);
            reader.TryReadBigEndian(out Int32 size);

            if (buffer.Length < size + 8)
                return false;

            reader.TryRead(out byte frameEnd);
            frame = new Frame(type, (ushort)channel, size);
            return true;
        }
    }

    internal struct Frame
    {
        public FrameType Type { get; }
        public ushort Channel { get; }
        public long Length { get; }

        public Frame(byte type, ushort channel, long length)
        {
            Type = (FrameType)type;
            Channel = channel;
            Length = length;
        }
    }

    internal enum FrameType : byte
    {
        Method = 1,
        Header = 2,
        Body = 3,
        Heartbeat = 4
    }
}
