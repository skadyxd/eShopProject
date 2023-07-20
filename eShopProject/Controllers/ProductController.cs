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
    public IActionResult Index()
    {
        return View();
    }
    
    [HttpGet]
    public async Task<IActionResult> GetProductsSortJson(string sortName, string sortOrder, CancellationToken cancellationToken)
    {
        // var products = await _productService.GetProductsAsync(CancellationToken.None);
        // return Json(products);
        switch (sortName.ToLower())
        {
            case "price":
                if (sortOrder.ToLower() == "desc")
                {
                    var products = await _productService.SortByPriceDescendingAsync(cancellationToken);
                    return Json(products);
                }
                else
                {
                    var products = await _productService.SortByPriceAscendingAsync(cancellationToken);
                    return Json(products);
                }
            
        }

        return Json(await _productService.GetProductsAsync(cancellationToken));
    }
    
    [HttpGet]
    public async Task<IActionResult> GetProductsJson(CancellationToken cancellationToken)
    {
        var products = await _productService.GetProductsAsync(cancellationToken);
        return Json(products);
    }
    
    /// <summary>
    /// Выдает список продуктов отсортированных по цене по убыванию в виде json файла
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetSortByPriceDescendingJson()
    {
        var products = await _productService.SortByPriceDescendingAsync(CancellationToken.None);
        return Json(products);
    }
    
    /// <summary>
    /// Выдает список продуктов отсортированных по цене по возрастанию в виде json файла
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetSortByPriceAscendingJson()
    {
        var products = await _productService.SortByPriceAscendingAsync(CancellationToken.None);
        return Json(products);
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