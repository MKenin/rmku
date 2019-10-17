using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading.Tasks;
using rmku.Protocol;
using rmku.Framing;

namespace rmku.Connectivity
{
	internal class MainChannel : IChannel
	{
		public MainChannel(Socket socket)
		{
			this.socket = socket;
		}

		internal enum ConnectionState : byte
		{
			Base = 0,
			WaitForStart = 1
		}

		ConnectionState _currentState = ConnectionState.Base;
		private readonly Socket socket;

		public ValueTask Handle(Frame frame, ref ReadOnlySequence<byte> body)
		{
			Debug.Assert(frame.Type == FrameType.Method, "Main channel should onle handle Methods");
			uint methodHandle = BinaryPrimitives.ReadUInt32BigEndian(body.FirstSpan);
			ushort classId = BinaryPrimitives.ReadUInt16BigEndian(body.FirstSpan);
			body = body.Slice(2);
			ushort methodId = BinaryPrimitives.ReadUInt16BigEndian(body.FirstSpan);
			body = body.Slice(2);
			string bt = Convert.ToBase64String(body.FirstSpan);
			
			if (methodHandle == Method.Connection.Start)
			{
				if (_currentState != ConnectionState.WaitForStart)
					throw new Exception();
				var start = MethodReader.ParseStart(body.FirstSpan);

				socket.Send(new byte[0]);
			}

			return new ValueTask();
		}

		internal void KickOff()
		{
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
			_currentState = ConnectionState.WaitForStart;
		}
	}
}