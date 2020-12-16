using Cinema.DAL.DomainModels;
using Cinema.DAL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Cinema.WPF.ViewModels
{
    public class LoginWindowVM
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Pass { get; set; }
        public bool LoggedIn { get; set; }
        public bool IsEmployee { get; set; }

        public CinemaContext Context { get; set; }

        public LoginWindowVM()
        {
            Context = new CinemaContext();
            SignIn = new RelayCommand(SignInMethod);
            Registration = new RelayCommand(RegistrationMethod, t => !IsEmployee);
        }
        public RelayCommand SignIn { get; set; }
        private void SignInMethod(object obj)
        {
            LoggedIn = false;  
            object temp;
            if (IsEmployee)
                temp = Context.EmployeesLogins.FirstOrDefault(u => u.EmpName == Login && u.Password == Pass);
            else
                temp = Context.Users.FirstOrDefault(u => u.Email == Login && u.Password == Pass);
            if (temp != null)
            {
                LoggedIn = true;
                if (IsEmployee)
                    Id = ((EmployeesLogin)temp).Id;
                else
                    Id = ((User)temp).Id;
                MessageBox.Show("You entered");
                Application.Current.Windows[1].Close();
            }
            else
                MessageBox.Show("Wrong data");
        }
        public RelayCommand Registration { get; set; }
        private void RegistrationMethod(object obj)
        {
            if (Context.Users.FirstOrDefault(u => u.Email == Login) == null)
            {
                Context.Users.Add(new User() { Email = Login, Password = Pass });
                Context.SaveChanges();
                MessageBox.Show("User added");
            }
            else
                MessageBox.Show("User exists");
        }
    }
}
