using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ShoeStoreASP.Models.Domain
{
    public class DatabaseContext: IdentityDbContext<ApplicationUser>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options): base(options) { 
        }


        public DbSet<Shoe> Shoes { get; set; }
        public DbSet<Type> Types { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartShoe> CartShoes {  get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<ShoeType> ShoeTypes { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceShoe> InvoiceShoes { get; set; }
       
      
    }
}
