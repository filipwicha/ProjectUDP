using System;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Collections.Generic;

namespace ProjectUDP
{
    public class Client
    {
        UdpClient client;
        ClientInfo clientInfo;

        List<int> alredyChecked = new List<int>();

        IPEndPoint serverAddres = new IPEndPoint(IPAddress.Parse("25.21.58.123"), 211);

        public Client()
        {
            Connect();
            ClientLoop();
        }

        void DisplayCheckedList()
        {
            Console.Write("The numbers, you've already checked, are:");
            foreach (var current in alredyChecked)
            {
                Console.Write(" {0}", current);
            }
            Console.Write("\n\n");

        }

        private void ClientLoop()
        {
            while (true)
            {
                Console.Clear();
                DisplayCheckedList();
                Packet packet = new Packet();
                packet.sessionId = clientInfo.sessionId;

                Console.WriteLine("Try to guess number in range of {0} and {1}", clientInfo.range.Item1, clientInfo.range.Item2);
                do
                {
                    Int32.TryParse(Console.ReadLine(), out packet.leftNumber);
                }
                while (alredyChecked.Contains(packet.leftNumber));

                alredyChecked.Add(packet.leftNumber);

                Communicate(packet);
                if(packet.answer == 0)
                {
                    Console.WriteLine("Try again :-)");
                    System.Threading.Thread.Sleep(500);
                }
                else
                {
                    Console.WriteLine("Congratulation! You've guessed the number!");
                    client.Close();
                    Console.WriteLine("Connection closed!");
                    break;
                }
                
            }
        }

        private void Communicate(Packet packet)
        {
            client.SendAsync(packet.Bytes, packet.length);
            var receivedResult = client.ReceiveAsync();
            Console.WriteLine("ACK received");
            receivedResult = client.ReceiveAsync();
            packet.Deserialize(receivedResult.Result.Buffer);
            Packet ack = new Packet();
            ack.answer = 2;
            ack.sessionId = packet.sessionId;
            client.SendAsync(ack.Bytes, ack.length);
        }

        private void Connect()
        {
            client = new UdpClient();
            client.Connect(serverAddres);
            Console.WriteLine("Client connected!");

            Packet packet = new Packet();
            Communicate(packet);

            clientInfo = new ClientInfo(packet.leftNumber, packet.rightNumber, packet.sessionId);
        }
    }

    public class ClientInfo
    {
        public Tuple<int, int> range;
        public string sessionId = "";
        public int numberToGuess;

        public ClientInfo(int leftNumber, int rightNumber, string id, int numberToGuess = 0)
        {
            this.range = new Tuple<int, int>(leftNumber, rightNumber);
            this.sessionId = id;
            this.numberToGuess = numberToGuess;
        }
    }
}
