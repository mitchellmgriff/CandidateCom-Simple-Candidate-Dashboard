using CandidateWebApplication.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CandidateWebApplication.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public ReportsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult JobReport(string jobTitle = null, string currentCompany = null, int? salary = null, int? hourly = null)
        {
            var currentUser = _userManager.GetUserName(User);
            var query = _context.Candidates.Where(c => c.UserId == currentUser);

            if (!string.IsNullOrEmpty(jobTitle))
            {
                query = query.Where(c => c.JobTitle == jobTitle);
            }
            if (!string.IsNullOrEmpty(currentCompany))
            {
                query = query.Where(c => c.CompanyName == currentCompany);
            }

            if (salary.HasValue)
            {
                query = query.Where(c => c.Salary <= salary.Value);
            }

            if (hourly.HasValue)
            {
                query = query.Where(c => c.HourlyRate <= hourly.Value);
            }

            var list = query.ToList();
            return View(list);
        }


    }
}
