using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static SocketCSharp.Server.ConsoleApp.Logic.ServerSocketLogic;

namespace SocketCSharp.Server.ConsoleApp.Logic
{
    public class ServerSocketLogic : CallBackserver
    {
        private readonly Socket _serverSocket;
        private List<Socket> _clientSockets;
        private byte[] _buffer;
        private CallBackserver _callbackPlayer;

        public ServerSocketLogic(CallBackserver callback = null)
        {
            _callbackPlayer = callback ?? this;
            _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _clientSockets = new List<Socket>();
            _buffer = new byte[1024];
        }

        internal void Setup()
        {
            Console.WriteLine("Starting Server...");
            _serverSocket.Bind(new IPEndPoint(IPAddress.Any, 100));
            _serverSocket.Listen(1);
            BeginAccept();
        }

        internal void AcceptCallback(IAsyncResult ar)
        {
            var socket = _serverSocket.EndAccept(ar);
            _clientSockets.Add(socket);
            Console.WriteLine("Clients Connected");
            BeginReceive(socket);
            BeginAccept();
        }

        private void BeginReceive(Socket socket)
        {
            socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCAllback), socket);
        }

        private void BeginAccept()
        {
            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        private void ReceiveCAllback(IAsyncResult ar)
        {
            var socket = (Socket)ar.AsyncState;
            var received = socket.EndReceive(ar);
            var dataBuffer = new byte[received];
            Array.Copy(_buffer, dataBuffer, received);

            var text = Encoding.ASCII.GetString(dataBuffer);

            Console.WriteLine("Text received : " + text);

            var response = string.Empty;
            if (text.ToLower() != "play song")
            {
                response = "Invalid Request";
            }
            else
            {
                _callbackPlayer.Play(text.Replace("play song", ""));
                response = DateTime.Now.ToLongDateString();
            }
            var data = Encoding.ASCII.GetBytes(response);
            socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);
            BeginReceive(socket);
        }

        private void SendCallback(IAsyncResult ar)
        {
            var socket = (Socket)ar.AsyncState;
            var received = socket.EndReceive(ar);
        }

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

        public interface CallBackserver
        {
            void Play(string name);
            void Stop();
            void Pause();
        }
    }
}
