using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using test.Models;

namespace test.Data
{
    public class TestDb: DbContext
    {
        public TestDb(DbContextOptions<TestDb> options) : base(options) {}
        public DbSet<User> Users { get; set; }
        public DbSet<Country> Countries { get; set; } 
        public DbSet<City> Cities { get; set; }
        public DbSet<Population> Populations { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<Country>().HasKey(c => c.CountryId);
            modelBuilder.Entity<Country>().HasIndex(c => c.CountryName).IsUnique();
            modelBuilder.Entity<City>().HasKey(c => c.CityId);
            modelBuilder.Entity<City>()
               .HasOne(c => c.Country)  
               .WithMany(c => c.Cities)     
               .HasForeignKey(c => c.CountryId);
            modelBuilder.Entity<City>()
                .HasIndex(c => new { c.CountryId, c.CityName })
                .IsUnique();
            modelBuilder.Entity<Population>().HasKey(p => p.PopulationId);
            modelBuilder.Entity<Population>()
               .HasOne(p => p.City)  
               .WithMany(p => p.Populations)     
               .HasForeignKey(p => p.CityId);
        }
    }
}
