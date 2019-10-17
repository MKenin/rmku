using System.Buffers;
using System.Threading.Tasks;
using rmku.Framing;

namespace rmku.Connectivity
{
	internal interface IChannel
	{
		ValueTask Handle(Frame frame, ref ReadOnlySequence<byte> body);
	}
}
