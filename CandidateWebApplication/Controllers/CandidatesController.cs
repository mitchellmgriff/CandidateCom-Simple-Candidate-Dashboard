using CandidateWebApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CandidateWebApplication.Controllers
{
    // The [Authorize] attribute restricts access to the controller and its actions to authenticated users only.

    [Authorize]
    public class CandidatesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _env;

        // Constructor injection for the required services and environment
        public CandidatesController(IWebHostEnvironment env, ApplicationDbContext context, UserManager<IdentityUser> userManager = null)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }

        // Action to perform a search based on the provided search string
        [HttpPost]
        public IActionResult Search(string SearchString)
        {
            // Retrieve the username of the currently logged-in user
            var connectedUser = _userManager.GetUserName(User);

            // Perform a search based on the provided search string
            var query = _context.Candidates
                .Where(c => c.UserId == connectedUser &&
                    (c.FirstName.Contains(SearchString) ||
                    c.LastName.Contains(SearchString) ||
                    c.JobTitle.Contains(SearchString) ||
                    c.CompanyName.Contains(SearchString) ||
                    c.PhoneNumber.Contains(SearchString) ||
                    c.Email.Contains(SearchString) || c.Id.Equals(SearchString)))
                .ToList();

            return View(query);
        }

        // Action to display the list of candidates associated with the current user
        public IActionResult Index()
        {
            // Retrieve the username of the currently logged-in user
            var currentUser = _userManager.GetUserName(User);

            // Retrieve a list of candidates associated with the current user
            var list = _context.Candidates.Where(c => c.UserId == currentUser).ToList();

            return View(list);
        }

        // Action to display the form for adding a new candidate
        public IActionResult Add()
        {
            return View();
        }

        // Action to handle the form submission for adding a new candidate
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

            // Retrieve the username of the currently logged-in user
            var currentUser = _userManager.GetUserName(User);

            model.UserId = currentUser;

            // Save the candidate to the database
            _context.Candidates.Add(model);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // Action to view the details of a specific candidate
        public IActionResult View(int id)
        {
            // Retrieve the username of the currently logged-in user
            var currentUser = _userManager.GetUserName(User);

            // Retrieve a list of candidates associated with the current user
            var list = _context.Candidates.Where(c => c.UserId == currentUser);

            // Find the candidate with the specified ID
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

        // Action to display the form for editing a candidate's details
        public IActionResult Edit(int id)
        {
            // Find the candidate with the specified ID
            var candidate = _context.Candidates.Find(id);

            if (candidate == null)
            {
                // Handle case where candidate is not found
                return NotFound();
            }

            return View(candidate);
        }

        // Action to handle the form submission for editing a candidate's details
        [HttpPost]
        public IActionResult Edit(Candidates candidate)
        {
            var existingCandidate = _context.Candidates.Find(candidate.Id);

            if (existingCandidate == null)
            {
                return NotFound();
            }

            // Update the candidate's details
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

        // Action to handle the deletion of a candidate
        [HttpPost]
        public IActionResult Delete(int candidateId)
        {
            // Find the candidate with the specified ID
            var candidate = _context.Candidates.Find(candidateId);

            if (candidate == null)
            {
                // Handle case where candidate is not found
                return NotFound();
            }

            if (candidate != null)
            {
                // Remove the candidate from the database
                _context.Candidates.Remove(candidate);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }



}




