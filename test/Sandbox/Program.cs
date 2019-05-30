using System;
using System.Buffers;
using System.Buffers.Binary;
using System.IO.Pipelines;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Amp;

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
