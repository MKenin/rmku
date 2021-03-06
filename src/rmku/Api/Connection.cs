using System;
using System.Buffers;
using System.IO.Pipelines;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Collections.Generic;
using rmku.Protocol;
using rmku.Connectivity;
using rmku.Framing;

namespace rmku
{
	public class Connection
	{
		private readonly string _server;
		private readonly ushort _port;
		private readonly Dictionary<ushort, IChannel> _channels = new Dictionary<ushort, IChannel>();

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
			var sChannel = new MainChannel(socket);
			
			_channels.Add(0, (IChannel)sChannel);

			var comms = ProcessLinesAsync(socket, _channels);
			sChannel.KickOff();
			await comms;
		}

		static async Task ProcessLinesAsync(Socket socket, IReadOnlyDictionary<ushort, IChannel> handlers)
		{
			var pipe = new Pipe(new PipeOptions());
			Task writing = FillPipeAsync(socket, pipe.Writer);
			Task reading = ReadPipeAsync(handlers, pipe.Reader);

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

		static async Task ReadPipeAsync(IReadOnlyDictionary<ushort, IChannel> handlers, PipeReader reader)
		{
			while (true)
			{
				ReadResult result = await reader.ReadAsync();
				ReadOnlySequence<byte> buffer = result.Buffer;
				
				while (buffer.TryReadFrame(out Frame frame))
				{
					//TODO: proper memory management instead of copy
					var slice = buffer.Slice(Constants.HeaderSize, frame.ContentLength);

					await handlers[frame.Channel].Handle(frame, ref slice);

					var nextPart = buffer.GetPosition(frame.TotalLength);
					buffer = buffer.Slice(nextPart);
				};

				reader.AdvanceTo(buffer.Start, buffer.End);

				if (result.IsCompleted)
					break;
			}
			reader.Complete();
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
}
