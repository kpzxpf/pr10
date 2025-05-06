using Microsoft.EntityFrameworkCore;
using pr10.models;

namespace pr10;

public class ClinicContext : DbContext
{
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<Diagnosis> Diagnoses { get; set; }
    public DbSet<Appointment> Appointments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseNpgsql("Host=localhost;Database=clinic;Username=user;Password=password");

    protected override void OnModelCreating(ModelBuilder model)
    {
        model.Entity<Appointment>()
            .HasOne(a => a.Patient).WithMany(p => p.Appointments)
            .HasForeignKey(a => a.PatientId);
        model.Entity<Appointment>()
            .HasOne(a => a.Doctor).WithMany(d => d.Appointments)
            .HasForeignKey(a => a.DoctorId);
        model.Entity<Appointment>()
            .HasOne(a => a.Diagnosis).WithMany(d => d.Appointments)
            .HasForeignKey(a => a.DiagnosisId);
        model.Entity<Appointment>()
            .HasOne(a => a.Service).WithMany(s => s.Appointments)
            .HasForeignKey(a => a.ServiceId);
    }
}