namespace CandidateWebApplication.Models
{
    public class Notes
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserId { get; set; }
        public Candidates Candidate { get; set; }
    }
}
