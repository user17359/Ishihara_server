using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ishihara_leaderboard.Models;

namespace Ishihara_leaderboard.Data
{
    public class Ishihara_leaderboardContext : DbContext
    {
        public Ishihara_leaderboardContext (DbContextOptions<Ishihara_leaderboardContext> options)
            : base(options)
        {
        }

        public DbSet<Ishihara_leaderboard.Models.LeaderboardRecord> LeaderboardRecord { get; set; } = default!;
    }
}
