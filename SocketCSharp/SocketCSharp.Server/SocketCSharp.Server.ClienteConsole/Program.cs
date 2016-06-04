using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketCSharp.Server.ClienteConsole
{
    class Program
    {
        private static Socket _clientSocket;
        private static int attempts;

        static void Main(string[] args)
        {
            Console.Title = "Client";
            _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            LoopConnect();
            SendLoop();
            Console.ReadLine();
        }

        private static void SendLoop()
        {
            while (_clientSocket.Connected)
            {
                Console.WriteLine("Enter your request:");
                var request = Console.ReadLine();
                var buffer = Encoding.ASCII.GetBytes(request);
                _clientSocket.Send(buffer);
                var responseBuffer = new byte[1024];
                var rec = _clientSocket.Receive(responseBuffer);
                var data = new byte[rec];
                Array.Copy(responseBuffer, data, rec);
                Console.WriteLine("Received: "+ Encoding.ASCII.GetString(data));
            }
            Console.WriteLine("Disconected");
        }

        private static void LoopConnect()
        {
            while (!_clientSocket.Connected)
            {
                try
                {
                    attempts++;
                    _clientSocket.Connect(IPAddress.Loopback, 100);
                }
                catch (SocketException)
                {
                    Console.Clear();
                    Console.WriteLine("Connection Attemps: : " +attempts);
                }
            }
            Console.Clear();
            Console.WriteLine("Connected");
        }
    }
}
