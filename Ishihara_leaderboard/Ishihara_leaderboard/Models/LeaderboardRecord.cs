using System.ComponentModel.DataAnnotations;
namespace Ishihara_leaderboard.Models
{
    public class LeaderboardRecord
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(16)]
        public string? Name { get; set; }

        [Range(0.00, 100.00)]
        public decimal Score { get; set; }
    }
}
