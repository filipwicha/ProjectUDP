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
        Packet packet;

        List<int> alredyChecked = new List<int>();

        IPEndPoint serverAddres = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 211);

        public Client()
        {
            Connect();
            ClientLoop();
        }

        void DispalyClientMenu()
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
                DispalyClientMenu();

                Console.WriteLine("Try to guess number in range of {0} and {1}", clientInfo.range.Item1, clientInfo.range.Item2);
                do
                {
                    Int32.TryParse(Console.ReadLine(), out packet.leftNumber);
                }
                while (alredyChecked.Contains(packet.leftNumber));
                alredyChecked.Add(packet.leftNumber);
                Communicate();
                if(packet.answer == 0)
                {
                    Console.WriteLine("Try again :-)");
                }
                else
                {
                    Console.WriteLine("Congratulation! You've guessed the number!");
                    break;
                }
                
            }
        }

        private void Communicate()
        {
            byte[] tmp = packet.GetBytes();
            client.SendAsync(tmp, tmp.Length);
            var receivedResult = client.ReceiveAsync();
            packet.Deserialize(receivedResult.Result.Buffer);
        }

        private void Connect()
        {
            client = new UdpClient();
            client.Connect(serverAddres);

            packet = new Packet();
            Communicate();

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
            this.range = new Tuple<int, int>(leftNumber, leftNumber);
            this.sessionId = id;
            this.numberToGuess = numberToGuess;
        }
    }
}
