var builder = DistributedApplication.CreateBuilder(args);

var products = builder.AddProject<Projects.eShopLite_ProductService>("productservice");
builder.AddProject<Projects.eShopLite_Frontend>("frontend").WithReference(products);

builder.Build().Run();
