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
        private readonly IWebHostEnvironment _env;
        public CandidatesController(IWebHostEnvironment env, ApplicationDbContext context,
            UserManager<IdentityUser> userManager = null)
        {
            _context = context; _userManager = userManager;
            _env = env;

        }
        [HttpPost]
        public IActionResult Search(string SearchString)
        {
            var connectedUser = _userManager.GetUserName(User);



            var query = _context.Candidates
         .Where(c => c.UserId == connectedUser &&
             (c.FirstName.Contains(SearchString) ||
              c.LastName.Contains(SearchString) ||
              c.JobTitle.Contains(SearchString) ||
              c.CompanyName.Contains(SearchString) ||
              c.PhoneNumber.Contains(SearchString) ||
              c.Email.Contains(SearchString)))
         .ToList();



            return View(query);
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
        public async Task<IActionResult> Add(Candidates model, IFormFile resume)
        {
            if (resume != null && resume.Length > 0)
            {
                var filePath = Path.Combine(_env.WebRootPath, "resumes", resume.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await resume.CopyToAsync(stream);
                }
                model.ResumeFileName = resume.FileName;
            }

            var currentUser = _userManager.GetUserName(User);
            model.UserId = currentUser;
            // Save the candidate to the database
            _context.Candidates.Add(model);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        public IActionResult View(int id)
        {
            var currentUser = _userManager.GetUserName(User);
            var list = _context.Candidates.Where(c => c.UserId == currentUser);
            var candidate = _context.Candidates.Find(id);

            if (candidate == null)
            {
                // Handle case where candidate is not found
                return NotFound();
            }
            if (currentUser == candidate.UserId)
            {
                return View(candidate);
            }
            else
            {
                return NotFound();
            }
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
