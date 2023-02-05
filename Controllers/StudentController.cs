using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyNotesApp.Data;
using MyNotesApp.Models;
using MyNotesApp.Utilites;
using MyNotesApp.ViewModel;
using GemBox.Spreadsheet;
using System.Linq;

namespace MyNotesApp.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var students = _context.Students.Include(h => h.studentHoppies).ThenInclude(s => s.Hoppy).AsNoTracking().ToList();
            return View(students);
        }

        [HttpGet]
        public IActionResult Create()
        {
            Student student = new Student();

            List<SelectListItem> list = new List<SelectListItem>();
            var hoppylist = _context.Hoppies.ToList();
            foreach (var item in hoppylist)
            {
                SelectListItem element = new SelectListItem {Text = item.Name, Value=item.Id.ToString() };
                list.Add(element);
            }
            student.SH = list;
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Student student)
        {

            if (ModelState.IsValid)
            {
                student.SH = student.SH.Where(h => h.Selected == true).ToList();
                _context.Students.Add(student);
                _context.SaveChanges();
                List<StudentHoppy> studentHoppies= new List<StudentHoppy>();
                //foreach (var item in student.SH)
                //{
                //    studentHoppies.Add(new StudentHoppy { HoppyId= Convert.ToInt32(item.Value),StudentId = student.StudentId});
                //}
                student.SH.ForEach(x => { studentHoppies.Add(new StudentHoppy() { StudentId = student.StudentId, HoppyId = Convert.ToInt32(x.Value) }); });
                _context.StudentHoppies.AddRange(studentHoppies);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        [HttpGet]
        public IActionResult EditStudent(int id)
        {
            Student student = _context.Students.Find(id);
            List<SelectListItem> list = new List<SelectListItem>();
            var hoppylist = _context.Hoppies.ToList();
            foreach (var item in hoppylist)
            {
                SelectListItem element = new SelectListItem { 
                    Text = item.Name, 
                    Value = item.Id.ToString(),
                    Selected = _context.StudentHoppies.Where(x => x.StudentId == id && x.HoppyId == item.Id).Count() > 0 ? true : false
                };
                list.Add(element);
            }
            student.SH = list;
            return View(student);
        }
        [HttpPost]
        public IActionResult EditStudent(Student student)
        {
            if (!ModelState.IsValid) return View(student);
            student.SH = student.SH.Where(h => h.Selected == true).ToList();
            _context.Students.Update(student);
            _context.SaveChanges();

            var list = _context.StudentHoppies.Where(x => x.StudentId == student.StudentId).ToList();
            _context.StudentHoppies.RemoveRange(list);
            _context.SaveChanges();

            List<StudentHoppy> studentHoppies = new List<StudentHoppy>();
            student.SH.ForEach(x => { studentHoppies.Add(new StudentHoppy() { StudentId = student.StudentId, HoppyId = Convert.ToInt32(x.Value) }); });
            _context.StudentHoppies.AddRange(studentHoppies);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
            [HttpGet]
        public IActionResult Edit(int? id) 
        {
            var item = _context.Students.Include(h => h.studentHoppies).ThenInclude(s => s.Hoppy).FirstOrDefault(x => x.StudentId == id);
            if (item == null)
                return NotFound();
            
            CreateStudentVM vm = new CreateStudentVM()
            {
                StudentId = item.StudentId,
                StudentFirstName = item.StudentFirstName,
                StudentLasttName = item.StudentLasttName,
                HoppyChBoxVMs = _context.Hoppies.Select(h => new HoppyChBoxVM
                {
                    Id = h.Id,
                    Name = h.Name,
                    IsCheaked = h.studentHoppies.Any(x => x.StudentId == item.StudentId) ? true : false
                }).ToList()
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CreateStudentVM vm)
        {
            List<StudentHoppy> lstSH = new List<StudentHoppy>();
            Student student = _context.Students.Where(x => x.StudentId == vm.StudentId).Include(h => h.studentHoppies).ThenInclude(s => s.Hoppy).FirstOrDefault();
            if (student == null)
                return NotFound();
            foreach (var item in student.studentHoppies)
            {
                _context.StudentHoppies.Remove(item);
            }
            await _context.SaveChangesAsync();

            if (ModelState.IsValid)
            {
                student.StudentLasttName = vm.StudentLasttName;
                student.StudentFirstName = vm.StudentFirstName;
                _context.Students.Update(student);
                await _context.SaveChangesAsync();
                foreach (var item in vm.HoppyChBoxVMs)
                {
                    if (item.IsCheaked)
                    {
                        lstSH.Add(new StudentHoppy { StudentId = student.StudentId, HoppyId = item.Id });
                    };
                };
                await _context.StudentHoppies.AddRangeAsync(lstSH);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
          
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            Student student = _context.Students.Where(x => x.StudentId == id).Include(h => h.studentHoppies).ThenInclude(s => s.Hoppy).FirstOrDefault();
            if (student == null)
                return NotFound();
            foreach (var item in student.studentHoppies)
            {
                _context.StudentHoppies.Remove(item);
            }
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        } 
    }
}
