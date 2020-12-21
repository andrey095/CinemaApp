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

namespace CinemaAdmin.WPF.ViewModels
{
    public class LoginWindowVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public EmployeesLogin Employee { get; set; }
        public string Login { get; set; }
        public string Pass { get; set; }
        CinemaContext context;
        public LoginWindowVM()
        {
            context = new CinemaContext();
            SignIn = new RelayCommand(SignInMethod);
        }
        public RelayCommand SignIn { get; set; }
        private void SignInMethod(object obj)
        {
            var temp = context.EmployeesLogins.Where(e => e.EmpName == Login && e.Password == Pass).FirstOrDefault();
            if (temp != null && (temp.Employee.PositionId == 1 || temp.Employee.PositionId == 2))
            {
                Employee = temp;
                MessageBox.Show("Вы авторизованы");
            }
            else
            {
                Employee = null;
                MessageBox.Show("Неправильные данные");
            }            
        }
    }
}
