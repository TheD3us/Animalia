using Administration.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Administration.Data
{
    public class AdministrationDbContext : DbContext
    {
        public AdministrationDbContext(DbContextOptions<AdministrationDbContext> options) : base(options)
        {
        }

        public DbSet<Event> Events { get; set; } = null!;
        public DbSet<ProgramEntity> Programs { get; set; } = null!;
        public DbSet<Training> Trainings { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Testimonial> Testimonials { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Table Events
            modelBuilder.Entity<Event>()
                .ToTable("Events");

            // Table ProgramModels
            modelBuilder.Entity<ProgramEntity>()
                .ToTable("ProgramModels");

            // Table Trainings
            modelBuilder.Entity<Training>()
                .ToTable("Trainings");

            // Table Users
            modelBuilder.Entity<User>()
                .ToTable("Users");

            // Table Testimonials
            modelBuilder.Entity<Testimonial>()
                .ToTable("Testimonials");

            // Configuration de la relation Many-to-Many entre Programs et Trainings
            modelBuilder.Entity<ProgramEntity>()
                .HasMany(p => p.Trainings)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "ProgramTrainings",
                    j => j
                        .HasOne<Training>()
                        .WithMany()
                        .HasForeignKey("TrainingId")
                        .OnDelete(DeleteBehavior.Restrict),
                    j => j
                        .HasOne<ProgramEntity>()
                        .WithMany()
                        .HasForeignKey("ProgramModelId")
                        .OnDelete(DeleteBehavior.Restrict),
                    j =>
                    {
                        j.HasKey("ProgramModelId", "TrainingId");
                        j.ToTable("ProgramTrainings");
                    });

            // Relation Events - Users
            modelBuilder.Entity<Event>()
                .HasOne(e => e.User)
                .WithMany(u => u.Events)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Proprietes decimales avec precision
            modelBuilder.Entity<ProgramEntity>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            // Valeurs par defaut
            modelBuilder.Entity<Testimonial>()
                .Property(t => t.CreatedDate)
                .HasDefaultValueSql("GETDATE()");

            // Index pour les performances
            modelBuilder.Entity<Event>()
                .HasIndex(e => e.DateTime)
                .HasDatabaseName("IX_Events_DateTime");

            modelBuilder.Entity<Event>()
                .HasIndex(e => e.UserId)
                .HasDatabaseName("IX_Events_UserId");

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique()
                .HasDatabaseName("IX_Users_Email");

            modelBuilder.Entity<ProgramEntity>()
                .HasIndex(p => p.Title)
                .HasDatabaseName("IX_ProgramModels_Title");

            // Index corrigé pour Training - utilise la vraie colonne "Level"
            modelBuilder.Entity<Training>()
                .HasIndex(t => t.Level)
                .HasDatabaseName("IX_Trainings_Level");

            // Relation Training - User (créateur)
            modelBuilder.Entity<Training>()
                .HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}