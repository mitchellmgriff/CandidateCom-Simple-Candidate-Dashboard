using CandidateWebApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace CandidateWebApplication.Controllers
{
    public class CandidatesController : Controller
    {

        private readonly ApplicationDbContext _context;

        public CandidatesController(ApplicationDbContext context) { _context = context; }
        public IActionResult Index()
        {
            var list = _context.Candidates.ToList();
            return View(list);
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(Candidates model)
        {
            _context.Add(model);
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
