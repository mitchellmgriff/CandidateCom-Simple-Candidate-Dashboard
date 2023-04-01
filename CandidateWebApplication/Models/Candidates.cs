namespace CandidateWebApplication.Models
{
    public class Candidates
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string? Address { get; set; }
        public int? Salary { get; set; }
        public int? HourlyRate { get; set; }

        public string JobTitle { get; set; }
        public string? CompanyName { get; set; }

        public string? Linkedin { get; set; }


    }
}
