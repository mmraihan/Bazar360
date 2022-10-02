using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bazar360.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : Controller
    {

        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
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
           
            var result = await _roleManager.CreateAsync(identityRole);
            if (result.Succeeded)
            {
                TempData["save"] = "Role created successfully";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
    }
}
