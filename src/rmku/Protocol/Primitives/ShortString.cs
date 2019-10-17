using System;

namespace rmku.Protocol.Primitives
{
	internal struct ShortString
	{
		public string Value { get; }

		public ShortString(string value)
		{
			if(value.Length > Constants.ShortStringLength)
				throw new ArgumentException(nameof(value));

			Value = value;
		}
	}
}