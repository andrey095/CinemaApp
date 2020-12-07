namespace Cinema.DAL.DomainModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class HallSeat
    {
        public int Id { get; set; }

        public int HallId { get; set; }

        public int Row { get; set; }

        public int Places { get; set; }

        public bool PlaceType { get; set; }

        public virtual Hall Hall { get; set; }
    }
}
