using EntityFrameworkCore.EncryptColumn.Interfaces;
using EntityFrameworkCore.EncryptColumn.Util;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RAWH.DAL.Models;
using System.Reflection.Emit;
namespace RAWH.DAL.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>

    {
        private readonly IEncryptionProvider _encryptionProvider;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            _encryptionProvider = new GenerateEncryptionProvider("63fdf5d627c30b56");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany(u => u.DoctorAppointments)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Appointment>()
                .HasOne(a => a.User)
                .WithOne(u => u.MotherAppointment)
                .HasForeignKey<Appointment>(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Appointment>()
                .HasIndex(a => a.UserId)
                .IsUnique();
            builder.Entity<MedicalReport>()
        .HasOne(m => m.Doctor)
        .WithMany(u => u.DoctorMedicalReports)
        .HasForeignKey(m => m.DoctorId)
        .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<MedicalReport>()
                .HasOne(m => m.Child)
                .WithMany(u => u.ChildMedicalReports)
                .HasForeignKey(m => m.ChildId)
                .OnDelete(DeleteBehavior.Restrict);
        }
        public DbSet<PneumoniaSurveyRequest> PneumoniaSurveyRequest { get; set; }
        public DbSet<Hospital> Hospitals { get; set; }

        public DbSet<Clinic> Clinics { get; set; }

        public DbSet<Appointment> Appointments { get; set; }

        public DbSet<MedicalReport> MedicalReports { get; set; }

        public DbSet<Notification> Notifications { get; set; }
        
    }
}
