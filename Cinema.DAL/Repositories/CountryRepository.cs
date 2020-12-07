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
    public class CountryRepository : GenericRepository<Country, int>
    {
        public CountryRepository(DbContext context) : base(context)
        {
        }
    }
}
