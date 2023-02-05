using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyNotesApp.Core.IRepository;
using MyNotesApp.Data;
using MyNotesApp.Models;
using MyNotesApp.ViewModel;
using System.Security.Claims;

namespace MyNotesApp.Controllers
{
    [Authorize(Roles = "Author,Admin")]
    public class NoteController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NoteController(ApplicationDbContext context, 
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult Index()
        {
            //var userid = _userManager.GetUserId(HttpContext.User);
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var notes = _unitOfWork.Notes.GetAllBy(u => u.UserId == claim.Value);
            return View(notes);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new NoteVM());
        }

        [HttpPost]
        public IActionResult Create(NoteVM model)
        {
            if (ModelState.IsValid)
            {
                var userid = _userManager.GetUserId(HttpContext.User);
                //var note = new Note()
                //{
                //    Title = model.Title,
                //    Description = model.Description,
                //    Color = model.Color,
                //    UserId = userid,
                //};
                model.UserId = userid;
                var note = _mapper.Map<Note>(model);
                _unitOfWork.Notes.Cteate(note);
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            var userid = _userManager?.GetUserId(HttpContext.User);
            var note = _unitOfWork.Notes.GetBy(n => n.Id == id);
            if (note != null && note?.UserId == userid)
            {
                var notevm = new NoteVM()
                {
                    Id = note.Id,
                    UserId = userid,
                    Color = note.Color,
                    CreatedDate = note.CreatedDate,
                    Description = note.Description,
                    Title = note.Title
                };
                return View(notevm);
            }
            return Content("אתה לא רשום");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(NoteVM model)
        {
            if (ModelState.IsValid)
            {
                var userid = _userManager.GetUserId(HttpContext.User);
                //if (model.UserId == userid)
                //{
                //    var note = new Note()
                //    {
                //        UserId = userid,
                //        Color = model.Color,
                //        CreatedDate = model.CreatedDate,
                //        Description = model.Description,
                //        Title = model.Title,
                //        Id = model.Id
                //    };
                if (model.UserId == userid)
                {
                    var note = _mapper.Map<Note>(model);
                    _unitOfWork.Notes.Update(note);
                    _unitOfWork.Save();
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }


        public IActionResult Delete(int? id)
        {
            var userid = _userManager?.GetUserId(HttpContext.User);
            var note = _unitOfWork.Notes.GetBy(n => n.Id == id);
            if (note != null && note?.UserId == userid)
            {
                _unitOfWork.Notes.Delete(id);
                _unitOfWork.Save();
            }
            return RedirectToAction("Index");
        }
    }
}
