namespace eShopProject.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int? ParentCategoryId { get; set; }
    
    public bool IsLeafCategory { get; set; }
}