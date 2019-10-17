using System;
using System.Threading.Tasks;
using rmku;

namespace Sandbox
{
	class Program
	{
		static async Task Main(string[] args)
		{
			var connection = new Connection("localhost");
			await connection.Open();

			Console.WriteLine("Estabilished connection");
			Console.WriteLine("Finished");
		}
	}
}
