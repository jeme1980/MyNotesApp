using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyNotesApp.Core.IRepository;
using MyNotesApp.Core.Repository;
using MyNotesApp.Data;
using MyNotesApp.Models;
using MyNotesApp.ViewModel;
using System.Data;

namespace MyNotesApp.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileUploadService _fileUploadService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ApplicationDbContext context,
            IWebHostEnvironment webHostEnvironment,
            IFileUploadService fileUploadService,
            RoleManager<IdentityRole> roleManager,
            IUnitOfWork unitOfWork
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _fileUploadService = fileUploadService;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
        }
      


        public IActionResult CreateUser()
        {
            ViewBag.GenderId = new SelectList(_unitOfWork.Genders.GetAll(),"Id","Name");
            //ViewBag.HoppyId = new SelectList(_unitOfWork.Hoppies.GetAll(),"Id","Name");
            NewAppUser newAppUser = new NewAppUser();
            newAppUser.HoppyChBoxVMs = _context.Hoppies.Select(vm => new HoppyChBoxVM()
            {
                Id = vm.Id,
                Name = vm.Name,
                IsCheaked = false
            }).ToList();
            return View(newAppUser);
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(NewAppUser model)
        {
            if (model.file != null)
            {
                model.ImageFile = await _fileUploadService.UploadImage(model.file, ConstHelper.ConstHelper.UploadUserDiractory);
            }
            AppUser appUser = new AppUser()
            {
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                GenderId = model.GenderId,
                ImageFile = model.ImageFile
            };
            var result = await _userManager.CreateAsync(appUser, model.Password);
            if (result.Succeeded)
            {
                foreach (var hoppy in model.HoppyChBoxVMs)
                {
                    if (hoppy.IsCheaked == true)
                    {
                        var hu = new HoppyUser() { AppUserId = appUser.Id, HoppyId = hoppy.Id };
                        _context.HoppyUsers.Add(hu);
                    }
                }
                _context.SaveChanges();
                await _userManager.AddToRoleAsync(appUser, Utilites.WebsiteRoles.WebsiteAuthor);
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult UpdateUser(string? id)
        {
            if (id == null) return NotFound();
            ViewBag.GenderId = new SelectList(_unitOfWork.Genders.GetAll(), "Id", "Name");
            AppUser? appUser = _context?.AppUsers?.Include(g => g.Gender).Include(uh => uh.HoppyUser).ThenInclude(h => h.Hoppy).FirstOrDefault(u => u.Id == id);
            if (appUser == null) return NotFound();
            
            var allHoppy = _unitOfWork.Hoppies.GetAll();
            var userhoopy = new HashSet<int>(appUser.HoppyUser.Select(c => c.HoppyId));

            UpdateNewAppUser updateNewAppUser = new UpdateNewAppUser()
            {
                Id = appUser.Id,
                FirstName = appUser.FirstName,
                GenderId = appUser.GenderId,
                LastName = appUser.LastName,
                ImageFile = appUser.ImageFile,
            };



            updateNewAppUser.HoppyChBoxVMs = new List<HoppyChBoxVM>();
            foreach (var hoppy in allHoppy)
            {
                updateNewAppUser.HoppyChBoxVMs.Add(new HoppyChBoxVM
                {
                    Id= hoppy.Id,
                    Name= hoppy.Name,
                    IsCheaked = userhoopy.Contains(hoppy.Id)
                });
            }
            ViewBag.hoppy = updateNewAppUser.HoppyChBoxVMs;
            return View(updateNewAppUser);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateUser(UpdateNewAppUser model)
        {
            string wroopath = _webHostEnvironment.WebRootPath;
            if (model.file != null)
            {
                if (model.ImageFile != null)
                {
                    var oldpath = Path.Combine(wroopath, ConstHelper.ConstHelper.UploadUserDiractory, model.ImageFile);
                    await _fileUploadService.DeleteFile(oldpath);
                }
                model.ImageFile = await _fileUploadService.UploadImage(model.file, ConstHelper.ConstHelper.UploadUserDiractory);
            }
            AppUser appUser = await _userManager.FindByIdAsync(model.Id);
            appUser.FirstName = model.FirstName;
            appUser.LastName = model.LastName;
            appUser.GenderId = model.GenderId;
            appUser.ImageFile = model.ImageFile;

            var result = await _userManager.UpdateAsync(appUser);
            if (result.Succeeded)
            {
                var hopyyuser = _context?.HoppyUsers?.Where(n => n.AppUserId == appUser.Id).ToList();
                _context?.HoppyUsers?.RemoveRange(hopyyuser);
                await _context?.SaveChangesAsync();

                foreach (var hoppy in model.HoppyChBoxVMs)
                {
                    if (hoppy.IsCheaked == true)
                    {
                        var hu = new HoppyUser() { AppUserId = appUser.Id, HoppyId = hoppy.Id };
                        _context.HoppyUsers.Add(hu);
                    }
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("Users");
            }
            foreach (var eror in result.Errors)
            {
                ModelState.AddModelError(string.Empty, eror.Description);
            }
            ViewBag.GenderId = new SelectList(_unitOfWork.Genders.GetAll(), "Id", "Name");
            return View(model);
        }
        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.Include(g => g.Gender).Include(uh=>uh.HoppyUser).ThenInclude(h=>h.Hoppy).ToListAsync();
            return View(users);
        }

        public async Task<IActionResult> UserCard()
        {
            var users = await _userManager.Users.Include(g => g.Gender).Include(uh => uh.HoppyUser).ThenInclude(h => h.Hoppy).ToListAsync();
            return View(users);
        }

        public async Task<IActionResult> Filter(string getuser)
        {
            var users = await _userManager.Users.Include(g => g.Gender).Include(uh => uh.HoppyUser).ThenInclude(h => h.Hoppy).ToListAsync();
            if (!string.IsNullOrEmpty(getuser) || !string.IsNullOrWhiteSpace(getuser))
            {
                var filterdata = users.Where(u => u.FirstName.StartsWith(getuser)).ToList();
                return View("UserCard", filterdata);
            }
            return View("UserCard", users);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null) 
        {
            return View(new LoginVM { ReturnUrl = returnUrl });
        }

        //public IActionResult Login(string? returnUrl = null)
        //{
        //    var model = new LoginVM { ReturnUrl = returnUrl };
        //    return View(model);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid) return View(model);
            var usr = await _userManager.FindByEmailAsync(model.Email);
            if (usr != null)
            {
                var passcheak = await _userManager.CheckPasswordAsync(usr, model.Password);
                if (passcheak)
                {
                    var result = await _signInManager.PasswordSignInAsync(usr, model.Password, model.RememberMe, false);
                    if (result.Succeeded)
                    {
                        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                        {
                            return Redirect(model.ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                    if (result.IsNotAllowed || result.IsLockedOut)
                    {
                        ModelState.AddModelError(string.Empty, "5");
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login1(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, true);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                if (result.IsNotAllowed || result.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "5");
                }
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            ViewData["GenderId"] = new SelectList(_context.Genders, "Id", "Name");
            ViewData["roles"] = new SelectList(_roleManager.Roles, "Id", "Name");
            ViewData["hoppyid"] = new SelectList(_context.Hoppies, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        [RequestFormLimits(MultipartBodyLengthLimit = 1085760)]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            var role = _roleManager.FindByIdAsync(model.roleName).Result;
            if (!ModelState.IsValid)
            {
                ViewData["roles"] = new SelectList(_roleManager.Roles, "Id", "Name", model.roleName);
                ViewData["GenderId"] = new SelectList(_context.Genders, "Id", "Name", model.GenderId);
                ViewData["hoppyid"] = new SelectList(_context.Hoppies, "Id", "Name", model.HoppyId);
                return View(model);
            }

            if (model.file != null)
            {
            model.ImageFile = await _fileUploadService.UploadImage(model.file, ConstHelper.ConstHelper.UploadUserDiractory);
            }
            AppUser appUser = new AppUser()
            {
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                GenderId = model.GenderId,
                ImageFile = model.ImageFile
            };
            var result = await _userManager.CreateAsync(appUser, model.Password);
            if (result.Succeeded)
            {
                foreach (var hoppy in model.HoppyId)
                {
                    var hoppyuser = new HoppyUser()
                    {
                        AppUserId = appUser.Id,
                        HoppyId = hoppy
                    };
                    _context.HoppyUsers.Add(hoppyuser);
                }
                _context.SaveChanges();
                await _userManager.AddToRoleAsync(appUser, role.Name);
                await _signInManager.SignInAsync(appUser, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            ViewData["roles"] = new SelectList(_roleManager.Roles, "Id", "Name", model.roleName);
            ViewData["GenderId"] = new SelectList(_context.Genders, "Id", "Name", model.GenderId);
            ViewData["hoppyid"] = new SelectList(_context.Hoppies, "Id", "Name", model.HoppyId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Update(string id)
        {
            AppUser usr = await _userManager.FindByIdAsync(id);
            AppUser? appUser = _context?.AppUsers?.Include(g => g.Gender).Include(uh => uh.HoppyUser).ThenInclude(h => h.Hoppy).FirstOrDefault(u=> u.Id == id);
            if (appUser == null) { return NotFound(); }

            UpdateVM updateVM = new UpdateVM()
            {
                FirstName = appUser.FirstName,
                LastName = appUser.LastName,
                ImageFile = appUser.ImageFile,
                GenderId = appUser.GenderId,
                HoppyId = appUser.HoppyUser?.Select(h => h.HoppyId).ToList()
            };
            ViewData["GenderId"] = new SelectList(_context?.Genders, "Id", "Name");
            ViewData["hoppyid"] = new SelectList(_context?.Hoppies, "Id", "Name");
            return View(updateVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Update(UpdateVM model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["GenderId"] = new SelectList(_context.Genders, "Id", "Name");
                ViewData["hoppyid"] = new SelectList(_context.Hoppies, "Id", "Name");
                return View(model);
            }

            string wroopath = _webHostEnvironment.WebRootPath;
                if (model.file != null && model.file.Length < 2000000)
                {
                    if (model.ImageFile != null)
                    {
                        var oldpath = Path.Combine(wroopath, ConstHelper.ConstHelper.UploadUserDiractory, model.ImageFile);
                        await _fileUploadService.DeleteFile(oldpath);
                    }
                    model.ImageFile = await _fileUploadService.UploadImage(model.file, ConstHelper.ConstHelper.UploadUserDiractory);
                }
                AppUser appUser = await _userManager.FindByIdAsync(model.Id);
                appUser.FirstName = model.FirstName;
                appUser.LastName = model.LastName;
                appUser.GenderId = model.GenderId;
                appUser.ImageFile = model.ImageFile;

                var result = await _userManager.UpdateAsync(appUser);
                if (result.Succeeded)
                {
                var hopyyuser = _context?.HoppyUsers?.Where(n => n.AppUserId == appUser.Id).ToList();
                _context?.HoppyUsers?.RemoveRange(hopyyuser);
                await _context?.SaveChangesAsync();

                var hu = new List<HoppyUser>();
                foreach (var hoppy in model.HoppyId)
                {               
                    var hoppyuser = new HoppyUser()
                    {
                        AppUserId = appUser.Id,
                        HoppyId = hoppy
                    };
                    hu.Add(hoppyuser);
                }
                await _context?.HoppyUsers?.AddRangeAsync(hu);
                await _context.SaveChangesAsync();

                return RedirectToAction("Users");
                }
                foreach (var eror in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, eror.Description);
                }
            ViewData["GenderId"] = new SelectList(_context.Genders, "Id", "Name", model.GenderId);
            ViewData["hoppyid"] = new SelectList(_context.Hoppies, "Id", "Name", model.HoppyId);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ManageRole(string userId)
        {
            ViewBag.userId = userId;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }
            ViewBag.UserName = user.UserName;
            var model = new List<ManageUserRolesViewModel>();
            foreach (var role in _roleManager.Roles)
            {
                var userRolesViewModel = new ManageUserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.Selected = true;
                }
                else
                {
                    userRolesViewModel.Selected = false;
                }
                model.Add(userRolesViewModel);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageRole(List<ManageUserRolesViewModel> model, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View();
            }
            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }
            result = await _userManager.AddToRolesAsync(user, model.Where(x => x.Selected).Select(y => y.RoleName));
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }
            return RedirectToAction("Users");
        }
    }
}
