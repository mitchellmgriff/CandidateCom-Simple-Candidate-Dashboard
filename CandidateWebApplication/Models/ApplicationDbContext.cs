using Microsoft.EntityFrameworkCore;

namespace CandidateWebApplication.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Candidates> Candidates { get; set; }

    }
}
