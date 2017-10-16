using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using RSData.Models;

namespace RSRepository
{
    public partial class RoomPlannerDevContext : DbContext
    {
        public RoomPlannerDevContext(DbContextOptions<RoomPlannerDevContext> options)
    : base(options) {}

        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<Event> Event { get; set; }
        public virtual DbSet<Penalty> Penalty { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Room> Room { get; set; }
        public virtual DbSet<TimeSlot> TimeSlot { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }

       /* protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(@"Server=BUSWGVMINDEV3\MSSQLSERVER12;Database=RoomPlannerDev;User Id=roomplanner;Password=roomplanner123");
            }
        } */

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("DepartmentID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("EventID");

                entity.Property(e => e.AttendeeId).HasColumnName("AttendeeID");

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.EventType)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.HostId).HasColumnName("HostID");

                entity.Property(e => e.Notes)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.RoomId).HasColumnName("RoomID");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.Attendee)
                    .WithMany(p => p.EventAttendee)
                    .HasForeignKey(d => d.AttendeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Event_Attendee");

                entity.HasOne(d => d.Host)
                    .WithMany(p => p.EventHost)
                    .HasForeignKey(d => d.HostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Event_Host");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Event)
                    .HasForeignKey(d => d.RoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Event_Room");
            });

            modelBuilder.Entity<Penalty>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("PenaltyID");

                entity.Property(e => e.AttendeeId).HasColumnName("AttendeeID");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.EventId).HasColumnName("EventID");

                entity.HasOne(d => d.Attendee)
                    .WithMany(p => p.Penalty)
                    .HasForeignKey(d => d.AttendeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Penalty_User");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.Penalty)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Penalty_Event");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("RoleID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("RoomID");

                entity.Property(e => e.Location).HasMaxLength(150);

                entity.Property(e => e.Name).HasMaxLength(150);
            });

            modelBuilder.Entity<TimeSlot>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("TimeSlotID");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.HostId).HasColumnName("HostID");

                entity.Property(e => e.RoomId).HasColumnName("RoomID");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.Host)
                    .WithMany(p => p.TimeSlot)
                    .HasForeignKey(d => d.HostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TimeSlot_User");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.TimeSlot)
                    .HasForeignKey(d => d.RoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TimeSlot_Room");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("UserID");

                entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.DepartmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Department");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                //entity.HasKey(e => new { e.UserId, e.RoleId });
                entity.Property(e => e.Id).HasColumnName("UserRoleID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserRole)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserRole_Role");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRole)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserRole_User");
            });
        }
    }
}
