using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketCSharp.Server.App.Logic;
namespace SocketCSharp.Server.App
{
    class Program
    {
        private static ServerSocketLogic server;

        static void Main(string[] args)
        {
            Console.Title = "Server";
            server = new ServerSocketLogic();
            server.Setup();
            Console.ReadKey();       
        }

        
    }
}
