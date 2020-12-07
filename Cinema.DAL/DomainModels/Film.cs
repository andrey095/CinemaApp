namespace Cinema.DAL.DomainModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Film
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Film()
        {
            FilmCountries = new HashSet<FilmCountry>();
            FilmGenres = new HashSet<FilmGenre>();
            Sessions = new HashSet<Session>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string NameOrigin { get; set; }

        [Required]
        [StringLength(400)]
        public string Description { get; set; }

        public int Age { get; set; }

        [Required]
        public byte[] Picture { get; set; }

        public short Duration { get; set; }

        public int LanguageId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? StartRental { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EndRental { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FilmCountry> FilmCountries { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FilmGenre> FilmGenres { get; set; }

        public virtual Language Language { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Session> Sessions { get; set; }
    }
}
