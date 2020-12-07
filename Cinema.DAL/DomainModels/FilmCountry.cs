namespace Cinema.DAL.DomainModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class FilmCountry
    {
        public int Id { get; set; }

        public int FilmId { get; set; }

        public int CountryId { get; set; }

        public virtual Country Country { get; set; }

        public virtual Film Film { get; set; }
    }
}
