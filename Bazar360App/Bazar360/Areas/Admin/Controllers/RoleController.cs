using Bazar360.Areas.Admin.ViewModels;
using Bazar360.Data;
using Bazar360.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bazar360.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : Controller
    {
        
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        private readonly ApplicationDbContext _user;
        public RoleController(RoleManager<IdentityRole> roleManager, ApplicationDbContext user, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _user = user;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();
            ViewBag.Roles = roles;
            return View();
        }

        public async Task <IActionResult> Create()
        {       
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name) 
        {
            IdentityRole identityRole = new IdentityRole()
            {
                Name = name
            };

            var isExist = await _roleManager.RoleExistsAsync(name);

            if (isExist)
            {
                ViewBag.message = "This role is already exist!";
                ViewBag.name = name;
                return View();
            }
           
            var result = await _roleManager.CreateAsync(identityRole);
            if (result.Succeeded)
            {
                TempData["save"] = "Role created successfully";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public async Task<IActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role==null)
            {
                return NotFound();
            }
            ViewBag.id = role.Id;
            ViewBag.name = role.Name;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, string name)
        {

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            role.Name = name;

            var isExist = await _roleManager.RoleExistsAsync(name);

            if (isExist)
            {
                ViewBag.message = "This role is already exist!";
                ViewBag.name = name;
                return View();
            }

            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                TempData["save"] = "Role updated successfully";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            ViewBag.id = role.Id;
            ViewBag.name = role.Name;

            return View();
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
           var result=await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                TempData["delete"] = "Role deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public async Task<IActionResult> Assign()
        {
            ViewData["userId"] = new SelectList(_user.ApplicationUsers.Where(c=>c.LockoutEnd==null).ToList(), "Id", "UserName");
            ViewData["roleId"] = new SelectList(_roleManager.Roles.ToList(), "Name", "Name"); //----Note 19
            return View();
                   
        }
        [HttpPost]
        public async Task<IActionResult> Assign(RoleUserVm roleUser)
        {
            var user = _user.ApplicationUsers.FirstOrDefault(c => c.Id == roleUser.UserId);
            if (user == null)
            {
                return NotFound();
            }
            var isRoleExist= await _userManager.IsInRoleAsync(user, roleUser.RoleId);
            if (isRoleExist)
            {
                ViewBag.message = "This role is already assigned!";
                ViewData["userId"] = new SelectList(_user.ApplicationUsers.Where(c => c.LockoutEnd == null).ToList(), "Id", "UserName");
                ViewData["roleId"] = new SelectList(_roleManager.Roles.ToList(), "Name", "Name"); //----Note 19
                return View();
            }

            var role = await _userManager.AddToRoleAsync(user, roleUser.RoleId); //----Note 19
            if (role.Succeeded)
            {
                TempData["save"] = "User Role assigned successfully";
                return RedirectToAction(nameof(Index));
            }
            return View();

        }



    }
}
