using HospitalSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

         
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=HospitalDb;Trusted_Connection=True;TrustServerCertificate=True;");
        }

         
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Doctor>().HasData(
                new Doctor { Id = 1, Name = "Dr. Kerolos  ", Specialization = "Cardiology", ImageUrl = "1.jpg" },
                new Doctor { Id = 2, Name = "Dr. Sarah ", Specialization = "Dermatology", ImageUrl = "2.jpg" },
                new Doctor { Id = 3, Name = "Dr. Omar  ", Specialization = "Orthopedics", ImageUrl = "3.jpg" },
                new Doctor { Id = 4, Name = "Dr. Mona  ", Specialization = "Pediatrics", ImageUrl = "4.jpg" },
                new Doctor { Id = 5, Name = "Dr. Hassan  ", Specialization = "Cardiology", ImageUrl = "5.jpg" },
                new Doctor { Id = 6, Name = "Dr. Nour ", Specialization = "Neurology", ImageUrl = "6.jpg" }
            );
        }
    }
}