using Bed4Head.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bed4Head.DAL.EF
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }



        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id)
                      .HasDefaultValueSql("gen_random_uuid()");
            });
        }
    }
}