using Microsoft.EntityFrameworkCore;

namespace StockAPI.Models
{
    public class StockAPIDbContext : DbContext
    {
        public StockAPIDbContext(DbContextOptions options) : base(options)
        {

        }
        public Dbset<Stock> Stocks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Stock>().HasData(
             new Stock() { Id = 1, ProductId = Guid.NewGuid(), Count = 2000 },
             new Stock() { Id = 2, ProductId = Guid.NewGuid(), Count = 1000 },
             new Stock() { Id = 3, ProductId = Guid.NewGuid(), Count = 3000 },
             new Stock() { Id = 4, ProductId = Guid.NewGuid(), Count = 5000 },
             new Stock() { Id = 5, ProductId = Guid.NewGuid(), Count = 500 }
            );
        }
}
