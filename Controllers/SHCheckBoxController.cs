using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CodeStyle;
using Microsoft.EntityFrameworkCore;
using MyNotesApp.Data;
using MyNotesApp.Models;
using MyNotesApp.ViewModel;
using NToastNotify;

namespace MyNotesApp.Controllers
{
    public class SHCheckBoxController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IToastNotification _toastNotification;

        public SHCheckBoxController(ApplicationDbContext db, IToastNotification toastNotification)
        {
            _db = db;
            _toastNotification = toastNotification;
        }
        public IActionResult Index()
        {
            var studentlist = _db.Students.Include(sh => sh.studentHoppies).ThenInclude(h => h.Hoppy).ToList();
            return View(studentlist);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var item = _db.Hoppies.ToList();
            CreateStudentVM vm = new CreateStudentVM();
            vm.HoppyChBoxVMs = item.Select(vm => new HoppyChBoxVM()
            {
                Id = vm.Id,
                Name = vm.Name,
                IsCheaked = false
            }).ToList();
            return View(vm);
        }
        [HttpPost]
        public IActionResult Create(CreateStudentVM model)
        {
            model.HoppyChBoxVMs = model.HoppyChBoxVMs.Where(h => h.IsCheaked == true).ToList();
            Student student = new Student()
            {
                StudentFirstName = model.StudentFirstName,
                StudentLasttName = model.StudentLasttName
            };
            _db.Students.Add(student);
            _db.SaveChanges();

            List<StudentHoppy> studentHoppies = new List<StudentHoppy>();
            //model.HoppyChBoxVMs.ForEach(x => { studentHoppies.Add(new StudentHoppy() { StudentId = student.StudentId, HoppyId = Convert.ToInt32(x.Id) }); });
            foreach (var item in model.HoppyChBoxVMs)
            {
                _db.StudentHoppies.Add(new StudentHoppy() { StudentId = student.StudentId, HoppyId = item.Id });
            }
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Student student = _db.Students.Where(s => s.StudentId == id).Include(sh => sh.studentHoppies).ThenInclude(h => h.Hoppy).FirstOrDefault();
            if (id == null)
            {
                return NotFound();
            }
            var allhoppy = _db.Hoppies.Select(vm => new HoppyChBoxVM()
            {
                Id = vm.Id,
                Name = vm.Name,
                IsCheaked = vm.studentHoppies.Any(s => s.StudentId == student.StudentId) ? true : false
            }).ToList();
            CreateStudentVM model = new CreateStudentVM()
            {
                StudentId = student.StudentId,
                StudentFirstName = student.StudentFirstName,
                StudentLasttName = student.StudentLasttName,
                HoppyChBoxVMs = allhoppy
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult Edit(CreateStudentVM model)
        {
            model.HoppyChBoxVMs = model.HoppyChBoxVMs.Where(h => h.IsCheaked == true).ToList();
            Student student = _db.Students.Where(s => s.StudentId == model.StudentId).Include(sh => sh.studentHoppies).ThenInclude(h => h.Hoppy).FirstOrDefault();
            student.StudentFirstName = model.StudentFirstName;
            student.StudentLasttName = model.StudentLasttName;
            _db.Students.Update(student);
            _db.SaveChanges();

            foreach (var item in student.studentHoppies)
            {
                    _db.StudentHoppies.Remove(item);
            }
            _db.SaveChanges();
            List<StudentHoppy> studentHoppies = new List<StudentHoppy>();
            foreach (var item in model.HoppyChBoxVMs)
            {
                _db.StudentHoppies.Add(new StudentHoppy() { StudentId = student.StudentId, HoppyId = item.Id });
            }
            _db.SaveChanges();
            _toastNotification.AddSuccessToastMessage("הנתונים נשמרו בהצלחה",new ToastrOptions() { Title = "נשמר בהצלחה"});
            return RedirectToAction("Index");
        }
    }
}
