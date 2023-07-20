using eShopProject.Models;
using Microsoft.EntityFrameworkCore;


namespace eShopProject.Services;

public class ProductService
{
    private readonly ApplicationDbContext _context;

    public ProductService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    
    /// <summary>
    /// Получает список всех продуктов.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены для асинхронной операции.</param>
    /// <returns>Список всех продуктов.</returns>
    public async Task<List<Product>> GetProductsAsync(CancellationToken cancellationToken)
    {
        return await _context.Products
            .Include(p => p.Category)
            .ToListAsync(cancellationToken);
    }
    
    /// <summary>
    /// Получает продукт по указанному идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор продукта.</param>
    /// <param name="cancellationToken">Токен отмены для асинхронной операции.</param>
    /// <returns>Продукт с указанным идентификатором, либо null, если категория не найдена.</returns>
    public async Task<Product> GetProductByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    /// <summary>
    /// Получает список продуктов, которые будут отсортированы по цене по убыванию.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены для асинхронной операции.</param>
    /// <returns>Список продуктов отсортированных по цене по убыванию</returns>
    public async Task<List<Product>> SortByPriceDescendingAsync(CancellationToken cancellationToken)
    {
        return await _context.Products
            .Include(p => p.Category)
            .OrderByDescending(x => x.Price)
            .ToListAsync(cancellationToken);

    }
    
    /// <summary>
    /// Получает список продуктов, которые будут отсортированы по цене по возрастанию.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены для асинхронной операции.</param>
    /// <returns>Список продуктов отсортированных по цене по возрастанию</returns>
    public async Task<List<Product>> SortByPriceAscendingAsync(CancellationToken cancellationToken)
    {
        return await _context.Products
            .Include(p => p.Category)
            .OrderBy(x => x.Price)
            .ToListAsync(cancellationToken);

    }
    
    /// <summary>
    /// Добавляет новый продукт.
    /// </summary>
    /// <param name="product">Добавляемый продукт.</param>
    /// <param name="cancellationToken">Токен отмены для асинхронной операции.</param>
    public async Task AddAsync(Product product, CancellationToken cancellationToken)
    {
        await _context.Products.AddAsync(product, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
    
    /// <summary>
    /// Редактирует выбранный продукт.
    /// </summary>
    /// <param name="product">Редактируемый продукт.</param>
    /// <param name="cancellationToken">Токен отмены для асинхронной операции.</param>
    public async Task EditAsync(Product product, CancellationToken cancellationToken)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync(cancellationToken);
    }
    
    /// <summary>
    /// Удаляет продукт по указанному идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор удаляемоего продукта.</param>
    /// <param name="cancellationToken">Токен отмены для асинхронной операции.</param>
    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        _context.Products.Remove(await GetProductByIdAsync(id, cancellationToken));
        await _context.SaveChangesAsync(cancellationToken);
    }
}