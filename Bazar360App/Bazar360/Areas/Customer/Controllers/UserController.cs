using Bazar360.Data;
using Bazar360.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bazar360.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class UserController : Controller
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _db;

        public UserController(UserManager<IdentityUser> userManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        public IActionResult Index()
        {
            return View(_db.ApplicationUsers.ToList());
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ApplicationUser user)
        {
            if (ModelState.IsValid) //--Note 14
            {
                var result = await _userManager.CreateAsync(user, user.PasswordHash); //----Note 13

                if (result.Succeeded)
                {
                    var isSaveRole = await _userManager.AddToRoleAsync(user, "User");
                    TempData["Save"] = "User created successfully";
                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

            }
           

            return View();
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(x => x.Id == id);
            if (user==null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ApplicationUser appUser)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(x => x.Id == appUser.Id);
            if (user == null)
            {
                return NotFound();
            }

            user.FirstName=appUser.FirstName;
            user.LastName = appUser.LastName;
            var result =  await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                TempData["save"] = "User updated successfully";
                return RedirectToAction(nameof(Index));

            }
            return View(user);
        }

        public async Task<IActionResult> Detail(string id)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public async Task<IActionResult> Lockout(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = _db.ApplicationUsers.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Lockout(ApplicationUser user)
        {
            if (user == null)
            {
                return NotFound();
            }
            var userInfo = _db.ApplicationUsers.FirstOrDefault(x => x.Id ==user.Id);
           
            if (userInfo == null)
            {
                return NotFound();
            }           
            userInfo.LockoutEnd = DateTime.Now.AddYears(200);
            var result= _db.SaveChanges();

            if (result>0)
            {
                TempData["save"] = "User locked successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(user);


        }


        public async Task<IActionResult> Active(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = _db.ApplicationUsers.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }


            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Active(ApplicationUser user)
        {
            if (user == null)
            {
                return NotFound();
            }
            var userInfo = _db.ApplicationUsers.FirstOrDefault(x => x.Id == user.Id);

            if (userInfo == null)
            {
                return NotFound();
            }
            userInfo.LockoutEnd = null;
            var result = _db.SaveChanges();

            if (result > 0)
            {
                TempData["save"] = "User Unlocked successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }



        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = _db.ApplicationUsers.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }


            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ApplicationUser user)
        {
            if (user == null)
            {
                return NotFound();
            }
            var userInfo = _db.ApplicationUsers.FirstOrDefault(x => x.Id == user.Id);

            if (userInfo == null)
            {
                return NotFound();
            }
            _db.ApplicationUsers.Remove(userInfo);
            var result = _db.SaveChanges();

            if (result > 0)
            {
                TempData["save"] = "User deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }
    }
}
