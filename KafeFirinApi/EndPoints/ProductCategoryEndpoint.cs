using SharedClasses;
using Microsoft.EntityFrameworkCore;
using SharedClass.Classes;

namespace KafeFirinApi.EndPoints
{
    public static class ProductCategoryEndpoint
    {
        public static void MapProductCategoryEndpoints(this IEndpointRouteBuilder routes)
        {
            routes.MapGet("/api/productcategories", async (AppDbContext db) =>
            {
                var productCategories = await db.ProductCategory.ToListAsync();
                return Results.Ok(productCategories);
            });
            routes.MapGet("/api/productcategories/{id}", async (int id, AppDbContext db) =>
            {
                var productCategory = await db.ProductCategory.FindAsync(id);
                return productCategory is not null ? Results.Ok(productCategory) : Results.NotFound();
            });
            routes.MapPost("/api/productcategories", async (ProductCategory productCategory, AppDbContext db) =>
            {
                db.ProductCategory.Add(productCategory);
                await db.SaveChangesAsync();
                return Results.Created($"/api/productcategories/{productCategory.CategoryID}", productCategory);
            });
            routes.MapDelete("/api/productcategories/{id}", async (int id, AppDbContext db) =>
            {
                var productCategory = await db.ProductCategory.FindAsync(id);
                if (productCategory is null)
                {
                    return Results.NotFound();
                }
                db.Remove(productCategory);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });
            routes.MapPut("/api/productcategories/{id}", async (int id, ProductCategory updatedProductCategory, AppDbContext db) =>
            {
                var productCategory = await db.ProductCategory.FindAsync(id);
                if (productCategory is null)
                {
                    return Results.NotFound();
                }
                productCategory.CategoryName = updatedProductCategory.CategoryName;
                await db.SaveChangesAsync();
                return Results.NoContent();
            });
        }
    }
}
