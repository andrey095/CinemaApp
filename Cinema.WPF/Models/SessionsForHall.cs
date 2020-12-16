using Cinema.DAL.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.WPF.Models
{
    public class SessionsForHall
    {
        public SessionsForHall()
        {
            Sessions = new List<Session>();
        }

        public Hall Hall { get; set; }
        public IEnumerable<Session> Sessions { get; set; }
    }
}
