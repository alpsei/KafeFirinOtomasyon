using KafeFirinApi.Data;
using SharedClasses;
using Microsoft.EntityFrameworkCore;

namespace KafeFirinApi.EndPoints
{
    public static class UserEndpoint
    {
        public static void MapUserEndpoints(this IEndpointRouteBuilder routes)
        {
            routes.MapGet("/api/users", async (AppDbContext db) =>
            {
                var users = await db.Users.Include(u => u.Roles).ToListAsync();
                return Results.Ok(users);
            });
            routes.MapGet("/api/users/{id}", async (int id, AppDbContext db) =>
            {
                var user = await db.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.UserID == id);
                return user is not null ? Results.Ok(user) : Results.NotFound();
            });
            routes.MapPost("/api/users", async (Users user, AppDbContext db) =>
            {
                db.Users.Add(user);
                await db.SaveChangesAsync();
                return Results.Created($"/api/users/{user.UserID}", user);
            });
            routes.MapDelete("/api/users/{id}", async (int id, AppDbContext db) =>
            {
                var user = await db.Users.FindAsync(id);
                if (user is null)
                {
                    return Results.NotFound();
                }
                db.Users.Remove(user);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });
            routes.MapPut("/api/users/{id}", async (int id, Users updatedUser, AppDbContext db) =>
            {
                var user = await db.Users.FindAsync(id);
                if (user is null)
                {
                    return Results.NotFound();
                }
                user.Username = updatedUser.Username;
                user.Password = updatedUser.Password;
                user.Email = updatedUser.Email;
                user.Roles = updatedUser.Roles;
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            routes.MapGet("api/users/login", async (string username, string password, AppDbContext db) =>
            {
                var user = await db.Users.FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
                if (user is null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(user);
            });
        }
    }
}
