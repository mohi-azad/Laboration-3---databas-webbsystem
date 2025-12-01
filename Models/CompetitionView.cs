namespace Laboration_3.Models
{
    public class CompetitionView
    {
        public int MemberId { get; set; }   
        public int TournamentId { get; set; }
        public string MemberName { get; set; }
        public string Sort { get; set; }
        public DateTime T_date { get; set; }
        public TimeSpan Start_time { get; set; }
    }
}
