using Cinema.DAL.DomainModels;
using Cinema.DAL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Cinema.WPF.ViewModels
{
    public class UserWindowVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        CinemaContext context;
        private User user;
        public User User 
        { 
            get => user; 
            set 
            { 
                user = value; 
                Tickets = new ObservableCollection<Ticket>(User.Tickets.Where(t => t.IsReturned == false).ToList()); 
                OnPropertyChanged(); 
            } 
        }

        private ObservableCollection<Ticket> tickets;
        public ObservableCollection<Ticket> Tickets 
        { 
            get => tickets; 
            set 
            { 
                tickets = value; 
                OnPropertyChanged(); 
            } 
        }

        public UserWindowVM()
        {
            context = new CinemaContext();
            ChangePass = new RelayCommand(ChangePassMethod);
        }
        public RelayCommand ChangePass { get; set; }
        private void ChangePassMethod(object obj)
        {
            if(MessageBox.Show("Вы уверенны?", "Смена пароля", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                context.Users.AddOrUpdate(User);
                context.SaveChanges();
            }
        }
    }
}
