using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


var cache = new MemoryCache(new MemoryCacheOptions());


app.UseCors(policy =>
    policy.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader());


app.MapGet("/api/productlist", () =>
{
    if (!cache.TryGetValue("products", out Product[] products))
    {
        products = new[]
        {
            new Product { Id = 1, Name = "Laptop", Price = 1200.50, Stock = 25, Category = new Category { Id = 101, Name = "Electronics" }},
            new Product { Id = 2, Name = "Headphones", Price = 50.00, Stock = 100, Category = new Category { Id = 102, Name = "Accessories" }}
        };

        cache.Set("products", products, TimeSpan.FromMinutes(10)); 
    }

    return Results.Json(products, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
});


app.Run();


public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public int Stock { get; set; }
    public Category Category { get; set; }
}

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
}




