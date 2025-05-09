using Microsoft.EntityFrameworkCore;
using SharedClass.Classes;
using SharedClasses;

namespace KafeFirinApi.EndPoints
{
    public static class FavoriteEndpoint
    {
        public static void MapFavoriteEndpoints(this IEndpointRouteBuilder routes)
        {
            routes.MapGet("/favorite", async (AppDbContext db) =>
            {
                return await db.Favorites.ToListAsync();
            })
            .WithName("GetAllFavorites");
            routes.MapGet("/favorite/{id}", async (int id, AppDbContext db) =>
            {
                return await db.Favorites.FindAsync(id)
                    is Favorites favorite
                        ? Results.Ok(favorite)
                        : Results.NotFound();
            })
            .WithName("GetFavoriteById");
            routes.MapPost("/favorite", async (Favorites favorite, AppDbContext db) =>
            {
                db.Favorites.Add(favorite);
                await db.SaveChangesAsync();
                return Results.Created($"/favorite/{favorite.FavID}", favorite);
            })
            .WithName("CreateFavorite");
            routes.MapPut("/favorite/{id}", async (int id, Favorites updatedFavorite, AppDbContext db) =>
            {
                var favorite = await db.Favorites.FindAsync(id);
                if (favorite is null) return Results.NotFound();
                favorite.ProductID = updatedFavorite.ProductID;
                await db.SaveChangesAsync();
                return Results.NoContent();
            })
            .WithName("UpdateFavorite");
            routes.MapDelete("/favorite/{id}", async (int id, AppDbContext db) =>
            {
                if (await db.Favorites.FindAsync(id) is Favorites favorite)
                {
                    db.Favorites.Remove(favorite);
                    await db.SaveChangesAsync();
                    return Results.Ok(favorite);
                }
                return Results.NotFound();
            })
            .WithName("DeleteFavorite");
        }
    }
}
