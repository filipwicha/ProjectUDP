using System;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Collections.Generic;

namespace ProjectUDP
{
    public class Server
    {
        const int DEFAULT_PORT = 211;
        UdpClient server;

        List<ClientInfo> clients = new List<ClientInfo>();
        

        public Server()
        {
            server = new UdpClient(new IPEndPoint(IPAddress.Parse("127.0.0.1"), DEFAULT_PORT));
            ServerLoop();
        }

        private void ServerLoop()
        {
            while (true)
            {
                var receivedResult = server.ReceiveAsync();
                Packet packet = new Packet();
                packet.Deserialize(receivedResult.Result.Buffer);

                if (packet.sessionId == "")
                {
                    packet.sessionId = Guid.NewGuid().ToString();
                    clients.Add(new ClientInfo(1, 5, packet.sessionId));
                }

                Console.WriteLine("Server received packet:" + packet.leftNumber);
                var datagram = Encoding.ASCII.GetBytes("Server message");
                server.SendAsync(datagram, datagram.Length, receivedResult.Result.RemoteEndPoint);
            }
        }
    }
}