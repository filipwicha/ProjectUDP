using System;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ProjectUDP
{
    public class Client
    {
        UdpClient client;

        IPEndPoint serverAddres = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 211);
        public Client()
        {
            client = new UdpClient();
            client.Connect(serverAddres);
            ClientLoop();
        }

        private void ClientLoop()
        {
            while (true)
            {
                string line = Console.ReadLine();
                Packet packet = new Packet();
                Int32.TryParse(line, out packet.leftNumber);
                packet.rightNumber = 7;
                packet.sessionId = 10210412;
                packet.answer = 1;
                byte[] tmp = packet.GetBytes();
                client.SendAsync(tmp, tmp.Length);
                var receivedResult = client.ReceiveAsync();
                Console.WriteLine(Encoding.ASCII.GetString(receivedResult.Result.Buffer));
            }
        }
    }
}
