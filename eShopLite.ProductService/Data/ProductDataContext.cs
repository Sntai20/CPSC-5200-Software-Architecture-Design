namespace eShopLite.ProductService.Data;

using DataEntities;
using Microsoft.EntityFrameworkCore;

public class ProductDataContext(DbContextOptions<ProductDataContext> options) : DbContext(options)
{
    public DbSet<Product> Product { get; set; } = default!;
}
