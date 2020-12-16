using Cinema.DAL.DomainModels;
using Cinema.DAL.Infrastructure;
using Cinema.WPF.Models;
using Cinema.WPF.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Cinema.WPF.ViewModels
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TcpClient Client { get; set; }
        public CinemaContext Context { get; set; }

        public List<City> Cities { get; set; }
        private City currCity;
        public City CurrCity { get => currCity; set { currCity = value; OnPropertyChanged(); Cinemas = Context.Cinemas.Where(c => c.CityId == CurrCity.Id).ToList(); } }

        private List<DAL.DomainModels.Cinema> cinemas;
        public List<DAL.DomainModels.Cinema> Cinemas { get => cinemas; set { cinemas = value; OnPropertyChanged(); CurrCinema = Cinemas.FirstOrDefault(); } }
        private DAL.DomainModels.Cinema currCinema;
        public DAL.DomainModels.Cinema CurrCinema
        {
            get => currCinema;
            set
            {
                currCinema = value;
                OnPropertyChanged();
                Films = (from f in Context.Films
                         where (from s in Context.Sessions
                                join hl in (from h in Context.Halls
                                            where h.CinemaId == CurrCinema.Id
                                            select h) on s.HallId equals hl.Id
                                where s.Date > DateTime.Now
                                select s.FilmId).Distinct().Contains(f.Id)
                         select f).ToList();
            }
        }

        private List<Film> films;
        public List<Film> Films { get => films; set { films = value; OnPropertyChanged(); CurrFilm = Films.FirstOrDefault(); } }
        private Film currFilm;
        public Film CurrFilm 
        { 
            get => currFilm;
            set
            {
                currFilm = value;
                OnPropertyChanged();
                DateTime limit = DateTime.Today.AddDays(5);
                FilmsDates = (from s in Context.Sessions
                             join hl in (from h in Context.Halls where h.CinemaId == CurrCinema.Id select h) on s.HallId equals hl.Id
                             where s.Date > DateTime.Now && s.Date < limit && s.FilmId == CurrFilm.Id
                             select s.Date).ToList().Select(s => s.Date).Distinct().ToList();
            }
        }
        private List<DateTime> filmsDates;
        public List<DateTime> FilmsDates
        { 
            get => filmsDates; 
            set 
            {
                filmsDates = value; 
                OnPropertyChanged();
                CurrDate = FilmsDates.FirstOrDefault();
            } 
        }
        private DateTime currDate;
        public DateTime CurrDate
        {
            get => currDate;
            set
            {
                currDate = value;
                OnPropertyChanged();
                var tomorrow = DateTime.Today.AddDays(1);
                FilmsSessions = (from s in Context.Sessions
                                 join hl in (from h in Context.Halls where h.CinemaId == CurrCinema.Id select h) on s.HallId equals hl.Id
                                 where s.Date > DateTime.Today && s.Date < tomorrow && s.FilmId == CurrFilm.Id
                                 select s).GroupBy(s => s.Hall).Select(t => new SessionsForHall{ Hall = t.Key, Sessions = t.Select(s => s) }).ToList();
            }
        }

        private List<SessionsForHall> filmsSessions;
        public List<SessionsForHall> FilmsSessions { get => filmsSessions; set { filmsSessions = value; OnPropertyChanged(); } }


        public MainWindowVM()//помянять запрос с Today на Now в CurrDate (where s.Date > DateTime.Today && s.Date < tomorrow && s.FilmId == CurrFilm.Id)
        {
            Context = new CinemaContext();
            Cities = Context.Cities.ToList();
            BuyTicket = new RelayCommand(BuyTicketMethod);
            Login = new RelayCommand(LoginMethod);
            LoginW = new LoginWindow() { WindowStartupLocation = WindowStartupLocation.CenterOwner};
            LoginVM = (LoginWindowVM)LoginW.DataContext;
            ShowDetails = new RelayCommand(ShowDetailsMethod);
        }
        public RelayCommand BuyTicket { get; set; }
        private void BuyTicketMethod(object obj)
        {
            var temp = (Session)obj;
            if(LoginVM.LoggedIn)
            {
                try
                {
                    PlacesInCinemaWindow window = new PlacesInCinemaWindow();
                    var dataC = (PlacesInCinemaVM)window.DataContext;
                    if (dataC?.Client?.Connected != true)
                        return;
                    dataC.Session = temp;
                    Grid g = dataC.Places;
                    Grid.SetRow(g, 1);
                    Grid.SetColumn(g, 1);
                    window.grd1.Children.Add(g);
                    window.ShowDialog();
                    dataC.Client.Client.Close();
                    //dataC.Client.Close();
                }
                catch(Exception ex)
                {
                    //MessageBox.Show($"{ex.Message}/n/n{ex.StackTrace}", "Error");
                }
            }
            else
                MessageBox.Show("Вы не авторизованы!", "Error");
        }

        public RelayCommand Login { get; set; }
        public LoginWindow LoginW { get; set; }
        public LoginWindowVM LoginVM { get; set; }
        private string userName = "Login";
        public string UserName { get => userName; set { userName = value; OnPropertyChanged(); } }
        private void LoginMethod(object obj)
        {
            if(LoginVM.LoggedIn == false)
            {
                LoginW.ShowDialog();
                if (LoginVM.LoggedIn == true)
                {
                    UserName = LoginVM.Login;
                }
            }
        }
        
        public RelayCommand ShowDetails { get; set; }
        private void ShowDetailsMethod(object obj)
        {
            FilmWindow window = new FilmWindow() { DataContext = new FilmWindowVM {Film = CurrFilm } };
            window.ShowDialog();
        }
    }
}