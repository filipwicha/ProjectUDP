using System;
using System.Net;
using System.Text;
using System.Net.Sockets;

namespace ProjectUDP
{
    public class Client
    {
        UdpClient client;
        ClientInfo clientInfo;
        Packet packet;

        IPEndPoint serverAddres = new IPEndPoint(IPAddress.Parse("25.21.58.123"), 211);

        public Client()
        {
            Connect();
            ClientLoop();
        }

        private void ClientLoop()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Try to guess number in range of {0} and {1}", clientInfo.range.Item1, clientInfo.range.Item2);
                Int32.TryParse(Console.ReadLine(), out packet.leftNumber);
                Communicate();
                Console.WriteLine("Guesed" + packet.answer);
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
