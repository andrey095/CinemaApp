using Cinema.DAL.DomainModels;
using Cinema.DAL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Cinema.WPF.ViewModels
{
    public class PlacesInCinemaVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Session session;
        public Session Session { get => session; set { session = value; OnPropertyChanged(); Places = CreateHall(); } }
        public User User { get; set; }

        private CinemaContext Context;
        public TcpClient Client;
        public NetworkStream Stream;

        public Grid Places { get; set; }
        public List<List<Button>> ArrBtn;
        private ObservableCollection<Button> tickets;
        public ObservableCollection<Button> Tickets { get => tickets; set { tickets = value; OnPropertyChanged(); } }
        public decimal Price { get; set; }

        Button btntest;
        public PlacesInCinemaVM()
        {
            try
            {
                Client = new TcpClient(ConfigurationManager.AppSettings["ServerIpAddress"], int.Parse(ConfigurationManager.AppSettings["ServerPort"]));
                Stream = Client.GetStream();
                Tickets = new ObservableCollection<Button>();
                Context = new CinemaContext();
                ArrBtn = new List<List<Button>>();
                GetPlace = new RelayCommand(GetPlaceMethod);
                btntest = new Button() { Content = "test" };
                BuyTickets = new RelayCommand(BuyTicketsMethod);
                Closing = new RelayCommand(ClosingMethod);
                Some();
            }
            catch (SocketException ex)
            {
                MessageBox.Show("Ошибка подключение к серверу", "Error");
            }
        }
        private async Task Some()
        {
            byte[] arr = new byte[20];
            do
            {
                int len = await Stream.ReadAsync(arr, 0, arr.Length);
                string loc = Encoding.Unicode.GetString(arr, 0, len);
                if (loc.Length > 1)
                {
                    int row = int.Parse(loc.Substring(0, loc.IndexOf("/"))) - 1;
                    int place = int.Parse(loc.Substring(loc.IndexOf("/") + 1)) - 1;
                    ArrBtn[row][place].IsEnabled = !ArrBtn[row][place].IsEnabled;
                }
            } while (true);
        }

        private Grid CreateHall()
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(Stream, Session.Id);

            var hallSeats = Context.HallSeats.Where(h => h.HallId == Session.HallId).OrderBy(h => h.Row).ToList();
            var tickects = Context.Tickets.Where(t => t.SessionId == Session.Id && t.IsReturned == false).ToList();
            int max = hallSeats.Max(h => h.Places);
            Grid grid = new Grid();
            for (int i = 0; i < max; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int i = 0; i < hallSeats.Count; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition());
            }
            int offset = 0;
            for (int r = 0; r < hallSeats.Count; r++)
            {
                if (hallSeats[r].Places < max)
                {
                    offset = (max - hallSeats[r].Places) / 2;
                }
                ArrBtn.Add(new List<Button>());
                for (int i = 0; i < hallSeats[r].Places; i++)
                {
                    Button btn = new Button();
                    if (hallSeats[r].PlaceType)
                    {
                        btn.BorderBrush = Brushes.DarkRed;
                        btn.BorderThickness = new Thickness(2);
                    }
                    btn.Content = $"{hallSeats[r].Row: ##}/{i + 1: ##}";
                    btn.Tag = Session.Price + (hallSeats[r].PlaceType ? Session.AddLux : 0);
                    btn.Command = GetPlace;
                    btn.CommandParameter = btn;
                    btn.ToolTip = new TextBlock { Text = $"Ряд:{hallSeats[r].Row}  Место:{i + 1}  Цена:{Session.Price + (hallSeats[r].PlaceType ? Session.AddLux : 0): 0 грн}" };
                    Grid.SetRow(btn, hallSeats[r].Row - 1);
                    Grid.SetColumn(btn, i + offset);
                    grid.Children.Add(btn);
                    ArrBtn[r].Add(btn);
                }
            }
            foreach (var t in tickects)
            {
                ArrBtn[t.Row - 1][t.Place - 1].IsEnabled = false;
            }
            return grid;
        }

        public RelayCommand GetPlace { get; set; }
        private void GetPlaceMethod(object obj)
        {
            Button btn = obj as Button;
            var arr = Encoding.Unicode.GetBytes((string)((Button)obj).Content);
            Stream.Write(arr, 0, arr.Length);
            if (btn.Background != Brushes.Green)
            {
                btn.Background = Brushes.Green;
                Tickets.Add(btn);
                Price += (decimal)btn.Tag;
            }
            else
            {
                btn.Background = btntest.Background;
                Tickets.Remove(btn);
                Price -= (decimal)btn.Tag;
            }
            OnPropertyChanged("Price");
        }

        public RelayCommand BuyTickets { get; set; }
        private void BuyTicketsMethod(object obj)
        {
            if(MessageBox.Show("Вы подтверждаете покупку?", "Подтверждение покупки", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                for (int i = Tickets.Count - 1; i >= 0; i--)
                {
                    string temp = (string)Tickets[i].Content;
                    int row = int.Parse(temp.Substring(0, temp.IndexOf('/')));
                    int place = int.Parse(temp.Substring(temp.IndexOf('/') + 1));
                    Context.Tickets.Add(new Ticket() { Date = DateTime.Now, SessionId = Session.Id, UserId = User.Id, Row = row, Place = place, Price = (decimal)Tickets[i].Tag });
                    Context.SaveChanges();
                    GetPlaceMethod(Tickets[i]);
                }
            }
        }

        public RelayCommand Closing { get; set; }
        private void ClosingMethod(object obj)
        {
            for (int i = Tickets.Count - 1; i >= 0; i--)
            {
                GetPlaceMethod(Tickets[i]);
            }
        }
    }
}
