using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketCSharp.Server.ConsoleApp.Logic;
namespace SocketCSharp.Server.ConsoleApp
{
    class Program
    {
        private static ServerSocketLogic server;

        static void Main(string[] args)
        {
            Console.Title = "Server";
            server = new ServerSocketLogic(new MusicPlayer());
            server.Setup();
            Console.ReadKey();       
        }
    }
}
