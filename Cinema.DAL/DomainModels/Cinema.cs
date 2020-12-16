namespace Cinema.DAL.DomainModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Cinema
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cinema()
        {
            Employees = new HashSet<Employee>();
            Halls = new HashSet<Hall>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public int CityId { get; set; }

        public int StreetId { get; set; }

        [Required]
        [StringLength(10)]
        public string Bilding { get; set; }

        [StringLength(20)]
        public string Tel { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        public virtual City City { get; set; }

        public virtual Street Street { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Employee> Employees { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Hall> Halls { get; set; }
    }
}
