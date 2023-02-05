using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyNotesApp.Data;
using MyNotesApp.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace MyNotesApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger,ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.note = new SelectList(_db.Notes,"Id","Title");
            TempData["id"] = userid;
            return View();
        }
        [HttpPost]
        public IActionResult Index(string[] gender, int[] note)
        {
            TempData["gender"] = gender;
            TempData["note"] = note;
            ViewBag.note = new SelectList(_db.Notes, "Id", "Title");
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
    }
}