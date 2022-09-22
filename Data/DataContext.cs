using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace ApiCart.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<Carrinho> Carrinho => Set<Carrinho>();
        public DbSet<Produto> Produto => Set<Produto>();
        public DbSet<CarrinhoProduto> CarrinhoProduto => Set<CarrinhoProduto>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CarrinhoProduto>()
                .HasOne(p => p.Carrinho)
                .WithMany(p => p.CarrinhoProduto)
                .HasForeignKey(p => p.IdCarrinho);
        }
    }
}
