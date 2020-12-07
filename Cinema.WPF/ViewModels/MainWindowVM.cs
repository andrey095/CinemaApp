using Cinema.DAL.DomainModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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
        public DAL.DomainModels.Cinema CurrCinema { get => currCinema; set { currCinema = value; OnPropertyChanged(); } }

        public List<Film> Films { get; set; }

        public MainWindowVM()
        {
            //Client = new TcpClient("127.0.0.1", 14888);
            Context = new CinemaContext();
            Cities = Context.Cities.ToList();

        }
    }
}
