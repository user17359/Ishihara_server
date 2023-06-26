using System.ComponentModel.DataAnnotations;
namespace Ishihara_leaderboard.Models
{
    public class LeaderboardRecord
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Range(0.01, 100.00)]
        public decimal Score { get; set; }
    }
}
