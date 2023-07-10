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
    
    public async Task<List<Category>> GetCategoriesAsync(CancellationToken cancellationToken)
    {
        return await _context.Categories.ToListAsync(cancellationToken);
    }
    
    public async Task<Category> GetCategoryByIdAsync(int id, CancellationToken cancellationToken) 
    {
        return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }
    
    public async Task<List<Category>> GetParentCategoriesAsync(CancellationToken cancellationToken)
    {
        return await _context.Categories
            .Where(c => c.ParentCategoryId == null)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<List<Category>> GetChildCategoriesAsync(int parentCategoryId, CancellationToken cancellationToken)
    {
        return await _context.Categories
            .Where(c => c.ParentCategoryId == parentCategoryId)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Category category, CancellationToken cancellationToken)
    {
        await _context.Categories.AddAsync(category, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task EditAsync(Category category, CancellationToken cancellationToken)
    {
        _context.Categories.Update(category);
        await _context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        _context.Categories.Remove(await GetCategoryByIdAsync(id, cancellationToken));
        await _context.SaveChangesAsync(cancellationToken);
    }
}