using System;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Collections.Generic;

namespace ProjectUDP
{
    public class Server
    {
        UdpClient server;
        List<string> clients = new List<string>();

        int leftNumber = 0;
        int rightNumber = 0;
        int numberToGues = 0;

        public Server()
        {
            server = new UdpClient(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 211));
            ServerLoop();
        }

        private void ServerLoop()
        {
            while (true)
            {
                var receivedResult = server.ReceiveAsync();
                Packet packet = new Packet();
                packet.Deserialize(receivedResult.Result.Buffer);

                if (packet.answer == 2)
                {
                    Console.WriteLine("ACK received");
                    continue;
                }

                Packet ack = new Packet(packet.sessionId);
                server.SendAsync(ack.Bytes, ack.length, receivedResult.Result.RemoteEndPoint);

                if (clients.Exists(x => x == packet.sessionId))
                {
                    Console.WriteLine("Server received packet from " + packet.sessionId);
                    if (numberToGues == packet.numberToGues)
                    {
                        packet.answer = 1;
                        Console.WriteLine("Client " + packet.sessionId + " guesed number!");
                    }
                }
                else
                {
                    Console.WriteLine("Client connected!");
                    packet.sessionId = Guid.NewGuid().ToString();

                    if(clients.Count == 0)
                    {
                        leftNumber = packet.leftNumber;
                    }
                    else if(clients.Count == 1)
                    {
                        rightNumber = packet.leftNumber;
                    }

                    clients.Add(packet.sessionId);

                    if (clients.Count == 2)
                    {
                        Random rand = new Random();
                        numberToGues = rand.Next(leftNumber, rightNumber);
                    }
                }
                
                packet.leftNumber = leftNumber;
                packet.rightNumber = rightNumber; 

                server.SendAsync(packet.Bytes, packet.length, receivedResult.Result.RemoteEndPoint);
                Console.WriteLine("Packet sended");

                Console.WriteLine("Number to gues: " + numberToGues);
            }
        }
    }
}