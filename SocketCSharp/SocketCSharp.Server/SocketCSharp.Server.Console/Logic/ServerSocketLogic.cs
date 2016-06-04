﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketCSharp.Server.App.Logic
{
    public class ServerSocketLogic
    {
        private readonly Socket _serverSocket;
        private List<Socket> _clientSockets;
        private byte[] _buffer;

        public ServerSocketLogic()
        {
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
    }
}
