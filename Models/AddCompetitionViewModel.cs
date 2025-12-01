namespace Laboration_3.Models
{
    public class AddCompetitionViewModel
    {
        public int MemberId { get; set; }
        public int TournamentId { get; set; }
        public List<MemberDetails> Members { get; set; }
        public List<Tournament> Tournaments { get; set; }
    }
}
