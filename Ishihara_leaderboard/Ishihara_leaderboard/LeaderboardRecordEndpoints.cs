using Microsoft.EntityFrameworkCore;
using Ishihara_leaderboard.Data;
using Ishihara_leaderboard.Models;
namespace Ishihara_leaderboard;

public static class LeaderboardRecordEndpoints
{
    public static void MapLeaderboardRecordEndpoints (this IEndpointRouteBuilder routes)
    {
        int maxSize = 10;

        //get
        routes.MapGet("/api/LeaderboardRecord", async (Ishihara_leaderboardContext db) =>
        {
            var result = await db.LeaderboardRecord.OrderByDescending(o => o.Score).Take(maxSize).ToListAsync();
            return result;
        })
        .WithName("GetAllLeaderboardRecords")
        .Produces<List<LeaderboardRecord>>(StatusCodes.Status200OK);

        //post
        routes.MapPost("/api/LeaderboardRecord/", async (LeaderboardRecord leaderboardRecord, Ishihara_leaderboardContext db) =>
        {
            var record = await db.LeaderboardRecord.Where(o => o.Name == leaderboardRecord.Name).FirstAsync();
            if (record == null)
            {
                db.LeaderboardRecord.Add(leaderboardRecord);
                record = leaderboardRecord;
            }
            else if (record.Score < leaderboardRecord.Score)
            {
                record.Score = leaderboardRecord.Score;
                db.LeaderboardRecord.Update(record);
            }
            await db.SaveChangesAsync();
            return Results.Created($"/LeaderboardRecords/{record.Id}", record);
        })
        .WithName("CreateLeaderboardRecord")
        .Produces<LeaderboardRecord>(StatusCodes.Status201Created);

        //delete
        routes.MapDelete("/api/LeaderboardRecord/{id}", async (int id, Ishihara_leaderboardContext db) =>
        {
            //TODO require key to delete
            if (await db.LeaderboardRecord.FindAsync(id) is LeaderboardRecord leaderboardRecord)
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
