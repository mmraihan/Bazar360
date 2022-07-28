using Bazar360.Data;
using Bazar360.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bazar360.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SpecialTagController : Controller
    {
        private readonly ApplicationDbContext _db;
        public SpecialTagController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View(_db.SpecialTags.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SpecialTag specialTag)
        {
            if (ModelState.IsValid) //Server Side Valid
            {
                _db.SpecialTags.Add(specialTag);
                await _db.SaveChangesAsync();

                TempData["save"] = "Special Tag saved successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(specialTag);
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var specialTag = _db.SpecialTags.Find(id);
            if (specialTag == null)
            {
                return NotFound();
            }
            return View(specialTag);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SpecialTag specialTag)
        {
            if (ModelState.IsValid) //Server Side Valid
            {
                _db.SpecialTags.Update(specialTag);
                await _db.SaveChangesAsync();

                TempData["edit"] = "Successfully Updated";

                return RedirectToAction(nameof(Index));
            }
            return View(specialTag);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var specialTag = _db.SpecialTags.Find(id);
            if (specialTag == null)
            {
                return NotFound();
            }
            return View(specialTag);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var specialTag = _db.SpecialTags.Find(id);
            if (specialTag == null)
            {
                return NotFound();
            }
            return View(specialTag);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<ActionResult> Delete(int? id, SpecialTag specialTag)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (id != specialTag.Id)
            {
                return NotFound();
            }

            var specialTagFromDb = _db.SpecialTags.Find(id);
            if (specialTagFromDb == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _db.SpecialTags.Remove(specialTagFromDb);
                await _db.SaveChangesAsync();

                TempData["delete"] = "Special Tag deleted successfully";

                return RedirectToAction(nameof(Index));
            }
            return View(specialTag);
        }
    }
}
