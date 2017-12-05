using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Net;
namespace ProjectUDP
{
    public class Client
    {
        const int DEFAULT_PORT = 211;
        UdpClient client;
        IPEndPoint serverAddres = new IPEndPoint(IPAddress.Parse("127.0.0.1"), DEFAULT_PORT);
        public Client()
        {
            client = new UdpClient();
            client.Connect(serverAddres);
            ClientLoop();
        }

        private async void ClientLoop()
        {
            while (true)
            {
                var datagram = Encoding.ASCII.GetBytes("Client message");
                await client.SendAsync(datagram, datagram.Length);
                var receivedResult = await client.ReceiveAsync();
                Console.WriteLine(Encoding.ASCII.GetString(receivedResult.Buffer));
            }
        }
    }
}
