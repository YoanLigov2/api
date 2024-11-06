using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Data
{
    public class StockContext : DbContext
    {
        public StockContext(DbContextOptions<StockContext> options) : base(options) { }

        public DbSet<StockPrice> StockPrices { get; set; }
    }
}