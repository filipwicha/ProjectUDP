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

                if (clients.Exists(x => x.sessionId == packet.sessionId))
                {
                    Console.WriteLine("Server received packet from " + packet.sessionId);
                    ClientInfo client = clients.Find(x => x.sessionId == packet.sessionId);
                    if (client.numberToGuess == packet.leftNumber)
                    {
                        packet.answer = 1;
                        Console.WriteLine("Client " + packet.sessionId + " guesed number!");
                    }
                }
                else
                {
                    Console.WriteLine("Client connected!");

                    packet.sessionId = Guid.NewGuid().ToString();
                    packet.leftNumber = 1;
                    packet.rightNumber = 5;
                    Random rand = new Random();
                    clients.Add(new ClientInfo(packet.leftNumber, packet.rightNumber, packet.sessionId, rand.Next(1, 5)));
                }

                byte[] tmp = packet.GetBytes();
                server.SendAsync(tmp, tmp.Length,receivedResult.Result.RemoteEndPoint);
            }
        }
    }
}