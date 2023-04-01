using CandidateWebApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CandidateWebApplication.Controllers
{
    [Authorize]
    public class CandidatesController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CandidatesController(ApplicationDbContext context, UserManager<IdentityUser> userManager = null)
        {
            _context = context; _userManager = userManager;

        }

        public IActionResult Index()
        {
            var currentUser = _userManager.GetUserName(User);
            var list = _context.Candidates.Where(c => c.UserId == currentUser).ToList();
            return View(list);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Candidates model)
        {
            var owner = _userManager.GetUserName(User);
            model.UserId = owner;
            _context.Candidates.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Index");



        }
        public IActionResult View(int id)
        {

            var candidate = _context.Candidates.Find(id);

            if (candidate == null)
            {
                // Handle case where candidate is not found
                return NotFound();
            }

            return View(candidate);
        }
        public IActionResult Edit(int id)
        {
            var candidate = _context.Candidates.Find(id);

            if (candidate == null)
            {
                // Handle case where candidate is not found
                return NotFound();
            }

            return View(candidate);
        }

        [HttpPost]
        public IActionResult Edit(Candidates candidate)
        {
            var existingCandidate = _context.Candidates.Find(candidate.Id);

            if (existingCandidate == null)
            {
                return NotFound();
            }

            existingCandidate.FirstName = candidate.FirstName;
            existingCandidate.LastName = candidate.LastName;
            existingCandidate.JobTitle = candidate.JobTitle;
            existingCandidate.CompanyName = candidate.CompanyName;
            existingCandidate.PhoneNumber = candidate.PhoneNumber;
            existingCandidate.Email = candidate.Email;
            existingCandidate.Address = candidate.Address;
            existingCandidate.Linkedin = candidate.Linkedin;
            existingCandidate.HourlyRate = candidate.HourlyRate;
            existingCandidate.Salary = candidate.Salary;

            _context.Update(existingCandidate);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int[] candidateIds)
        {

            if (candidateIds == null || candidateIds.Length == 0)
            {
                // Handle case where no candidates were selected
                return RedirectToAction("Index");
            }

            foreach (int id in candidateIds)
            {
                var candidate = _context.Candidates.Find(id);
                if (candidate != null)
                {
                    _context.Candidates.Remove(candidate);
                }
            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}
