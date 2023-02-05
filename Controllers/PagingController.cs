using Microsoft.AspNetCore.Mvc;
using MyNotesApp.Data;
using MyNotesApp.Models;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;

namespace MyNotesApp.Controllers
{
    public class PagingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PagingController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int? pageIndex)
        {
            int pageSize = 3;
            var hoppy = from p in _context.Hoppies select p;
            var paginatedList = await PaginatedList<Hoppy>.CreateAsync((IQueryable<Hoppy>)hoppy, pageIndex ?? 1, pageSize);

            return View(paginatedList);
        }
    }
}
