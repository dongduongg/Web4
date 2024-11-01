using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace QLBN.Models
{
    public partial class QLBNContext : DbContext
    {
        public QLBNContext()
        {
        }

        public QLBNContext(DbContextOptions<QLBNContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Appointment> Appointments { get; set; } = null!;
        public virtual DbSet<Doctor> Doctors { get; set; } = null!;
        public virtual DbSet<Faculty> Faculties { get; set; } = null!;
        public virtual DbSet<Patient> Patients { get; set; } = null!;
        public virtual DbSet<Record> Records { get; set; } = null!;
        public virtual DbSet<Room> Rooms { get; set; } = null!;
        public virtual DbSet<Service> Services { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning 
                optionsBuilder.UseSqlServer("Data Source=LAMTHUCNGHI\\MAYCHU;Initial Catalog=QLBN;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.ToTable("Appointment");

                entity.Property(e => e.AppointmentId)
                    .ValueGeneratedNever()
                    .HasColumnName("AppointmentID");

                entity.Property(e => e.AppointmentStatus)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.DoctorId).HasColumnName("DoctorID");

                entity.Property(e => e.PatientId)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("PatientID")
                    .IsFixedLength();

                entity.Property(e => e.ServiceId).HasColumnName("ServiceID");

                entity.HasOne(d => d.Doctor)
                    .WithMany(p => p.Appointments)
                    .HasForeignKey(d => d.DoctorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Appointment_Doctor");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.Appointments)
                    .HasForeignKey(d => d.PatientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Appointment_Patient");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.Appointments)
                    .HasForeignKey(d => d.ServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Appointment_Service");
            });

            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.ToTable("Doctor");

                entity.Property(e => e.DoctorId)
                    .ValueGeneratedNever()
                    .HasColumnName("DoctorID");

                entity.Property(e => e.DoctorDegree).HasMaxLength(50);

                entity.Property(e => e.DoctorName).HasMaxLength(50);

                entity.Property(e => e.FacultyId).HasColumnName("FacultyID");

                entity.Property(e => e.RoomId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("RoomID")
                    .IsFixedLength();

                entity.Property(e => e.ServiceId).HasColumnName("ServiceID");

                entity.HasOne(d => d.Faculty)
                    .WithMany(p => p.Doctors)
                    .HasForeignKey(d => d.FacultyId)
                    .HasConstraintName("FK_Doctor_Faculty");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Doctors)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("FK_Doctor_Room");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.Doctors)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("FK_Doctor_Service");
            });

            modelBuilder.Entity<Faculty>(entity =>
            {
                entity.ToTable("Faculty");

                entity.Property(e => e.FacultyId)
                    .ValueGeneratedNever()
                    .HasColumnName("FacultyID");

                entity.Property(e => e.FacultyName).HasMaxLength(50);
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.ToTable("Patient");

                entity.Property(e => e.PatientId)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("PatientID")
                    .IsFixedLength();

                entity.Property(e => e.PatientEmail)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.PatientGender).HasMaxLength(10);

                entity.Property(e => e.PatientName).HasMaxLength(50);

                entity.Property(e => e.PatientPhone)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.UsernameNavigation)
                    .WithMany(p => p.Patients)
                    .HasForeignKey(d => d.Username)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Patient_User");
            });

            modelBuilder.Entity<Record>(entity =>
            {
                entity.ToTable("Record");

                entity.Property(e => e.RecordId)
                    .ValueGeneratedNever()
                    .HasColumnName("RecordID");

                entity.Property(e => e.AppointmentId).HasColumnName("AppointmentID");

                entity.HasOne(d => d.Appointment)
                    .WithMany(p => p.Records)
                    .HasForeignKey(d => d.AppointmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Record_Appointment");
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.ToTable("Room");

                entity.Property(e => e.RoomId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("RoomID")
                    .IsFixedLength();

                entity.Property(e => e.FacultyId).HasColumnName("FacultyID");

                entity.Property(e => e.RoomBuilding)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.RoomFloor)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.Faculty)
                    .WithMany(p => p.Rooms)
                    .HasForeignKey(d => d.FacultyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Room_Faculty");
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.ToTable("Service");

                entity.Property(e => e.ServiceId)
                    .ValueGeneratedNever()
                    .HasColumnName("ServiceID");

                entity.Property(e => e.ServiceName).HasMaxLength(50);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Username)
                    .HasName("PK__User__536C85E5E2B545B4");

                entity.ToTable("User");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
