using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Diagnostics;
using System.Threading.Tasks;
using Amp.Framing;

namespace Amp
{
	internal class MainChannel : IChannel
	{
		internal enum ConnectionState : byte
		{
			Base = 0,
			WaitForStart = 1
		}

		ConnectionState _currentState = ConnectionState.Base;

		public void Handle(Frame frame, Span<byte> body)
		{
			Debug.Assert(frame.Type == FrameType.Method, "Main channel should onle handle Methods");

			uint methodHandle = BinaryPrimitives.ReadUInt32BigEndian(body);
			ushort classId = BinaryPrimitives.ReadUInt16BigEndian(body);
			byte[] sample = body.Slice(0, 4).ToArray();

			body = body.Slice(2);
			ushort methodId = BinaryPrimitives.ReadUInt16BigEndian(body);
			body = body.Slice(2);

			if (methodHandle == Method.Connection.Start)
			{
				if (_currentState != ConnectionState.WaitForStart)
					throw new Exception();

				// okay, server wants to speak with us
				// let's go and answer something. But how?
			}
		}
	}
}
