using Cinema.DAL.DomainModels;
using Cinema.DAL.Infrastructure;
using CinemaAdmin.WPF.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CinemaAdmin.WPF.ViewModels
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        //private Employee employee;
        //public Employee Employee { get => employee; set { employee = value; OnPropertyChanged(); } }

        CinemaContext context;
        public RelayCommand Save { get; set; }      
        public void SaveMethod(object obj)
        {
            context.SaveChanges();
        } 
        public MainWindowVM()
        {
            LoginWindow window = new LoginWindow();
            window.ShowDialog();
            var Employee = ((LoginWindowVM)window.DataContext).Employee.Employee;
            if (Employee?.Id == 0)
                Application.Current.MainWindow.Close();

            context = new CinemaContext();
            Save = new RelayCommand(SaveMethod);

            Cities = new ObservableCollection<City>(context.Cities.ToList());
            CurrCity = Cities.FirstOrDefault();
            AddCity = new RelayCommand(AddCityMethod);

            Streets = new ObservableCollection<Street>(context.Streets.ToList());
            CurrStreet = Streets.FirstOrDefault();
            AddStreet = new RelayCommand(AddStreetMethod);

            Cinemas = new ObservableCollection<Cinema.DAL.DomainModels.Cinema>(context.Cinemas.ToList());
            CurrCinema = Cinemas.FirstOrDefault();
            AddCinema = new RelayCommand(AddCinemaMethod);

            Halls = new ObservableCollection<Hall>(context.Halls.ToList());
            CurrHall = Halls.FirstOrDefault();
            AddHall = new RelayCommand(AddHallMethod);

            HallSeats = new ObservableCollection<HallSeat>(context.HallSeats.ToList());
            CurrHallSeat = HallSeats.FirstOrDefault();
            AddHallSeat = new RelayCommand(AddHallSeatMethod);

            Sessions = new ObservableCollection<Session>(context.Sessions.ToList());
            CurrSession = Sessions.FirstOrDefault();
            AddSession = new RelayCommand(AddSessionMethod);

            Countries = new ObservableCollection<Country>(context.Countries.ToList());
            CurrCountry = Countries.FirstOrDefault();
            AddCountry = new RelayCommand(AddCountryMethod);

            Languages = new ObservableCollection<Language>(context.Languages.ToList());
            CurrLanguage = Languages.FirstOrDefault();
            AddLanguage = new RelayCommand(AddLanguageMethod);

            Genres = new ObservableCollection<Genre>(context.Genres.ToList());
            CurrGenre = Genres.FirstOrDefault();
            AddGenre = new RelayCommand(AddGenreMethod);

            Films = new ObservableCollection<Film>(context.Films.ToList());
            CurrFilm = Films.FirstOrDefault();
            AddFilm = new RelayCommand(AddFilmMethod);
            AddPicture = new RelayCommand(AddPictureMethod);

            Positions = new ObservableCollection<Position>(context.Positions.ToList());
            CurrPosition = Positions.FirstOrDefault();
            AddPosition = new RelayCommand(AddPositionMethod);

            Employees = new ObservableCollection<Employee>(context.Employees.ToList());
            CurrEmployee = Employees.FirstOrDefault();
            AddEmployee = new RelayCommand(AddEmployeeMethod);

            Users = new ObservableCollection<User>(context.Users.ToList());
            CurrUser = Users.FirstOrDefault();
        }

        private ObservableCollection<City> cities;
        public ObservableCollection<City> Cities { get => cities; set { cities = value; OnPropertyChanged(); } }
        private City currCity;
        public City CurrCity { get => currCity; set { currCity = value; OnPropertyChanged(); } }
        public RelayCommand AddCity { get; set; }
        public void AddCityMethod(object obj)
        {
            Cities.Add(new City());
            CurrCity = Cities.LastOrDefault();
            context.Cities.Add(CurrCity);
        } 

        private ObservableCollection<Street> streets;
        public ObservableCollection<Street> Streets { get => streets; set { streets = value; OnPropertyChanged(); } }
        private Street currStreet;
        public Street CurrStreet { get => currStreet; set { currStreet = value; OnPropertyChanged(); } }
        public RelayCommand AddStreet { get; set; }
        public void AddStreetMethod(object obj)
        {
            Streets.Add(new Street());
            CurrStreet = Streets.LastOrDefault();
            context.Streets.Add(CurrStreet);
        }

        private ObservableCollection<Cinema.DAL.DomainModels.Cinema> cinemas;
        public ObservableCollection<Cinema.DAL.DomainModels.Cinema> Cinemas { get => cinemas; set { cinemas = value; OnPropertyChanged(); } }
        private Cinema.DAL.DomainModels.Cinema currCinema;
        public Cinema.DAL.DomainModels.Cinema CurrCinema { get => currCinema; set { currCinema = value; OnPropertyChanged(); } }
        public RelayCommand AddCinema { get; set; }
        public void AddCinemaMethod(object obj)
        {
            Cinemas.Add(new Cinema.DAL.DomainModels.Cinema());
            CurrCinema = Cinemas.LastOrDefault();
            context.Cinemas.Add(CurrCinema);
        }

        private ObservableCollection<Hall> halls;
        public ObservableCollection<Hall> Halls { get => halls; set { halls = value; OnPropertyChanged(); } }
        private Hall currHall;
        public Hall CurrHall { get => currHall; set { currHall = value; OnPropertyChanged(); HallSeats = new ObservableCollection<HallSeat>(CurrHall.HallSeats.ToList()); } }
        public RelayCommand AddHall { get; set; }
        public void AddHallMethod(object obj)
        {
            Halls.Add(new Hall());
            CurrHall = Halls.LastOrDefault();
            context.Halls.Add(CurrHall);
        }

        private ObservableCollection<HallSeat> hallSeats;
        public ObservableCollection<HallSeat> HallSeats { get => hallSeats; set { hallSeats = value; OnPropertyChanged(); } }
        private HallSeat currHallSeat;
        public HallSeat CurrHallSeat { get => currHallSeat; set { currHallSeat = value; OnPropertyChanged(); } }
        public RelayCommand AddHallSeat { get; set; }
        public void AddHallSeatMethod(object obj)
        {
            HallSeats.Add(new HallSeat());
            CurrHallSeat = HallSeats.LastOrDefault();
            context.HallSeats.Add(CurrHallSeat);
        }

        private ObservableCollection<Session> sessions;
        public ObservableCollection<Session> Sessions { get => sessions; set { sessions = value; OnPropertyChanged(); } }
        private Session currSession;
        public Session CurrSession { get => currSession; set { currSession = value; OnPropertyChanged(); } }
        public RelayCommand AddSession { get; set; }
        public void AddSessionMethod(object obj)
        {
            Sessions.Add(new Session());
            CurrSession = Sessions.LastOrDefault();
            context.Sessions.Add(CurrSession);
        }

        private ObservableCollection<Country> countries;
        public ObservableCollection<Country> Countries { get => countries; set { countries = value; OnPropertyChanged(); } }
        private Country currCountry;
        public Country CurrCountry { get => currCountry; set { currCountry = value; OnPropertyChanged(); } }
        public RelayCommand AddCountry { get; set; }
        public void AddCountryMethod(object obj)
        {
            Countries.Add(new Country());
            CurrCountry = Countries.LastOrDefault();
            context.Countries.Add(CurrCountry);
        }

        private ObservableCollection<Language> languages;
        public ObservableCollection<Language> Languages { get => languages; set { languages = value; OnPropertyChanged(); } }
        private Language currLanguage;
        public Language CurrLanguage { get => currLanguage; set { currLanguage = value; OnPropertyChanged(); } }
        public RelayCommand AddLanguage { get; set; }
        public void AddLanguageMethod(object obj)
        {
            Languages.Add(new Language());
            CurrLanguage = Languages.LastOrDefault();
            context.Languages.Add(CurrLanguage);
        }

        private ObservableCollection<Genre> genres;
        public ObservableCollection<Genre> Genres { get => genres; set { genres = value; OnPropertyChanged(); } }
        private Genre currGenre;
        public Genre CurrGenre { get => currGenre; set { currGenre = value; OnPropertyChanged(); } }
        public RelayCommand AddGenre { get; set; }
        public void AddGenreMethod(object obj)
        {
            Genres.Add(new Genre());
            CurrGenre = Genres.LastOrDefault();
            context.Genres.Add(CurrGenre);
        }

        private ObservableCollection<Film> films;
        public ObservableCollection<Film> Films { get => films; set { films = value; OnPropertyChanged(); } }
        private Film currFilm;
        public Film CurrFilm { get => currFilm; set { currFilm = value; OnPropertyChanged(); } }
        public RelayCommand AddFilm { get; set; }
        public void AddFilmMethod(object obj)
        {
            Films.Add(new Film() { StartRental = DateTime.Today, EndRental = DateTime.Today });
            CurrFilm = Films.LastOrDefault();
            context.Films.Add(CurrFilm);
        }
        public RelayCommand AddPicture { get; set; }
        public void AddPictureMethod(object obj)
        {
            FilmPosterWindow window = new FilmPosterWindow();
            ((FilmPosterWindowVM)window.DataContext).Picture = CurrFilm.Picture;
            window.ShowDialog();
            if(((FilmPosterWindowVM)window.DataContext).IsSaved)
            {
                CurrFilm.Picture = ((FilmPosterWindowVM)window.DataContext).Picture;
            }
        }

        private ObservableCollection<Position> positions;
        public ObservableCollection<Position> Positions { get => positions; set { positions = value; OnPropertyChanged(); } }
        private Position currPosition;
        public Position CurrPosition { get => currPosition; set { currPosition = value; OnPropertyChanged(); } }
        public RelayCommand AddPosition { get; set; }
        public void AddPositionMethod(object obj)
        {
            Positions.Add(new Position());
            CurrPosition = Positions.LastOrDefault();
            context.Positions.Add(CurrPosition);
        }

        private ObservableCollection<Employee> employees;
        public ObservableCollection<Employee> Employees { get => employees; set { employees = value; OnPropertyChanged(); } }
        private Employee currEmployee;
        public Employee CurrEmployee { get => currEmployee; set { currEmployee = value; OnPropertyChanged(); } }
        public RelayCommand AddEmployee { get; set; }
        public void AddEmployeeMethod(object obj)
        {
            Employees.Add(new Employee() { EmployeesLogin = new EmployeesLogin() });
            CurrEmployee = Employees.LastOrDefault();
            context.Employees.Add(CurrEmployee);
        }

        private ObservableCollection<User> users;
        public ObservableCollection<User> Users { get => users; set { users = value; OnPropertyChanged(); } }
        private User currUser;
        public User CurrUser { get => currUser; set { currUser = value; OnPropertyChanged(); } }
    }
}
