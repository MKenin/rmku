using rmku.Protocol.Primitives;

namespace rmku.Protocol.Connection
{
	internal struct Close
	{
		public ShortUInt ReplyCode { get; }
		public ShortString ReplyText { get; }
		public ShortUInt ClassId { get; }
		public ShortUInt MethodId { get; }

		public Close(ShortUInt replyCode, ShortString replyText, ShortUInt classId, ShortUInt methodId)
		{
			ReplyCode = replyCode;
			ReplyText = replyText;
			ClassId = classId;
			MethodId = methodId;
		}
	}
}
