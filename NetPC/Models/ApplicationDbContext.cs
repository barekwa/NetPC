using Microsoft.EntityFrameworkCore;
using NetPC.Models;

namespace NetPC.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Służbowy" },
                new Category { Id = 2, Name = "Prywatny" },
                new Category { Id = 3, Name = "Inny" }
            );

            modelBuilder.Entity<Subcategory>().HasData(
                new Subcategory { Id = 1, Name = "Szef", CategoryId = 1 },
                new Subcategory { Id = 2, Name = "Klient", CategoryId = 1 },
                new Subcategory { Id = 3, Name = "Współpracownik", CategoryId = 1 }
            );

            modelBuilder.Entity<Subcategory>()
                .HasOne(s => s.Category)
                .WithMany(c => c.Subcategories)
                .HasForeignKey(s => s.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
