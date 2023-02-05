using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyNotesApp.Data.Mappings;
using MyNotesApp.Models;
using System.Reflection.Emit;

namespace MyNotesApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<HoppyUser>()
        .HasKey(sc => new { sc.AppUserId, sc.HoppyId });

            builder.Entity<HoppyUser>()
                .HasOne(sc => sc.AppUser)
                .WithMany(s => s.HoppyUser)
                .HasForeignKey(sc => sc.AppUserId);

            builder.Entity<HoppyUser>()
                .HasOne(sc => sc.Hoppy)
                .WithMany(c => c.HoppyUser)
                .HasForeignKey(sc => sc.HoppyId);

            builder.Entity<StudentHoppy>()
        .HasKey(sc => new { sc.StudentId, sc.HoppyId });

            builder.Entity<StudentHoppy>()
                .HasOne(sc => sc.Student)
                .WithMany(s => s.studentHoppies)
                .HasForeignKey(sc => sc.StudentId);

            builder.Entity<StudentHoppy>()
                .HasOne(sc => sc.Hoppy)
                .WithMany(c => c.studentHoppies)
                .HasForeignKey(sc => sc.HoppyId);

            builder.ApplyConfiguration(new GenderMap());
        }
        public DbSet<AppUser>? AppUsers { get; set; }
        public DbSet<Note>? Notes { get; set; }
        public DbSet<Gender>? Genders { get; set; }
        public DbSet<Hoppy>? Hoppies { get; set; }
        public DbSet<Student>? Students { get; set; }
        public DbSet<HoppyUser>? HoppyUsers { get; set; }
        public DbSet<StudentHoppy>? StudentHoppies { get; set; }
    }
}
