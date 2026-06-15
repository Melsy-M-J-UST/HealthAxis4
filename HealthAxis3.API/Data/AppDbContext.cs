using HealthAxis3.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace HealthAxis3.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<HealthRecord> HealthRecords { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<HealthRecord>()
            .HasOne(hr => hr.Patient)
            .WithMany()
            .HasForeignKey(hr => hr.PatientId)
            .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<HealthRecord>()
            .HasOne(hr => hr.Doctor)
            .WithMany()
            .HasForeignKey(hr => hr.DoctorId)
            .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Patient>().HasData(
                new Patient { PatientId = 1, PatientName = "Arun Kumar", DateOfBirth = new DateTime(1992, 5, 14, 12, 24, 33), Gender = "Male", PhoneNumber = "9876543210", Email = "arun.kumar@example.com", InsuranceId = "INS1001", RegisteredDate = new DateTime(2026, 6, 15), IsActive = true },
                new Patient { PatientId = 2, PatientName = "Meera Nair", DateOfBirth = new DateTime(1988, 9, 22, 22, 15, 30), Gender = "Female", PhoneNumber = "9876543211", Email = "meera.nair@example.com", InsuranceId = "INS1002", RegisteredDate = new DateTime(2026, 6, 15), IsActive = true },
                new Patient { PatientId = 3, PatientName = "Rahul Menon", DateOfBirth = new DateTime(2000, 1, 10, 16, 17, 18), Gender = "Male", PhoneNumber = "9876543212", Email = "rahul.menon@example.com", InsuranceId = "INS1003", RegisteredDate = new DateTime(2026, 6, 15), IsActive = true },
                new Patient { PatientId = 4, PatientName = "Anjali Thomas", DateOfBirth = new DateTime(1995, 12, 3, 1, 2, 3), Gender = "Female", PhoneNumber = "9876543213", Email = "anjali.thomas@example.com", InsuranceId = "INS1004", RegisteredDate = new DateTime(2026, 6, 15), IsActive = true },
                new Patient { PatientId = 5, PatientName = "Vivek Pillai", DateOfBirth = new DateTime(1983, 7, 19, 5, 6, 7), Gender = "Male", PhoneNumber = "9876543214", Email = "vivek.pillai@example.com", InsuranceId = "INS1005", RegisteredDate = new DateTime(2026, 6, 15), IsActive = true });

            modelBuilder.Entity<Doctor>().HasData(
                new Doctor { DoctorId = 1, DoctorName = "Dr. Priya Sharma", Specialisation = "Cardiologist", Experience = 12, Fees = 800, IsActive = true },
                new Doctor { DoctorId = 2, DoctorName = "Dr. Suresh Mathew", Specialisation = "Dermatologist", Experience = 9, Fees = 600, IsActive = true },
                new Doctor { DoctorId = 3, DoctorName = "Dr. Neha Iyer", Specialisation = "Pediatrician", Experience = 10, Fees = 700, IsActive = true },
                new Doctor { DoctorId = 4, DoctorName = "Dr. Thomas George", Specialisation = "OrthopedicSurgeon", Experience = 15, Fees = 900, IsActive = true },
                new Doctor { DoctorId = 5, DoctorName = "Dr. Kavitha Rao", Specialisation = "Neurologist", Experience = 14, Fees = 1000, IsActive = true },
                new Doctor { DoctorId = 6, DoctorName = "Dr. Mohammed Ali", Specialisation = "GeneralPractitioner", Experience = 11,Fees = 500,  IsActive = true },
                new Doctor { DoctorId = 7, DoctorName = "Dr. Lakshmi Menon", Specialisation = "Endocrinologist", Experience = 8, Fees = 550, IsActive = true },
                new Doctor { DoctorId = 8, DoctorName = "Dr. Rajesh Nambiar", Specialisation = "Oncologist", Experience = 13, Fees = 650, IsActive = true } );
        }
    }
}
