using Microsoft.EntityFrameworkCore;

namespace eShopProject.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderProduct> OrderProducts { get; set; }
}

public class DbSeeder
{
    private readonly ApplicationDbContext _context;

    public DbSeeder(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task SeedAsync()
    {
        var exist = await _context.Products.AnyAsync();

        if (exist)
        {
            return;
        }
        
        var categories = new List<Category>
        {
            new Category { Name = "Категория 1" },
            new Category { Name = "Категория 2" },
        };

        await _context.Categories.AddRangeAsync(categories);
        await _context.SaveChangesAsync();
        
        var products = Enumerable.Range(1, 100)
            .Select(x => new Product
            {
                Name = $"Milk #{x}",
                Description = $"Product #{x}",
                CategoryId = categories[new Random().Next(categories.Count)].Id,
                Price = new Random().Next(100, 10000),
                ReadyToSale = false
            })
            .ToList();

        await _context.Products.AddRangeAsync(products);
        await _context.SaveChangesAsync();
    }
    
}