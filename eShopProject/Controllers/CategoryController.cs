using eShopProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace eShopProject.Controllers;

public class CategoryController : Controller
{
    private readonly CategoryService _categoryService;
    
    public CategoryController(CategoryService categoryService)
    {
        _categoryService = categoryService;
    }
    
    /// <summary>
    /// Отображает каталог.
    /// </summary>
    [HttpGet]
    public IActionResult Catalog()
    {
        return View();
    }
    
    /// <summary>
    /// Получает список категорий, которые являются родительскими категориями в формате JSON.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetParentCategoriesJson(CancellationToken cancellationToken)
    {
        var parentCategories = await _categoryService.GetParentCategoriesAsync(cancellationToken);
        return Json(parentCategories);
    }
    
    /// <summary>
    /// Получает список дочерних категорий для указанного идентификатора родительской категории в формате JSON.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetChildCategoriesJson(int parentCategoryId, CancellationToken cancellationToken)
    {
        var childCategories = await _categoryService.GetChildCategoriesAsync(parentCategoryId, cancellationToken);
        return Json(childCategories);
    }
    
    /// <summary>
    /// Получает список конечных (листовых) категорий, которые не являются родительскими категориями в формате JSON.
    /// </summary>
    public async Task<IActionResult> GetLeafCategoriesJson()
    {
        var categories = await _categoryService.GetLeafCategoriesAsync(CancellationToken.None);
        return Json(categories);
    }


}