using Microsoft.EntityFrameworkCore;
using SharedClass.Classes;

namespace KafeFirinApi.EndPoints
{
    public static class FeedBackEndpoint
    {
        public static void MapFeedBackEndpoints(this IEndpointRouteBuilder routes)
        {
            routes.MapGet("/feedback", async (AppDbContext db) =>
            {
                return await db.FeedBacks.ToListAsync();
            })
            .WithName("GetAllFeedBacks");
            routes.MapGet("/feedback/{id}", async (int id, AppDbContext db) =>
            {
                return await db.FeedBacks.FindAsync(id)
                    is FeedBacks feedBack
                        ? Results.Ok(feedBack)
                        : Results.NotFound();
            })
            .WithName("GetFeedBackById");
            routes.MapPost("/feedback", async (FeedBacks feedBack, AppDbContext db) =>
            {
                db.FeedBacks.Add(feedBack);
                await db.SaveChangesAsync();
                return Results.Created($"/feedback/{feedBack.FeedBackID}", feedBack);
            })
            .WithName("CreateFeedBack");
            routes.MapPut("/feedback/{id}", async (int id, FeedBacks updatedFeedBack, AppDbContext db) =>
            {
                var feedBack = await db.FeedBacks.FindAsync(id);
                if (feedBack is null) return Results.NotFound();

                feedBack.Content = updatedFeedBack.Content;
                feedBack.Subject = updatedFeedBack.Subject;
                feedBack.FBDate = updatedFeedBack.FBDate;
                feedBack.ReadReceipt = updatedFeedBack.ReadReceipt;

                await db.SaveChangesAsync();
                return Results.NoContent();
            })
            .WithName("UpdateFeedBack");
            routes.MapDelete("/feedback/{id}", async (int id, AppDbContext db) =>
            {
                if (await db.FeedBacks.FindAsync(id) is FeedBacks feedBack)
                {
                    db.FeedBacks.Remove(feedBack);
                    await db.SaveChangesAsync();
                    return Results.Ok(feedBack);
                }
                return Results.NotFound();
            })
            .WithName("DeleteFeedBack");
            routes.MapGet("/feedback/user/{userId}", async (int userId, AppDbContext db, ILogger<RateEndpointsLogging> logger) =>
            {
                logger.LogInformation("GET /feedback/user/{userId} çağrıldı.", userId);
                try
                {
                    var feedbacks = await db.FeedBacks
                        .Where(f => f.CustomerID == userId)
                        .ToListAsync();
                    logger.LogInformation("{UserId} ID'li müşteriye ait {Count} adet geri bildirim bulundu.", userId, feedbacks.Count);
                    return Results.Ok(feedbacks);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "GET /feedback/user/{UserId} sırasında bir hata oluştu.", userId);
                    return Results.Problem("Müşteriye ait geri bildirimler listelenirken bir sorun oluştu.", statusCode: StatusCodes.Status500InternalServerError);
                }
            })
            .WithName("GetFeedbacksByUserId");
        }
    }
}
