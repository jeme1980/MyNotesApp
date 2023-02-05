using Microsoft.AspNetCore.Mvc;
using MyNotesApp.Core.IRepository;

namespace MyNotesApp.Controllers
{
    public class AjaxController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AjaxController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetHoppList() 
        {
            var hoppyList = _unitOfWork.Hoppies.GetAll();
            return Json(hoppyList);
        }
    }
}
