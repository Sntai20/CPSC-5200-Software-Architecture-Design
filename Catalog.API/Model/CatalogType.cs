namespace eShop.Catalog.API.Model;
using System.ComponentModel.DataAnnotations;

public class CatalogType
{
    public int Id { get; set; }

    [Required]
    public string Type { get; set; }
}
