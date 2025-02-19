using AliciasWebDisplay.Models;
using AliciasWebDisplay.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace AliciasWebDisplay.Controllers
{
    [Route("Admin")]
    public class AdminController : Controller
    {
        private readonly UserDbContext _context;

        public AdminController(UserDbContext context)
        {
            this._context = context;
        }

        [HttpGet] // Explicitly set method type
        [Route("Index")]
        public IActionResult Index(string searchString)
        {
            var users = _context.TUsers.ToList();
            if (!string.IsNullOrEmpty(searchString))
            {
                users = users.Where(u => u.UserCode.ToString().Contains(searchString)).ToList();
            }
            return View(users);
        }

        [HttpGet]
        [Route("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult Create(Users users)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", users);
            }

            // Check if the UserCode already exists
            var existingUser = _context.TUsers.FirstOrDefault(u => u.UserCode == users.UserCode);
            if (existingUser != null)
            {
                ModelState.AddModelError("UserCode", "This User Code is already in use. Please choose a different one.");
                return View("Create", users);
            }

            // Ensure AccessCode matches and do not set the UserCode explicitly
            // Optionally, you could check if users.AccessCode matches a ConfirmAccessCode if you want, but since you don't have that field, skip it.

            try
            {
                // Ensure the UserCode is not set manually; let the DB handle it
                users.UserCode = 0;  // You can explicitly set it to 0 to make sure the DB will generate it.

                _context.TUsers.Add(users);
                _context.SaveChanges();
                TempData["AlertMessage"] = $"User {users.Name} has been CREATED.";
            }
            catch (Exception ex)
            {
                TempData["AlertMessage"] = $"Error creating admin: {ex.Message}";

                // Log the inner exception for better debugging
                if (ex.InnerException != null)
                {
                    TempData["AlertMessage"] += $" Inner Exception: {ex.InnerException.Message}";
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("Edit/{UserCode}")]
        public IActionResult Edit(int UserCode)
        {
            var users = _context.TUsers.FirstOrDefault(c => c.UserCode == UserCode);
            if (users == null)
            {
                TempData["AlertMessage"] = $"Admin with ID {UserCode} not found!";
                return RedirectToAction("Index");
            }
            return View(users);
        }

        [HttpPost]
        [Route("Edit/{UserCode}")]  // Update the route to match the GET method's route
        public IActionResult Edit(int UserCode, Users users)
        {
            if (!ModelState.IsValid)
            {
                return View(users);
            }

            var existingUser = _context.TUsers.FirstOrDefault(c => c.UserCode == UserCode);
            if (existingUser == null)
            {
                TempData["AlertMessage"] = "Admin not found!";
                return RedirectToAction("Index");
            }

            // Do not change the UserCode, only update the rest of the fields
            existingUser.Name = users.Name;
            existingUser.Admin = users.Admin;
            existingUser.Active = users.Active;

            // Only update the AccessCode if it's provided (and not null or empty)
            if (!string.IsNullOrWhiteSpace(users.AccessCode))
            {
                existingUser.AccessCode = users.AccessCode;
            }

            // Save changes to the database
            _context.SaveChanges();
            TempData["AlertMessage"] = $"User {users.Name} has been UPDATED.";

            return RedirectToAction("Index");
        }


        [HttpPost]
        [Route("Delete/{UserCode}")]
        public IActionResult Delete(int UserCode)
        {
            var users = _context.TUsers.FirstOrDefault(c => c.UserCode == UserCode);
            if (users == null)
            {
                TempData["AlertMessage"] = $"Admin with ID {UserCode} not found!";
                return RedirectToAction("Index");
            }

            _context.TUsers.Remove(users);
            _context.SaveChanges();
            TempData["AlertMessage"] = $"Admin {users.Name} has been DELETED.";

            return RedirectToAction("Index");
        }
    }
}
