using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Amp.Tests
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void TestMethod1()
		{
			Span<byte> start = Convert.FromBase64String("AAkAAAHDDGNhcGFiaWxpdGllc0YAAADHEnB1Ymxpc2hlcl9jb25maXJtc3QBGmV4Y2hhbmdlX2V4Y2hhbmdlX2JpbmRpbmdzdAEKYmFzaWMubmFja3QBFmNvbnN1bWVyX2NhbmNlbF9ub3RpZnl0ARJjb25uZWN0aW9uLmJsb2NrZWR0ARNjb25zdW1lcl9wcmlvcml0aWVzdAEcYXV0aGVudGljYXRpb25fZmFpbHVyZV9jbG9zZXQBEHBlcl9jb25zdW1lcl9xb3N0AQ9kaXJlY3RfcmVwbHlfdG90AQxjbHVzdGVyX25hbWVTAAAAD3JhYmJpdEBBZG1pbi1IUAljb3B5cmlnaHRTAAAALkNvcHlyaWdodCAoQykgMjAwNy0yMDE5IFBpdm90YWwgU29mdHdhcmUsIEluYy4LaW5mb3JtYXRpb25TAAAANkxpY2Vuc2VkIHVuZGVyIHRoZSBNUEwuICBTZWUgaHR0cHM6Ly93d3cucmFiYml0bXEuY29tLwhwbGF0Zm9ybVMAAAAPRXJsYW5nL09UUCAyMi4wB3Byb2R1Y3RTAAAACFJhYmJpdE1RB3ZlcnNpb25TAAAABjMuNy4xNwAAAA5BTVFQTEFJTiBQTEFJTgAAAAVlbl9VUw==");
			var res = MethodReader.ParseStart(start);

			Assert.AreEqual(9, res.VersionMajor);
			Assert.AreEqual(1, res.VersionMinor);
		}
	}
}
