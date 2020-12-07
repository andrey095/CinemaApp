namespace Cinema.DAL.DomainModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class FilmGenre
    {
        public int Id { get; set; }

        public int FilmId { get; set; }

        public int GenreId { get; set; }

        public virtual Film Film { get; set; }

        public virtual Genre Genre { get; set; }
    }
}
