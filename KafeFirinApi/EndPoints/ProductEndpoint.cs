using KafeFirinApi.Data;
using SharedClasses;
using Microsoft.EntityFrameworkCore;

namespace KafeFirinApi.EndPoints
{
    public static class ProductEndpoint
    {
        public static void MapProductEndpoints(this IEndpointRouteBuilder routes)
        {
            routes.MapGet("/api/products", async (AppDbContext db) =>
            {
                var products = await db.Products.Include(p => p.ProductID).ToListAsync();
                return Results.Ok(products);
            });
            routes.MapGet("/api/products/{id}", async (int id, AppDbContext db) =>
            {
                var product = await db.Products.Include(p => p.ProductID).FirstOrDefaultAsync(p => p.ProductID == id);
                return product is not null ? Results.Ok(product) : Results.NotFound();
            });
            routes.MapPost("/api/products", async (Products product, AppDbContext db) =>
            {
                db.Products.Add(product);
                await db.SaveChangesAsync();
                return Results.Created($"/api/products/{product.ProductID}", product);
            });
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
