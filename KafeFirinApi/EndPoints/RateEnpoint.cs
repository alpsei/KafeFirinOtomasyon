using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SharedClass;
using SharedClass.Classes;

namespace KafeFirinApi.EndPoints
{
    public class RateEndpointsLogging { }

    public static class RateEnpoint
    {
        public static void MapRateEndpoints(this IEndpointRouteBuilder routes)
        {
            routes.MapGet("/rate", async (AppDbContext db, ILogger<RateEndpointsLogging> logger) =>
            {
                try
                {
                    logger.LogInformation("GET /rate çağrıldı - Tüm puanlar listeleniyor.");
                    var rates = await db.Rates.ToListAsync();
                    logger.LogInformation("{Count} adet puan bulundu.", rates.Count);
                    return Results.Ok(rates);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "GET /rate sırasında bir hata oluştu.");
                    return Results.Problem("Puanlar listelenirken bir sorun oluştu.", statusCode: StatusCodes.Status500InternalServerError);
                }
            })
            .WithName("GetAllRates");

            routes.MapGet("/rate/{id}", async (int id, AppDbContext db, ILogger<RateEndpointsLogging> logger) =>
            {
                logger.LogInformation("GET /rate/{Id} çağrıldı.", id);
                var rate = await db.Rates.FindAsync(id);

                if (rate is Rates)
                {
                    logger.LogInformation("RateID {Id} için puan bulundu.", id);
                    return Results.Ok(rate);
                }
                else
                {
                    logger.LogWarning("RateID {Id} için puan bulunamadı.", id);
                    return Results.NotFound();
                }
            })
            .WithName("GetRateById");

            routes.MapPost("/rate", async (Rates rate, AppDbContext db, ILogger<RateEndpointsLogging> logger) =>
            {
                try
                {
                    logger.LogInformation("POST /rate çağrıldı. StaffID: {StaffID}, CustomerID: {CustomerID}, Rate: {Rate}",
                        rate.StaffID, rate.CustomerID, rate.Rate);

                    var existingRate = await db.Rates
                        .FirstOrDefaultAsync(r => r.StaffID == rate.StaffID && r.CustomerID == rate.CustomerID);

                    if (existingRate != null)
                    {
                        existingRate.Rate = rate.Rate;
                        db.Rates.Update(existingRate);
                        logger.LogInformation("Var olan puan güncellendi.");
                    }
                    else
                    {
                        await db.Rates.AddAsync(rate);
                        logger.LogInformation("Yeni puan eklendi.");
                    }

                    await db.SaveChangesAsync();

                    return Results.Ok(rate);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "POST /rate sırasında hata oluştu.");
                    return Results.Problem("Puan kaydedilirken hata oluştu.", statusCode: StatusCodes.Status500InternalServerError);
                }
            });


            routes.MapPut("/rate/{id}", async (int id, Rates updatedRate, AppDbContext db, ILogger<RateEndpointsLogging> logger) =>
            {
                logger.LogInformation("PUT /rate/{Id} çağrıldı. Güncellenecek Puan ID: {Id}, Yeni Puan Değeri: {NewRateValue}", id, updatedRate.Rate);
                var rateEntity = await db.Rates.FindAsync(id);

                if (rateEntity is null)
                {
                    logger.LogWarning("PUT /rate/{Id} - Güncellenecek puan bulunamadı. ID: {Id}", id);
                    return Results.NotFound();
                }
                if (updatedRate.Rate < 1 || updatedRate.Rate > 5)
                {
                    logger.LogWarning("PUT /rate/{Id} - Geçersiz yeni puan değeri: {NewRateValue}", id, updatedRate.Rate);
                    return Results.BadRequest("Puan değeri 1 ile 5 arasında olmalıdır.");
                }

                rateEntity.Rate = updatedRate.Rate;

                try
                {
                    await db.SaveChangesAsync();
                    logger.LogInformation("PUT /rate/{Id} - Puan başarıyla güncellendi. ID: {Id}", id);
                    return Results.NoContent();
                }
                catch (DbUpdateConcurrencyException dbConcEx)
                {
                    logger.LogError(dbConcEx, "PUT /rate/{Id} - Puan güncellenirken DbUpdateConcurrencyException oluştu. ID: {Id}", id);
                    return Results.Problem("Puan güncellenirken bir eş zamanlılık sorunu oluştu.", statusCode: StatusCodes.Status409Conflict);
                }
                catch (DbUpdateException dbEx)
                {
                    logger.LogError(dbEx, "PUT /rate/{Id} - Puan güncellenirken DbUpdateException oluştu. InnerException: {InnerExceptionMessage}. ID: {Id}", id, dbEx.InnerException?.Message ?? "N/A");
                    return Results.Problem($"Puan güncellenirken bir veritabanı hatası oluştu: {dbEx.InnerException?.Message ?? dbEx.Message}", statusCode: StatusCodes.Status500InternalServerError);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "PUT /rate/{Id} - Puan güncellenirken genel bir hata oluştu. ID: {Id}", id);
                    return Results.Problem("Puan güncellenirken sunucuda beklenmedik bir hata oluştu.", statusCode: StatusCodes.Status500InternalServerError);
                }
            })
            .WithName("UpdateRate");

            routes.MapDelete("/rate/{id}", async (int id, AppDbContext db, ILogger<RateEndpointsLogging> logger) =>
            {
                logger.LogInformation("DELETE /rate/{Id} çağrıldı.", id);
                if (await db.Rates.FindAsync(id) is Rates rateToDelete)
                {
                    db.Rates.Remove(rateToDelete);
                    try
                    {
                        await db.SaveChangesAsync();
                        logger.LogInformation("RateID {Id} başarıyla silindi.", id);
                        return Results.Ok(rateToDelete);
                    }
                    catch (DbUpdateException dbEx)
                    {
                        logger.LogError(dbEx, "DELETE /rate/{Id} - Puan silinirken DbUpdateException oluştu. InnerException: {InnerExceptionMessage}. ID: {Id}", id, dbEx.InnerException?.Message ?? "N/A");
                        return Results.Problem($"Puan silinirken bir veritabanı hatası oluştu: {dbEx.InnerException?.Message ?? dbEx.Message}", statusCode: StatusCodes.Status500InternalServerError);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "DELETE /rate/{Id} - Puan silinirken genel bir hata oluştu. ID: {Id}", id);
                        return Results.Problem("Puan silinirken sunucuda beklenmedik bir hata oluştu.", statusCode: StatusCodes.Status500InternalServerError);
                    }
                }
                logger.LogWarning("DELETE /rate/{Id} - Silinecek puan bulunamadı. ID: {Id}", id);
                return Results.NotFound();
            })
            .WithName("DeleteRate");

            routes.MapGet("/rate/employee/{employeeId}", async (int employeeId, AppDbContext db, ILogger<RateEndpointsLogging> logger) =>
            {
                logger.LogInformation("GET /rate/employee/{EmployeeId} çağrıldı.", employeeId);
                try
                {
                    var rates = await db.Rates
                        .Where(r => r.StaffID == employeeId)
                        .ToListAsync();
                    logger.LogInformation("{EmployeeId} ID'li personele ait {Count} adet puan bulundu.", employeeId, rates.Count);
                    return Results.Ok(rates);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "GET /rate/employee/{EmployeeId} sırasında bir hata oluştu.", employeeId);
                    return Results.Problem("Personele ait puanlar listelenirken bir sorun oluştu.", statusCode: StatusCodes.Status500InternalServerError);
                }
            })
            .WithName("GetRatesByEmployeeId");
        }
    }
}