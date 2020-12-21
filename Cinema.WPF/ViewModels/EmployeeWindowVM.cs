using Cinema.DAL.DomainModels;
using Cinema.DAL.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Cinema.WPF.ViewModels
{
    public class EmployeeWindowVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        CinemaContext context;

        private Employee employee;
        public Employee Employee { get => employee; set { employee = value; OnPropertyChanged(); } }
        private Ticket ticket;

        public Ticket Ticket { get => ticket; set { ticket = value; OnPropertyChanged(); } }

        private string id;
        public string Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged();
                int tId = 0;
                if (int.TryParse(Id, out tId))
                {
                    Ticket = context.Tickets.FirstOrDefault(t => t.Id == tId && t.IsReturned == false);
                }
                else
                    Ticket = null;
            }
        }

        public EmployeeWindowVM()
        {
            context = new CinemaContext();
            ReturnTicket = new RelayCommand(ReturnTicketMethod, obj => Ticket != null);
        }
        public RelayCommand ReturnTicket { get; set; }
        private void ReturnTicketMethod(object obj)
        {
            Ticket.IsReturned = true;
            context.SaveChanges();
            Ticket = null;
        }
    }
}
