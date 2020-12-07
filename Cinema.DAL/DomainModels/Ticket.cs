namespace Cinema.DAL.DomainModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ticket
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int SessionId { get; set; }

        public int? EmployeeId { get; set; }

        public int? UserId { get; set; }

        public int Row { get; set; }

        public int Place { get; set; }

        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        public bool IsReturned { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual Session Session { get; set; }

        public virtual User User { get; set; }
    }
}
