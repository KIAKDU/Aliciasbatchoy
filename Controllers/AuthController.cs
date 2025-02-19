using Microsoft.AspNetCore.Mvc;
using AliciasWebDisplay.Data;
using AliciasWebDisplay.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;
using AliciasWebDisplay.Services;

public class AuthController : Controller
{
    private readonly UserDbContext _context;
    private readonly LapazUserDbContext _Lapazcontext;
    private readonly VillaUserDbContext _Villacontext;

    public AuthController(UserDbContext context, LapazUserDbContext Lapazcontext, VillaUserDbContext Villacontext)
    {
        _context = context;
        _Lapazcontext = Lapazcontext;
        _Villacontext = Villacontext;
    }

    public IActionResult Login()
    {
        // Check if "UserCode" cookie exists and automatically log in the user
        var userCode = Request.Cookies["UserCode"];
        if (userCode != null)
        {
            // Fetch the user from your database using the userCode stored in the cookie
            var user = _context.TUsers.FirstOrDefault(u => u.UserCode.ToString() == userCode)
                        ?? _Lapazcontext.TUsers.FirstOrDefault(u => u.UserCode.ToString() == userCode)
                        ?? _Villacontext.TUsers.FirstOrDefault(u => u.UserCode.ToString() == userCode);

            if (user != null)
            {
                // If the user exists, store session data and redirect to the home page
                HttpContext.Session.SetString("UserCode", user.UserCode.ToString());
                HttpContext.Session.SetString("UserName", user.Name);
                HttpContext.Session.SetString("IsAdmin", user.Admin.ToString());  // Set admin session info
                return RedirectToAction("Index", "Home");
            }
        }
        return View();
    }

    [HttpPost]
    public IActionResult Login(string userCode, string accessCode, bool rememberMe)
    {
        if (!int.TryParse(userCode, out int userCodeInt))
        {
            ViewBag.Error = "Invalid User Code format!";
            return View();
        }

        var user = _context.TUsers.FirstOrDefault(u => u.UserCode == userCodeInt && u.AccessCode == accessCode && u.Active)
                    ?? _Lapazcontext.TUsers.FirstOrDefault(u => u.UserCode == userCodeInt && u.AccessCode == accessCode && u.Active)
                    ?? _Villacontext.TUsers.FirstOrDefault(u => u.UserCode == userCodeInt && u.AccessCode == accessCode && u.Active);

        if (user == null)
        {
            ViewBag.Error = "Invalid login credentials!";
            return View();
        }

        // Debugging: Log the user.Admin value to ensure it’s being set correctly
        Console.WriteLine($"User Admin Status: {user.Admin}");

        // Store user information in session
        HttpContext.Session.SetString("UserCode", user.UserCode.ToString());
        HttpContext.Session.SetString("UserName", user.Name);
        HttpContext.Session.SetString("IsAdmin", user.Admin.ToString());  // Ensure user.Admin is true/false or 1/0

        // Handle Remember Me - Use Cookie for Persistent Login
        if (rememberMe)
        {
            Response.Cookies.Append("UserCode", user.UserCode.ToString(), new CookieOptions
            {
                Expires = DateTime.Now.AddDays(7),  // Cookie expiration
                HttpOnly = true,                    // Only accessible via HTTP requests
                Secure = true                        // Ensures cookie is sent over HTTPS
            });
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear(); // Clears the session, including IsAdmin

        // Remove Remember Me Cookie
        if (Request.Cookies["UserCode"] != null)
        {
            Response.Cookies.Delete("UserCode");
        }

        return RedirectToAction("Login", "Auth");
    }

    public IActionResult Index()
    {
        // Check if "IsAdmin" is set in session, or if user is admin from cookie
        var isAdmin = HttpContext.Session.GetString("IsAdmin") == "True";  // Check session value directly
        ViewBag.IsAdmin = isAdmin;
        return View();
    }
}
