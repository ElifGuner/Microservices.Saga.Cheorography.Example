using Microsoft.EntityFrameworkCore;
using System.Data.Entity;

namespace StockAPI.Models
{
    public class StockAPIDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public StockAPIDbContext(DbContextOptions options) : base(options)
        {

        }
        public Microsoft.EntityFrameworkCore.DbSet<Stock> Stocks { get; set; }

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
}
