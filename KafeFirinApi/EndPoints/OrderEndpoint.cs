using Microsoft.EntityFrameworkCore;
using SharedClass.Classes;

namespace KafeFirinApi.EndPoints
{
    public static class OrderEndpoint
    {
        public static void MapOrderEndpoints(this IEndpointRouteBuilder routes)
        {
            routes.MapGet("/api/orders", async (AppDbContext db) =>
            {
                var orders = await db.Orders.ToListAsync();
                return Results.Ok(orders);
            });
            routes.MapGet("/api/orders/{id}", async (int id, AppDbContext db) =>
            {
                var order = await db.Orders.FirstOrDefaultAsync(o => o.OrderID == id);
                return order is not null ? Results.Ok(order) : Results.NotFound();
            });
            routes.MapPost("/api/orders", async (Orders order, AppDbContext db) =>
            {
                db.Orders.Add(order);
                await db.SaveChangesAsync();
                return Results.Created($"/api/orders/{order.OrderID}", order);
            });
            routes.MapDelete("/api/orders/{id}", async (int id, AppDbContext db) =>
            {
                var order = await db.Orders.FindAsync(id);
                if (order is null)
                {
                    return Results.NotFound();
                }
                db.Orders.Remove(order);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });
            routes.MapPut("/api/orders/{id}", async (int id, Orders updatedOrder, AppDbContext db) =>
            {
                var order = await db.Orders.FindAsync(id);
                if (order is null)
                {
                    return Results.NotFound();
                }
                order.CustomerID = updatedOrder.CustomerID;
                order.StaffID = updatedOrder.StaffID;
                order.OrderStatus = updatedOrder.OrderStatus;
                order.OrderNote = updatedOrder.OrderNote;
                order.DiscountApplied = updatedOrder.DiscountApplied;
                order.OrderDate = updatedOrder.OrderDate;
                await db.SaveChangesAsync();
                return Results.NoContent();
            });
        }
    }
}
