namespace eShop.Catalog.API.Model;
using System.ComponentModel.DataAnnotations;

public class CatalogBrand
{
    public int Id { get; set; }

    [Required]
    public string Brand { get; set; }
}
