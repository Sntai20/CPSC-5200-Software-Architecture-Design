namespace eShop.WebApp.Services;
using eShop.WebAppComponents.Services;

public class ProductImageUrlProvider : IProductImageUrlProvider
{
    public string GetProductImageUrl(int productId)
        => $"product-images/{productId}";
}
