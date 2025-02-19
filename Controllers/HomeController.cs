using AliciasWebDisplay.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace AliciasWebDisplay.Controllers
{
    [CustomAuthorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration, ILogger<HomeController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewData["UserId"] = HttpContext.Session.GetString("UserId"); // Using Session-based authentication
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult ChangeDatabase(DatabaseSelectionViewModel model)
        {
            string selectedDatabase = model.SelectedDatabase;
            string connectionString;

            switch (selectedDatabase)
            {
                case "AliciasLapaz":
                    connectionString = _configuration.GetConnectionString("SecondDatabaseConnection");
                    break;
                case "AliciasVilla":
                    connectionString = _configuration.GetConnectionString("ThirdDatabaseConnection");
                    break;
                default:
                    connectionString = _configuration.GetConnectionString("DefaultConnection");
                    break;
            }

            HttpContext.Session.SetString("CurrentConnectionString", connectionString);
            return RedirectToAction("Index");
        }
    }
}
