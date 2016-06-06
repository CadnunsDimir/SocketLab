using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SocketCSharp.Server.ConsoleApp.Logic.ServerSocketLogic;

namespace SocketCSharp.Server.ConsoleApp.Logic
{
    public class MusicPlayer : CallBackserver
    {

        public void Play(string name)
        {
            Console.WriteLine("play song : " + name);
        }

        public void Stop()
        {
            Console.WriteLine("stop song");
        }

        public void Pause()
        {
            Console.WriteLine("pause song");
        }

    }
}
