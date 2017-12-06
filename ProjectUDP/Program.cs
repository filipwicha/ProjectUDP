using System;

namespace ProjectUDP
{
    class Program
    {
        static Server server;
        static Client client;

        static void Main(string[] args)
        {
            Console.WriteLine("Start as:\n1.Client\n2.Server");
            if (Convert.ToInt32(Console.ReadLine()) == 1)
            {
                client = new Client();
            }
            else
            {
                server = new Server();

            }
            Console.WriteLine("Program is about to finish");
            Console.ReadLine();
        }
    }
}