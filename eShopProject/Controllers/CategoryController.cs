using Microsoft.AspNetCore.Mvc;
using eShopProject.Models;
using eShopProject.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class CategoryController : Controller
{
    private readonly CategoryService _categoryService;
    
    public CategoryController(CategoryService categoryService)
    {
        _categoryService = categoryService;
    }
    
    [HttpGet]
    public async Task<IActionResult> Catalog(CancellationToken cancellationToken)
    {
        var categories = await _categoryService.GetParentCategoriesAsync(cancellationToken);
        return View(categories);
    }

}