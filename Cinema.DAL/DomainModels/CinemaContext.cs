using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Cinema.DAL.DomainModels
{
    public partial class CinemaContext : DbContext
    {
        public CinemaContext()
            : base("name=CinemaContext")
        {
        }

        public virtual DbSet<Cinema> Cinemas { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<EmployeesLogin> EmployeesLogins { get; set; }
        public virtual DbSet<FilmCountry> FilmCountries { get; set; }
        public virtual DbSet<FilmGenre> FilmGenres { get; set; }
        public virtual DbSet<Film> Films { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Hall> Halls { get; set; }
        public virtual DbSet<HallSeat> HallSeats { get; set; }
        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<Session> Sessions { get; set; }
        public virtual DbSet<Street> Streets { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cinema>()
                .HasMany(e => e.Employees)
                .WithRequired(e => e.Cinema)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Cinema>()
                .HasMany(e => e.Halls)
                .WithRequired(e => e.Cinema)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<City>()
                .HasMany(e => e.Cinemas)
                .WithRequired(e => e.City)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Country>()
                .HasMany(e => e.FilmCountries)
                .WithRequired(e => e.Country)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Employees1)
                .WithOptional(e => e.Employee1)
                .HasForeignKey(e => e.ManagerId);

            modelBuilder.Entity<Employee>()
                .HasOptional(e => e.EmployeesLogin)
                .WithRequired(e => e.Employee);

            modelBuilder.Entity<Film>()
                .HasMany(e => e.FilmCountries)
                .WithRequired(e => e.Film)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Film>()
                .HasMany(e => e.FilmGenres)
                .WithRequired(e => e.Film)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Film>()
                .HasMany(e => e.Sessions)
                .WithRequired(e => e.Film)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Genre>()
                .HasMany(e => e.FilmGenres)
                .WithRequired(e => e.Genre)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Hall>()
                .HasMany(e => e.HallSeats)
                .WithRequired(e => e.Hall)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Hall>()
                .HasMany(e => e.Sessions)
                .WithRequired(e => e.Hall)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Language>()
                .HasMany(e => e.Films)
                .WithRequired(e => e.Language)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Position>()
                .HasMany(e => e.Employees)
                .WithRequired(e => e.Position)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Session>()
                .Property(e => e.Price)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Session>()
                .Property(e => e.AddLux)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Session>()
                .HasMany(e => e.Tickets)
                .WithRequired(e => e.Session)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Street>()
                .HasMany(e => e.Cinemas)
                .WithRequired(e => e.Street)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Ticket>()
                .Property(e => e.Price)
                .HasPrecision(19, 4);
        }
    }
}
