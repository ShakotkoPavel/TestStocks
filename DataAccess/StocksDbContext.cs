using Microsoft.EntityFrameworkCore;
using TestStocks.DataAccess.Entities;

namespace TestStocks.DataAccess
{
    public class StocksDbContext : DbContext
    {
        public StocksDbContext(DbContextOptions<StocksDbContext> options) : base(options) 
        {
            Database.EnsureCreated();
        }

        public DbSet<StockEntity> Stocks { get; set; }

        public DbSet<ResultEntity> StockResults { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StockEntity>().ToTable("Stocks").HasKey(x => x.Id);

            modelBuilder.Entity<ResultEntity>().ToTable("StockResults")
                .HasOne<StockEntity>(x => x.Stock)
                .WithMany(x => x.Results)
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey(x => x.StockId);
        }
    }
}
