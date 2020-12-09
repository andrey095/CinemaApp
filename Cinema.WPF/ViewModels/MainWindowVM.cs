using Cinema.DAL.DomainModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

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
                //Films = Context.Films.Where(f => f.Sessions.Where(s => s.Hall.CinemaId == CurrCinema.CityId)); 
                //Films = from t in Context.Films
                //        where t
                //Films = from h in Context.Halls
                //        where h.CinemaId == CurrCinema.Id
                //        from s in Context.Sessions
                //        where s.HallId == h.Id
                //        from f in Context.Films
                //        where f.Sessions.Add
                //Films = from f in Context.Films
                //        join se in (from s in Context.Sessions join hl in (from h in Context.Halls where h.CinemaId == CurrCinema.Id select h) on s.HallId equals hl.Id select s.FilmId).Distinct() on f.Id equals se
                //        select f;

                int n1 = 0;
                Films = (from f in Context.Films
                         where (from s in Context.Sessions join hl in (from h in Context.Halls where h.CinemaId == CurrCinema.Id select h) on s.HallId equals hl.Id where s.Date > DateTime.Now select s.FilmId).Distinct().Contains(f.Id)
                         select f).ToList();
                int n = 0;
                //where s.HallId == h.Id
                //from f in Context.Films
                //where f.Sessions.Add


                //IQueryable halls = Context.Halls.Where(h => h.CinemaId == CurrCinema.Id);
                //var sess = Context.Sessions.Intersect(Context.Halls.Where(h => h.CinemaId == CurrCinema.Id));
                //var sess1 = Context.Sessions.Join(,) .Intersect(Context.Halls.Where(h => h.CinemaId == CurrCinema.Id))

                //Films = Context.Films.Where(f => f.Sessions.Contains(f.));
            } 
        }

        //  select distinct s.FilmId
        //  from Sessions s join(select h.Id
        //                from Cinemas c join Halls h on c.Id= h.CinemaId
        //                where c.Id = 1) hl on s.HallId = hl.Id
        //  where s.Date > GETDATE()


        public List<Film> Films { get; set; }

        public MainWindowVM()
        {
            //Client = new TcpClient("127.0.0.1", 14888);
            Context = new CinemaContext();
            Cities = Context.Cities.ToList();
            Films = (from f in Context.Films
                     where (from s in Context.Sessions join hl in (from h in Context.Halls where h.CinemaId == 1 select h) on s.HallId equals hl.Id where s.Date > DateTime.Now select s.FilmId).Distinct().Contains(f.Id)
                     select f).ToList();
        }
    }
    public class ImageBytesToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            MemoryStream ms = new MemoryStream((byte[])value);
            var t = new BitmapImage();
            t.BeginInit();
            t.StreamSource = ms;
            t.EndInit();
            return t;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
