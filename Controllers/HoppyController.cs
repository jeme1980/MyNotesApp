using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MyNotesApp.Core.IRepository;
using MyNotesApp.Data;
using MyNotesApp.Models;
using MyNotesApp.ViewModel;

namespace MyNotesApp.Controllers
{
    public class HoppyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public HoppyController(IUnitOfWork unitOfWork,ApplicationDbContext context,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _context = context;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            var model = _unitOfWork.Hoppies.GetAll();
            var vm = _mapper.Map<List<HoppyVM>>(model);
            return View(vm);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Hoppy model)
        {
            if (ModelState.IsValid)
            {
                _mapper.Map<Hoppy>(model);
                _unitOfWork.Hoppies.Cteate(model);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var model = _unitOfWork.Hoppies.GetBy(h=> h.Id == id);
            var vm = _mapper.Map<HoppyVM>(model);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(HoppyVM vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var model = _mapper.Map<Hoppy>(vm);
                    _unitOfWork.Hoppies.Update(model);
                    _unitOfWork.Save();
                    return RedirectToAction("Index");
                }
                return View();
            }
            catch (Exception)
            {
                return View();
            }
        }

        public IActionResult Delete(int id)
        {
            try
            {
                var vm = _unitOfWork.Hoppies.GetBy(h => h.Id == id);
                if (vm != null)
                {
                _unitOfWork.Hoppies.Delete(id);
                    _unitOfWork.Save();
                return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("Index");

            }
        }

        #region call_api
        public IActionResult GetAll()
        {
            var hoppylist = _unitOfWork.Hoppies.GetAll();
            return Json(new { data = hoppylist });
        }

        //POST
        [HttpDelete]
        public IActionResult Remove(int? id)
        {
            var obj = _unitOfWork.Hoppies.GetBy(u => u.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.Hoppies.Delete(id);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }
}
