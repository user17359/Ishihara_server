using Microsoft.EntityFrameworkCore;
using Ishihara_leaderboard.Data;
using Ishihara_leaderboard.Models;
namespace Ishihara_leaderboard;

public static class LeaderboardRecordEndpoints
{
    public static void MapLeaderboardRecordEndpoints (this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/LeaderboardRecord", async (Ishihara_leaderboardContext db) =>
        {
            return await db.LeaderboardRecord.ToListAsync();
        })
        .WithName("GetAllLeaderboardRecords")
        .Produces<List<LeaderboardRecord>>(StatusCodes.Status200OK);

        routes.MapGet("/api/LeaderboardRecord/{id}", async (int Id, Ishihara_leaderboardContext db) =>
        {
            return await db.LeaderboardRecord.FindAsync(Id)
                is LeaderboardRecord model
                    ? Results.Ok(model)
                    : Results.NotFound();
        })
        .WithName("GetLeaderboardRecordById")
        .Produces<LeaderboardRecord>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        routes.MapPut("/api/LeaderboardRecord/{id}", async (int Id, LeaderboardRecord leaderboardRecord, Ishihara_leaderboardContext db) =>
        {
            var foundModel = await db.LeaderboardRecord.FindAsync(Id);

            if (foundModel is null)
            {
                return Results.NotFound();
            }

            db.Update(leaderboardRecord);

            await db.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithName("UpdateLeaderboardRecord")
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status204NoContent);

        routes.MapPost("/api/LeaderboardRecord/", async (LeaderboardRecord leaderboardRecord, Ishihara_leaderboardContext db) =>
        {
            db.LeaderboardRecord.Add(leaderboardRecord);
            await db.SaveChangesAsync();
            return Results.Created($"/LeaderboardRecords/{leaderboardRecord.Id}", leaderboardRecord);
        })
        .WithName("CreateLeaderboardRecord")
        .Produces<LeaderboardRecord>(StatusCodes.Status201Created);

        routes.MapDelete("/api/LeaderboardRecord/{id}", async (int Id, Ishihara_leaderboardContext db) =>
        {
            if (await db.LeaderboardRecord.FindAsync(Id) is LeaderboardRecord leaderboardRecord)
            {
                db.LeaderboardRecord.Remove(leaderboardRecord);
                await db.SaveChangesAsync();
                return Results.Ok(leaderboardRecord);
            }

            return Results.NotFound();
        })
        .WithName("DeleteLeaderboardRecord")
        .Produces<LeaderboardRecord>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}
