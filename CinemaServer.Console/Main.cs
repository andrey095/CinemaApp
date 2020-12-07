using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CinemaServer.Console
{
    class Program
    {
        static TcpListener server;
        static List<TcpClient> clients;
        static async Task Main(string[] args)
        {
            server = new TcpListener(IPAddress.Parse("127.0.0.1"), 14888);
            server.Start();
            do
            {
                TcpClient client = await server.AcceptTcpClientAsync();
                clients.Add(client);
                await Task.Factory.StartNew(() => 
                {
                    ClientBuy(client.GetStream());
                }, TaskCreationOptions.LongRunning);
            } while (true);
        }

        static void ClientBuy(NetworkStream ns)
        {

        }
    }
}
