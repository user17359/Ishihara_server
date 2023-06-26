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
            var result = await db.LeaderboardRecord.ToListAsync();
            var sortedResult = result.OrderByDescending(o => o.Score).ToList();
            return sortedResult;
        })
        .WithName("GetAllLeaderboardRecords")
        .Produces<List<LeaderboardRecord>>(StatusCodes.Status200OK);

        //post
        routes.MapPost("/api/LeaderboardRecord/", async (LeaderboardRecord leaderboardRecord, Ishihara_leaderboardContext db) =>
        {
            var result = await db.LeaderboardRecord.ToListAsync();
            var sortedResult = result.OrderByDescending(o => o.Score).ToList();
            if(sortedResult.Count < maxSize || leaderboardRecord.Score > sortedResult.Last().Score)
            {
                bool doesNameApear = false;
                foreach(LeaderboardRecord position in sortedResult)
                {
                    if(position.Name == leaderboardRecord.Name)
                    {
                        doesNameApear = true; break;
                    }
                }
                if(!doesNameApear)
                {
                    if(sortedResult.Count >= maxSize)
                    {
                        db.LeaderboardRecord.Remove(sortedResult.Last());
                    }
                    db.LeaderboardRecord.Add(leaderboardRecord);
                }
                else
                {
                    //TODO change existing record
                }
                await db.SaveChangesAsync();
                return Results.Created($"/LeaderboardRecords/{leaderboardRecord.Id}", leaderboardRecord);
            }
            else
            {
                return Results.Created($"Not fitting in the leaderboard", leaderboardRecord);
            }
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
