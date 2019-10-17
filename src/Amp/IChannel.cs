using System.Buffers;
using System.Threading.Tasks;

namespace Amp
{
	internal interface IChannel
	{
		ValueTask Handle(Frame frame, ref ReadOnlySequence<byte> body);
	}
}
