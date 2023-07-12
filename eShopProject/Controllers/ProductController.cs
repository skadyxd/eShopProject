using eShopProject.Models;
using eShopProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace eShopProject.Controllers;

public class ProductController : Controller
{
    private readonly ProductService _productService;
    
    public ProductController(ProductService productService)
    {
        _productService = productService;
    }

    /// <summary>
    /// Отображает главную страницу, со списком всех продуктов.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        return View(await _productService.GetProductsAsync(cancellationToken));
    }
    
    /// <summary>
    /// Отображает страницу с подробной информацией по указанному идентификатору.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        return View(await _productService.GetProductByIdAsync(id, cancellationToken));
    }

    /// <summary>
    /// Создает новый продукт. Поле "Готов к продаже?" по умолчанию равно "false".
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(Product product, CancellationToken cancellationToken)
    {
        product.ReadyToSale = false;
        await _productService.AddAsync(product, cancellationToken);
        
        return RedirectToAction("Index");
    }
    
    /// <summary>
    /// Редактирует выбранный продукт.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Edit(Product product, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .ToDictionary(x => x.Key, x => x.Value.Errors
                    .Select(e => e.ErrorMessage)
                    .ToArray());
            return BadRequest(errors);
        }

        await _productService.EditAsync(product, cancellationToken);
        return Json(new { redirectUrl= Url.Action("Details", new { id = product.Id})});
    }
    
    /// <summary>
    /// Удаляет выбранный продукт.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _productService.DeleteAsync(id, cancellationToken);
        return RedirectToAction("Index");
    }
}