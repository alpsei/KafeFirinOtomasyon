using KafeFirinApi.Data;
using SharedClasses;
using Microsoft.EntityFrameworkCore;

namespace KafeFirinApi.EndPoints
{
    public static class OrderDetailsEndpoint
    {
        public static void MapOrderDetailsEndpoints(this IEndpointRouteBuilder routes)
        {
            routes.MapGet("/orderdetails", async (AppDbContext db) =>
            {
                return await db.OrderDetails.ToListAsync();
            })
            .WithName("GetAllOrderDetails");
            routes.MapGet("/orderdetails/{id}", async (int id, AppDbContext db) =>
            {
                return await db.OrderDetails.FindAsync(id)
                    is OrderDetails orderDetails
                        ? Results.Ok(orderDetails)
                        : Results.NotFound();
            })
            .WithName("GetOrderDetailsById");
            routes.MapPost("/orderdetails", async (OrderDetails orderDetails, AppDbContext db) =>
            {
                db.OrderDetails.Add(orderDetails);
                await db.SaveChangesAsync();
                return Results.Created($"/orderdetails/{orderDetails.DetailID}", orderDetails);
            })
            .WithName("CreateOrderDetails");
            routes.MapPut("/orderdetails/{id}", async (int id, OrderDetails updatedOrderDetails, AppDbContext db) =>
            {
                var orderDetails = await db.OrderDetails.FindAsync(id);
                if (orderDetails is null) return Results.NotFound();
                orderDetails.Quantity = updatedOrderDetails.Quantity;
                await db.SaveChangesAsync();
                return Results.NoContent();
            })
            .WithName("UpdateOrderDetails");
            routes.MapDelete("/orderdetails/{id}", async (int id, AppDbContext db) =>
            {
                if (await db.OrderDetails.FindAsync(id) is OrderDetails orderDetails)
                {
                    db.OrderDetails.Remove(orderDetails);
                    await db.SaveChangesAsync();
                    return Results.Ok(orderDetails);
                }
                return Results.NotFound();
            })
            .WithName("DeleteOrderDetails");
        }
    }
}
