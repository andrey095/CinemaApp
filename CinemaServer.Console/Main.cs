using Cinema.DAL.DomainModels;
using CinemaServer.Console.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base;
using TableDependency.SqlClient.Base.Enums;

namespace CinemaServer.Console
{
    class Program
    {
        static TcpListener server;
        static List<ClientsSessions> ClientsSessions;

        static CinemaContext context;
        static SqlTableDependency<Ticket> sqlTableTicketDependency;
        static void Main(string[] args)
        {
            context = new CinemaContext();
            ClientsSessions = new List<ClientsSessions>();
            InitNotifyChangeTicket();
            Task.Run(() =>
            {
                foreach (var item in ClientsSessions)
                {
                    for (int i = item.Clients.Count; i > 0; i++)
                    {
                        if (item.Clients[i].Connected == false)
                            item.Clients.Remove(item.Clients[i]);
                    }
                }
            });

            server = new TcpListener(IPAddress.Parse(ConfigurationManager.AppSettings["LocalIpAddress"]), int.Parse(ConfigurationManager.AppSettings["LocalPort"]));
            System.Console.WriteLine("Server started!!!");
            server.Start();
            do
            {
                TcpClient client = server.AcceptTcpClient();
                Task.Run(() => 
                {
                    System.Console.WriteLine($"connected: {client.Client.RemoteEndPoint}");
                    ClientBuy(client);
                });
            } while (true);
        }
        static void InitNotifyChangeTicket()
        {
            var mapper_g = new ModelToTableMapper<Ticket>();
            mapper_g.AddMapping(t => t.Id, "Id");
            mapper_g.AddMapping(t => t.Date, "Date");
            mapper_g.AddMapping(t => t.SessionId, "SessionId");
            mapper_g.AddMapping(t => t.EmployeeId, "EmployeeId");
            mapper_g.AddMapping(t => t.UserId, "UserId");
            mapper_g.AddMapping(t => t.Row, "Row");
            mapper_g.AddMapping(t => t.Place, "Place");
            mapper_g.AddMapping(t => t.Price, "Price");
            mapper_g.AddMapping(t => t.IsReturned, "IsReturned");
            sqlTableTicketDependency = new SqlTableDependency<Ticket>(context.Database.Connection.ConnectionString, "Tickets", mapper: mapper_g);
            sqlTableTicketDependency.OnChanged += DepTicket_OnChanged;
            sqlTableTicketDependency.Start();
        }
        static void DepTicket_OnChanged(object sender, TableDependency.SqlClient.Base.EventArgs.RecordChangedEventArgs<Ticket> e)
        {
            var changedEntity = e.Entity;
            switch (e.ChangeType)
            {
                case ChangeType.Update:
                case ChangeType.Insert:
                    var currSess = ClientsSessions.FirstOrDefault(c => c.Session.Id == e.Entity.SessionId);
                    if(currSess != null)
                    {
                        var arr = Encoding.Unicode.GetBytes($"{e.Entity.Row}/{e.Entity.Place}");
                        foreach (var item in currSess.Clients)
                        {
                            using (var ns = item.GetStream())
                            {
                                ns.Write(arr, 0, arr.Length);
                            }
                        }
                    }
                    break;
            }
        }
        static void ClientBuy(TcpClient client)
        {
            string cl = client.Client.RemoteEndPoint.ToString();
            var currSess = ClientsSessions.FirstOrDefault();
            try
            {
                using (var ns = client.GetStream())
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    int id = (int)bf.Deserialize(ns);
                    Session session = context.Sessions.FirstOrDefault(s => s.Id == id);
                    System.Console.WriteLine($"{cl}: {session?.Id}");
                    currSess = ClientsSessions.FirstOrDefault(c => c.Session.Id == session.Id);
                    if (currSess == null)
                    {
                        ClientsSessions.Add(new ClientsSessions(session));
                        currSess = ClientsSessions.FirstOrDefault(c => c.Session.Id == session.Id);
                    }
                    currSess.Clients.Add(client);
                    byte[] arr;
                    foreach (var item in currSess.Places)
                    {
                        arr = Encoding.Unicode.GetBytes($"{item.row}/{item.place}");
                        ns.Write(arr, 0, arr.Length);
                    }
                    arr = new byte[20];
                    do
                    {
                        int len = ns.Read(arr, 0, arr.Length);
                        string loc = Encoding.Unicode.GetString(arr, 0, len);
                        if (loc.Length > 1)
                        {
                            System.Console.WriteLine($"{cl}: {loc}");
                            foreach (var item in currSess.Clients)
                            {
                                if (item == client)
                                    continue;
                                var ns1 = item.GetStream();
                                ns1.Write(arr, 0, len);
                            }
                            int row = int.Parse(loc.Substring(0, loc.IndexOf('/')));
                            int place = int.Parse(loc.Substring(loc.IndexOf('/') + 1));
                            if (currSess.Places.FirstOrDefault(p => p.row == row && p.place == place) == default)
                                currSess.Places.Add((row, place));
                            else
                                currSess.Places.Remove((row, place));
                        }
                    } while (client.Connected);
                    System.Console.WriteLine($"{cl}: disconnected");
                    currSess.Clients.Remove(client);
                }
            }
            catch(Exception ex)
            {
                System.Console.WriteLine($"problem with: {cl}");
                currSess.Clients.Remove(client);
            }
        }
    }
}
