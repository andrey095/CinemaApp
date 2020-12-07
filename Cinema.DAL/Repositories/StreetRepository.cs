using Cinema.DAL.Commons;
using Cinema.DAL.DomainModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.DAL.Repositories
{
    public class StreetRepository : GenericRepository<Street, int>
    {
        public StreetRepository(DbContext context) : base(context)
        {
        }
    }
}
