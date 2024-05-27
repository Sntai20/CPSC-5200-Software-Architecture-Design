namespace eShop.WebAppComponents.Item;
using eShop.WebAppComponents.Catalog;

public static class ItemHelper
{
    public static string Url(CatalogItem item)
        => $"item/{item.Id}";
}
