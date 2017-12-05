using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ProjectUDP
{
    public class Server
    {
        const int DEFAULT_PORT = 211;
        UdpClient server;

        public Server()
        {
            server = new UdpClient(new IPEndPoint(IPAddress.Parse("127.0.0.1"), DEFAULT_PORT));
            ServerLoop();
        }

        private async void ServerLoop()
        {
            while (true)
            {
                var receivedResult = await server.ReceiveAsync();
                Console.WriteLine(receivedResult.RemoteEndPoint.Port);
                var datagram = Encoding.ASCII.GetBytes("Server message");
                await server.SendAsync(datagram, datagram.Length, receivedResult.RemoteEndPoint);
            }
        }
    }
}
