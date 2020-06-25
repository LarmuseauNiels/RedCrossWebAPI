using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RedCrossBackend.Model
{
    public partial class DB_RedCrossContext : DbContext
    {
        public DB_RedCrossContext()
        {

        }
        public DB_RedCrossContext(DbContextOptions<DB_RedCrossContext> options)
            : base(options)
        {
            this.Database.SetCommandTimeout(new TimeSpan(0, 15, 30));
        }


        public virtual DbSet<Assistance> Assistance { get; set; }
        public virtual DbSet<Faassistance> FAAssistance { get; set; }
        public virtual DbSet<Fainjury> FAInjury { get; set; }
        public virtual DbSet<FaphType> FaphType { get; set; }
        public virtual DbSet<FirstAid> FirstAid { get; set; }
        public virtual DbSet<Injury> Injury { get; set; }
        public virtual DbSet<PhType> PhType { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseSqlServer("Server=serv-redcross.database.windows.net;Database=DB_RedCross;User Id=sqladmin;Password=Nexios2020;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Assistance>(entity =>
            {
                entity.Property(e => e.id).HasColumnName("id");

                entity.Property(e => e.name)
                    .HasColumnName("name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Faassistance>(entity =>
            {
                entity.ToTable("FAAssistance");

                entity.Property(e => e.id).HasColumnName("id");

                entity.Property(e => e.AId).HasColumnName("AId");

                entity.Property(e => e.FAId).HasColumnName("FAId");
            });

            modelBuilder.Entity<Fainjury>(entity =>
            {
                entity.ToTable("FAInjury");

                entity.Property(e => e.id).HasColumnName("id");

                entity.Property(e => e.FAId).HasColumnName("FAId");

                entity.Property(e => e.IId).HasColumnName("IId");
            });

            modelBuilder.Entity<FaphType>(entity =>
            {
                entity.ToTable("FAPhType");

                entity.Property(e => e.id).HasColumnName("id");

                entity.Property(e => e.FAId).HasColumnName("FAId");

                entity.Property(e => e.PTId).HasColumnName("PTId");
            });

            modelBuilder.Entity<FirstAid>(entity =>
            {
                entity.Property(e => e.id).HasColumnName("id");

                entity.Property(e => e.age)
                    .HasColumnName("age")
                    .HasMaxLength(50);

                entity.Property(e => e.assignDate)
                    .HasColumnName("assignDate")
                    .HasColumnType("date");

                entity.Property(e => e.blendedTraining).HasColumnName("blendedTraining");

                entity.Property(e => e.confidentApplyingFA).HasColumnName("confidentApplyingFA");

                entity.Property(e => e.education)
                    .HasColumnName("education")
                    .HasMaxLength(50);

                entity.Property(e => e.gender)
                    .HasColumnName("gender")
                    .HasMaxLength(50);

                entity.Property(e => e.hadFATraining).HasColumnName("hadFATraining");

                entity.Property(e => e.hospitalisationRequired).HasColumnName("hospitalisationRequired");

                entity.Property(e => e.latitude)
                    .HasColumnName("latitude");

                entity.Property(e => e.longitude)
                    .HasColumnName("longitude");

                entity.Property(e => e.macAddress)
                    .HasColumnName("macAddress")
                    .HasMaxLength(50);

                entity.Property(e => e.numberOffFATtraining).HasColumnName("numberOffFATtraining");

                entity.Property(e => e.otherTrainingProvider)
                    .HasColumnName("otherTrainingProvider")
                    .HasMaxLength(50);

                entity.Property(e => e.phNeeded).HasColumnName("phNeeded").HasMaxLength(50);

                entity.Property(e => e.phTimeToArrive).HasColumnName("phTimeToArrive")
                    .HasMaxLength(50);

                entity.Property(e => e.setting)
                    .HasColumnName("setting")
                    .HasMaxLength(50);

                entity.Property(e => e.trainingByRC).HasColumnName("trainingByRC");
            });

            modelBuilder.Entity<Injury>(entity =>
            {
                entity.Property(e => e.id).HasColumnName("id");

                entity.Property(e => e.name)
                    .HasColumnName("name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<PhType>(entity =>
            {
                entity.Property(e => e.id).HasColumnName("id");

                entity.Property(e => e.name)
                    .HasColumnName("name")
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
