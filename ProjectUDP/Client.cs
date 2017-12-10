using System;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Collections.Generic;

namespace ProjectUDP
{
    public class Client
    {
        UdpClient client; //create client
        int leftNumber = 0; //client information about left range lim
        int rightNumber = 0; //client information about right range lim
        string sessionId = ""; //holds sessionId

        List<int> alredyChecked = new List<int>(); //list of numbers that have been already checked

        IPEndPoint serverAddres = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 211);   //create server endpoint

        public Client()
        {
            Connect(); //connect and set session id
            ClientLoop(); //function of a client
        }

        void DisplayCheckedList() //shows list of sorted numbers that have been checked
        {
            alredyChecked.Sort();
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
                Packet packet = new Packet(); //create packet
                packet.sessionId = sessionId;

                Console.WriteLine("Try to guess number between {0} and {1}.", leftNumber, rightNumber);
                do
                {
                    Int32.TryParse(Console.ReadLine(), out packet.numberToGues);
                }
                while (alredyChecked.Contains(packet.numberToGues));

                alredyChecked.Add(packet.numberToGues); //add number to already checked list

                Communicate(packet);  //send packet and wait for ACK 

                leftNumber = packet.leftNumber;
                rightNumber = packet.rightNumber;

                if (packet.answer == 0)
                {
                    Console.WriteLine("Try again :-)");
                    System.Threading.Thread.Sleep(500);
                }
                else
                {
                    Console.WriteLine("Congratulation! You've guessed the number!");
                    client.Close(); //end connection
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

            Packet ack = new Packet(sessionId);
            client.SendAsync(ack.Bytes, ack.length);
        }

        private void Connect()
        {
            client = new UdpClient();
            client.Connect(serverAddres);
            Console.WriteLine("Client connected!");
            Packet packet = new Packet();
            Console.WriteLine("Send number L");
            Int32.TryParse(Console.ReadLine(), out packet.leftNumber);
            Communicate(packet);

            sessionId = packet.sessionId;
            leftNumber = packet.leftNumber;
            rightNumber = packet.rightNumber;
        }
    }
}
