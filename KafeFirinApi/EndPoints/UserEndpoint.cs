using SharedClass;
using Microsoft.EntityFrameworkCore;
using SharedClass.Classes;
using Microsoft.AspNetCore.Mvc;

namespace KafeFirinApi.EndPoints
{
    public static class UserEndpoint
    {
        public static void MapUserEndpoints(this IEndpointRouteBuilder routes)
        {
            // kullanıcıları listele
            routes.MapGet("/api/users", async (AppDbContext db) =>
            {
                var users = await db.Users.Include(u => u.Roles).ToListAsync();
                return Results.Ok(users);
            });
            // kullanıcı id ile arama
            routes.MapGet("/api/users/by-id/{id}", async (int id, AppDbContext db) =>
            {
                var user = await db.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.UserID == id);
                return user is not null ? Results.Ok(user) : Results.NotFound();
            });
            // kullanıcı adı ile arama
            routes.MapGet("/api/users/by-username/{username}", async (string username, AppDbContext db) =>
            {
                var user = await db.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Username == username);
                return user is not null ? Results.Ok(user) : Results.NotFound();
            });
            // kullanıcı ekle
            routes.MapPost("/api/users", async (Users user, int? createdBy, AppDbContext db) =>
            {
                var mudurid = user.RoleID == 3;
                if (user.RoleID == 1)
                    user.CreatedBy = null;
                else if (user.RoleID == 3)
                    user.CreatedBy = createdBy;

                db.Users.Add(user);
                await db.SaveChangesAsync();
                return Results.Created($"/api/users/{user.UserID}", user);
            });
            // kullanıcı sil
            routes.MapDelete("/api/users/by-id/{id}", async (int id, AppDbContext db) =>
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
            // kullanıcı adı ile sil
            routes.MapDelete("/api/users/by-username/{username}", async (string username, AppDbContext db) =>
            {
                var user = await db.Users.FirstOrDefaultAsync(u => u.Username == username);
                if (user is null)
                {
                    return Results.NotFound();
                }
                db.Users.Remove(user);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });
            // kullanıcı güncelle
            routes.MapPut("/api/users/{id}", async (int id, int? updatedBy, Users updatedUser, AppDbContext db) =>
            {
                var user = await db.Users.FindAsync(id);
                if (user is null)
                {
                    return Results.NotFound();
                }

                user.Username = updatedUser.Username;
                user.Password = updatedUser.Password;
                user.Email = updatedUser.Email;
                user.FirstName = updatedUser.FirstName;
                user.LastName = updatedUser.LastName;
                user.SecQuestion = updatedUser.SecQuestion;
                user.SecAnswer = updatedUser.SecAnswer;
                user.Salary = updatedUser.Salary;
                user.RoleID = updatedUser.RoleID;
                user.UpdatedAt = DateTime.Now;
                user.UpdatedBy = updatedBy;

                await db.SaveChangesAsync();
                return Results.NoContent();
            });
            // kullanıcı giriş
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
