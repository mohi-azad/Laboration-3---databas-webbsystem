using System.ComponentModel.DataAnnotations;
namespace Laboration_3.Models
{
    public class Tournament
    {
        public int TournamentId { get; set; }

        [Required(ErrorMessage = "Type of tournament is required!")]
        [StringLength(100)]
        public string Sort { get; set; }

        [Required(ErrorMessage = "Tournament date is required!")]
        [DataType(DataType.Date)]
        public DateTime T_date { get; set; }

        [Required(ErrorMessage = "Start time is required")]
        [DataType(DataType.Time)]
        public TimeSpan Start_time { get; set; }
    }
}




