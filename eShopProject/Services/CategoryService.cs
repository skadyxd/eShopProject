using eShopProject.Models;
using Microsoft.EntityFrameworkCore;

namespace eShopProject.Services;

public class CategoryService
{
    private readonly ApplicationDbContext _context;

    public CategoryService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    /// <summary>
    /// Получает список всех категорий.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены для асинхронной операции.</param>
    /// <returns>Список всех категорий.</returns>
    public async Task<List<Category>> GetCategoriesAsync(CancellationToken cancellationToken)
    {
        return await _context.Categories.ToListAsync(cancellationToken);
    }
    
    /// <summary>
    /// Получает категорию по указанному идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор категории.</param>
    /// <param name="cancellationToken">Токен отмены для асинхронной операции.</param>
    /// <returns>Категория с указанным идентификатором, либо null, если категория не найдена.</returns>
    public async Task<Category> GetCategoryByIdAsync(int id, CancellationToken cancellationToken) 
    {
        return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }
    
    /// <summary>
    /// Получает список категорий, которые являются родительскими категориями.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены для асинхронной операции.</param>
    /// <returns>Список родительских категорий.</returns>
    public async Task<List<Category>> GetParentCategoriesAsync(CancellationToken cancellationToken)
    {
        return await _context.Categories
            .Where(c => c.ParentCategoryId == null)
            .ToListAsync(cancellationToken);
    }
    
    /// <summary>
    /// Получает список дочерних категорий для указанного идентификатора родительской категории.
    /// </summary>
    /// <param name="parentCategoryId">Идентификатор родительской категории.</param>
    /// <param name="cancellationToken">Токен отмены для асинхронной операции.</param>
    /// <returns>Список дочерних категорий для указанного родительского идентификатора.</returns>
    public async Task<List<Category>> GetChildCategoriesAsync(int parentCategoryId, CancellationToken cancellationToken)
    {
        return await _context.Categories
            .Where(c => c.ParentCategoryId == parentCategoryId)
            .ToListAsync(cancellationToken);
    }
    
    /// <summary>
    /// Получает список конечных (листовых) категорий, которые не являются родительскими категориями.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены для асинхронной операции.</param>
    /// <returns>Список конечных (листовых) категорий.</returns>
    public async Task<List<Category>> GetLeafCategoriesAsync(CancellationToken cancellationToken)
    {
        var parentCategoryIds = await _context.Categories.Select(c => c.ParentCategoryId).ToListAsync(cancellationToken);
        return await _context.Categories.Where(c => !parentCategoryIds.Contains(c.Id)).ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Добавляет новую категорию.
    /// </summary>
    /// <param name="category">Добавляемая категория.</param>
    /// <param name="cancellationToken">Токен отмены для асинхронной операции.</param>
    public async Task AddAsync(Category category, CancellationToken cancellationToken)
    {
        await _context.Categories.AddAsync(category, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
    
    /// <summary>
    /// Редактирует категорию.
    /// </summary>
    /// <param name="category">Редактируемая категория.</param>
    /// <param name="cancellationToken">Токен отмены для асинхронной операции.</param>
    public async Task EditAsync(Category category, CancellationToken cancellationToken)
    {
        _context.Categories.Update(category);
        await _context.SaveChangesAsync(cancellationToken);
    }
    
    /// <summary>
    /// Удаляет категорию по указанному идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор удаляемой категории.</param>
    /// <param name="cancellationToken">Токен отмены для асинхронной операции.</param>
    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        _context.Categories.Remove(await GetCategoryByIdAsync(id, cancellationToken));
        await _context.SaveChangesAsync(cancellationToken);
    }
}