using Cinema.DAL.DomainModels;
using Cinema.DAL.Infrastructure;
using System;
using System.Collections.Generic;
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
        public Grid Places { get; set; }
        private CinemaContext Context;
        public TcpClient Client;
        public NetworkStream Stream;
        public List<List<Button>> ArrBtn;
        Thread main;
        public PlacesInCinemaVM()
        {
            try
            {
                main = Thread.CurrentThread;
                Client = new TcpClient(ConfigurationManager.AppSettings["ServerIpAddress"], int.Parse(ConfigurationManager.AppSettings["ServerPort"]));
                Stream = Client.GetStream();
                Context = new CinemaContext();
                //sw = new StreamWriter(Stream, Encoding.Unicode);
                ArrBtn = new List<List<Button>>();
                //Task.Run(() =>
                //{
                //    byte[] arr = new byte[20];
                //    do
                //    {
                //        int len = Stream.Read(arr, 0, arr.Length);
                //        string loc = Encoding.Unicode.GetString(arr, 0, len);
                //        if (loc.Length > 1)
                //        {
                //            int row = int.Parse(loc.Substring(0, loc.IndexOf("/"))) - 1;
                //            int place = int.Parse(loc.Substring(loc.IndexOf("/") + 1)) - 1;
                //            //user1 @ukr.net
                //            //Dispatcher.CurrentDispatcher.Invoke(() => ArrBtn[row][place].IsEnabled = !ArrBtn[row][place].IsEnabled);
                //            new Action(() => ArrBtn[row][place].IsEnabled = !ArrBtn[row][place].IsEnabled).Invoke();
                //            //new Thread(() => {  ArrBtn[row][place].IsEnabled = !ArrBtn[row][place].IsEnabled; });
                //            //main.Start();
                //            //Dispatcher.CurrentDispatcher.Invoke(() => );
                //            //Dispatcher.CurrentDispatcher.Invoke(() => { main.Join(); ArrBtn[row][place].IsEnabled = !ArrBtn[row][place].IsEnabled; });
                //            //Dispatcher.Run();//.Invoke(() => ArrBtn[row][place].IsEnabled = !ArrBtn[row][place].IsEnabled);
                //        }
                //    } while (true);
                //});
                GetPlace = new RelayCommand(GetPlaceMethod);
                btntest = new Button() { Content = "test" };
                Some();
            }
            catch(SocketException ex)
            {
                MessageBox.Show("Ошибка подключение к серверу", "Error");
            }
        }
        private async Task Some()
        {
            byte[] arr = new byte[20];
            //StreamReader sr = new StreamReader(Stream, Encoding.Unicode);
            do
            {
                int len = await Stream.ReadAsync(arr, 0, arr.Length);
                string loc = Encoding.Unicode.GetString(arr, 0, len);
                //string loc = await sr.ReadLineAsync();
                if (loc.Length > 1)
                {
                    int row = int.Parse(loc.Substring(0, loc.IndexOf("/"))) - 1;
                    int place = int.Parse(loc.Substring(loc.IndexOf("/") + 1)) - 1;
                    //user1@ukr.net
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
                        btn.Background = Brushes.Green;
                        btn.BorderThickness = new Thickness(2);
                    }
                    btn.Content = $"{hallSeats[r].Row: ##}/{i + 1: ##}";
                    btn.Command = GetPlace;
                    btn.CommandParameter = btn;
                    Grid.SetRow(btn, hallSeats[r].Row - 1);
                    Grid.SetColumn(btn, i + offset);
                    grid.Children.Add(btn);
                    ArrBtn[r].Add(btn);
                }
            }
            foreach (var t in tickects)
            {
                ArrBtn[t.Row][t.Place].IsEnabled = false;
            }
            return grid;
        }

        Button btntest;
        //StreamWriter sw;
        public RelayCommand GetPlace { get; set; }
        private void GetPlaceMethod(object obj)
        {
            Button btn = obj as Button;
            //btn.IsEnabled = !btn.IsEnabled
            var arr = Encoding.Unicode.GetBytes((string)((Button)obj).Content);
            Stream.Write(arr, 0, arr.Length);
            //sw.WriteLine((string)((Button)obj).Content);
            if (btn.Background == btntest.Background)
                btn.Background = Brushes.Green;
            else
                btn.Background = btntest.Background.Clone();
        }
    }
}
