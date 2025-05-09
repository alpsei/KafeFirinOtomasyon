using SharedClass;
using Microsoft.EntityFrameworkCore;
using SharedClass.Classes;

namespace KafeFirinApi.EndPoints
{
    public static class RateEnpoint
    {
        public static void MapRateEndpoints(this IEndpointRouteBuilder routes)
        {
            routes.MapGet("/rate", async (AppDbContext db) =>
            {
                return await db.Rates.ToListAsync();
            })
            .WithName("GetAllRates");
            routes.MapGet("/rate/{id}", async (int id, AppDbContext db) =>
            {
                return await db.Rates.FindAsync(id)
                    is Rates rate
                        ? Results.Ok(rate)
                        : Results.NotFound();
            })
            .WithName("GetRateById");
            routes.MapPost("/rate", async (Rates rate, AppDbContext db) =>
            {
                db.Rates.Add(rate);
                await db.SaveChangesAsync();
                return Results.Created($"/rate/{rate.RateID}", rate);
            })
            .WithName("CreateRate");
            routes.MapPut("/rate/{id}", async (int id, Rates updatedRate, AppDbContext db) =>
            {
                var rate = await db.Rates.FindAsync(id);
                if (rate is null) return Results.NotFound();
                rate.Rate= updatedRate.Rate;
                await db.SaveChangesAsync();
                return Results.NoContent();
            })
            .WithName("UpdateRate");
            routes.MapDelete("/rate/{id}", async (int id, AppDbContext db) =>
            {
                if (await db.Rates.FindAsync(id) is Rates rate)
                {
                    db.Rates.Remove(rate);
                    await db.SaveChangesAsync();
                    return Results.Ok(rate);
                }
                return Results.NotFound();
            })
            .WithName("DeleteRate");
        }
    }
}
