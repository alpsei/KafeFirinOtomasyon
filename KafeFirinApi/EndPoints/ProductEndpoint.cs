using Microsoft.EntityFrameworkCore;
using SharedClass.Classes;

namespace KafeFirinApi.EndPoints
{
    public static class ProductEndpoint
    {
        public static void MapProductEndpoints(this IEndpointRouteBuilder routes)
        {
            // Tüm ürünleri getir (Category bilgisiyle birlikte)
            routes.MapGet("/api/products", async (AppDbContext db) =>
            {
                var products = await db.Products
                    .ToListAsync();
                return Results.Ok(products);
            });

            // ID'ye göre tek bir ürün getir
            routes.MapGet("/api/products/{id}", async (int id, AppDbContext db) =>
            {
                var product = await db.Products
                    .FirstOrDefaultAsync(p => p.ProductID == id);
                return product is not null ? Results.Ok(product) : Results.NotFound();
            });

            // Yeni ürün ekle
            routes.MapPost("/api/products", async (Products product, AppDbContext db) =>
            {
                db.Products.Add(product);
                await db.SaveChangesAsync();
                return Results.Created($"/api/products/{product.ProductID}", product);
            });

            // Ürün sil
            routes.MapDelete("/api/products/{id}", async (int id, AppDbContext db) =>
            {
                var product = await db.Products.FindAsync(id);
                if (product is null)
                {
                    return Results.NotFound();
                }
                db.Products.Remove(product);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            // Ürün güncelle
            routes.MapPut("/api/products/{id}", async (int id, Products updatedProduct, AppDbContext db) =>
            {
                var product = await db.Products.FindAsync(id);
                if (product is null)
                {
                    return Results.NotFound();
                }
                product.ProductName = updatedProduct.ProductName;
                product.Price = updatedProduct.Price;
                product.Stock = updatedProduct.Stock;
                product.CategoryID = updatedProduct.CategoryID;
                await db.SaveChangesAsync();
                return Results.NoContent();
            });
        }
    }
}