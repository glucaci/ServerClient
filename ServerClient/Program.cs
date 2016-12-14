using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ServerClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var serverTask = Server.Start();

            Client.Connect(Server.Address, Server.Port).GetAwaiter().GetResult();

            serverTask.GetAwaiter().GetResult();
        }
    }


    public class Client
    {
        public static async Task Connect(IPAddress address, int port)
        {
            var client = new TcpClient();
            await client.ConnectAsync(address, port);
        }
    }

    public class Server
    {
        public static IPAddress Address { get; } = IPAddress.Loopback;
        public static int Port { get; } = 6600;

        public static async Task Start()
        {
            var listner = new TcpListener(Address, Port);
            listner.Start();

            Console.WriteLine($"Listening on {Address}:{Port}");

            await listner.AcceptTcpClientAsync();
            Console.WriteLine("Client connected.");

            Console.WriteLine("Shuting down...");
            listner.Stop();
        }
    }
}
