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
            server = new UdpClient(new IPEndPoint(IPAddress.Parse("25.21.58.123"), DEFAULT_PORT));
            ServerLoop();
        }

        private void ServerLoop()
        {
            while (true)
            {
                var receivedResult = server.ReceiveAsync();
                Packet packet = new Packet();
                packet.Deserialize(receivedResult.Result.Buffer);
                Packet ack = new Packet();
                ack.answer = 2;
                ack.sessionId = packet.sessionId;
                server.SendAsync(ack.Bytes, ack.length, receivedResult.Result.RemoteEndPoint);

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

                server.SendAsync(packet.Bytes, packet.length, receivedResult.Result.RemoteEndPoint);
                receivedResult = server.ReceiveAsync();
                Console.WriteLine("ACK received");
            }
        }
    }
}