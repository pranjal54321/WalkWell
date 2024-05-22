using Microsoft.EntityFrameworkCore;

namespace FinalProject.Models
{
    public class FinalDbContext:DbContext
    {

        public FinalDbContext(DbContextOptions<FinalDbContext> options) : base(options) { }

        public  DbSet<User> Users { get; set; }

        public  DbSet<Product> Products { get; set; }

        public  DbSet<Category> Categories { get; set; }

        public  DbSet<Cart> Carts { get; set; }

        public  DbSet<Order> Orders { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(t => t.UserType)
                .HasDefaultValue("Customer");

        }


    }
}
