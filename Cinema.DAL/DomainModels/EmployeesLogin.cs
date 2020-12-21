namespace Cinema.DAL.DomainModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EmployeesLogin")]
    public partial class EmployeesLogin
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string EmpName { get; set; }

        [Required]
        [StringLength(20)]
        public string Password { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
