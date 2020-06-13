using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RedCrossBackend.DBModel
{
    public partial class DB_RedCrossContext : DbContext
    {
        public DB_RedCrossContext()
        {
        }

        public DB_RedCrossContext(DbContextOptions<DB_RedCrossContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Assistance> Assistance { get; set; }
        public virtual DbSet<Faassistance> Faassistance { get; set; }
        public virtual DbSet<Fainjury> Fainjury { get; set; }
        public virtual DbSet<FaphType> FaphType { get; set; }
        public virtual DbSet<FirstAid> FirstAid { get; set; }
        public virtual DbSet<Injury> Injury { get; set; }
        public virtual DbSet<PhType> PhType { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            // To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=serv-redcross.database.windows.net;Database=DB_RedCross;User Id=sqladmin;Password=Nexios2020;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Assistance>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Faassistance>(entity =>
            {
                entity.ToTable("FAAssistance");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Aid).HasColumnName("AId");

                entity.Property(e => e.Faid).HasColumnName("FAId");
            });

            modelBuilder.Entity<Fainjury>(entity =>
            {
                entity.ToTable("FAInjury");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Faid).HasColumnName("FAId");

                entity.Property(e => e.Iid).HasColumnName("IId");
            });

            modelBuilder.Entity<FaphType>(entity =>
            {
                entity.ToTable("FAPhType");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Faid).HasColumnName("FAId");

                entity.Property(e => e.Ptid).HasColumnName("PTId");
            });

            modelBuilder.Entity<FirstAid>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Age)
                    .HasColumnName("age")
                    .HasMaxLength(50);

                entity.Property(e => e.AssignDate)
                    .HasColumnName("assignDate")
                    .HasColumnType("date");

                entity.Property(e => e.BlendedTraining).HasColumnName("blendedTraining");

                entity.Property(e => e.ConfidentApplyingFa).HasColumnName("confidentApplyingFA");

                entity.Property(e => e.Education)
                    .HasColumnName("education")
                    .HasMaxLength(50);

                entity.Property(e => e.Gender)
                    .HasColumnName("gender")
                    .HasMaxLength(50);

                entity.Property(e => e.HadFatraining).HasColumnName("hadFATraining");

                entity.Property(e => e.HospitalisationRequired).HasColumnName("hospitalisationRequired");

                entity.Property(e => e.Latitude)
                    .HasColumnName("latitude")
                    .HasMaxLength(50);

                entity.Property(e => e.Longitude)
                    .HasColumnName("longitude")
                    .HasMaxLength(50);

                entity.Property(e => e.MacAddress)
                    .HasColumnName("macAddress")
                    .HasMaxLength(50);

                entity.Property(e => e.NumberOffAttraining).HasColumnName("numberOffATtraining");

                entity.Property(e => e.OtjerTrainingProvider)
                    .HasColumnName("otjerTrainingProvider")
                    .HasMaxLength(50);

                entity.Property(e => e.PhNeeded).HasColumnName("phNeeded");

                entity.Property(e => e.PhTimeToArriveMs).HasColumnName("phTimeToArriveMs");

                entity.Property(e => e.Setting)
                    .HasColumnName("setting")
                    .HasMaxLength(50);

                entity.Property(e => e.TrainingByRc).HasColumnName("trainingByRC");
            });

            modelBuilder.Entity<Injury>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<PhType>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
