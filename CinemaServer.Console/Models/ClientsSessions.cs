using Cinema.DAL.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CinemaServer.Console.Models
{
    public class ClientsSessions
    {
        public Session Session { get; set; }
        public List<TcpClient> Clients { get; set; }

        public ClientsSessions(Session session)
        {
            Session = session;
            Clients = new List<TcpClient>();
        }
    }
}
